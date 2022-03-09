using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;

namespace Service.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;
        private readonly IRoleService _roleService;
        private readonly IBaseValueService _baseValueService;
        private readonly IUserSettingService _userSettingService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IHandledErrorManager _handledErrorManager;
        public UserService(IRepository repository, IRoleService roleService, IUserSettingService userSettingService, IBaseValueService baseValueService, ICurrentUserAccessor currentUserAccessor, IHandledErrorManager handledErrorManager)
        {
            _repository = repository;
            _roleService = roleService;
            _userSettingService = userSettingService;
            _baseValueService = baseValueService;
            _currentUserAccessor = currentUserAccessor;
            _handledErrorManager = handledErrorManager;
        }

        public IQueryable<Domain.Entities.User> GetAll()
        {
            return _repository.GetAll<Domain.Entities.User>();
        }

        public IQueryable<Domain.Entities.User> FindById(int id)
        {
            return _repository.Find<Domain.Entities.User>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.User>> Add(Domain.Entities.User inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.User>> Update(Domain.Entities.User inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.User>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            if (!string.IsNullOrEmpty(inpute.Password))
            {
                entity.Password = Encryption.Symmetric.CreateMd5Hash(inpute.Password);
            }
            entity.Username = inpute.Username;
            entity.IsBlocked = inpute.IsBlocked;
            entity.BlockedReason = inpute.BlockedReason;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.User>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.User>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }


        public async Task<Domain.Entities.User> GetUserByUsername(string Username)
        {
            return await _repository.Find<Domain.Entities.User>(x =>
                x.ConditionExpression(x => x.Username == Username))
                .FirstOrDefaultAsync();
        }

        public async Task<Domain.Entities.User> GetUserById(int id)
        {
            return await _repository.Find<Domain.Entities.User>(x =>
                    x.ObjectId(id))
                .Include(x => x.Person)
                .ThenInclude(x => x.Employee)
                .ThenInclude(x => x.Operator)
                .Include(x => x.Person.Employee.UnitPosition)
                .Include(x => x.Person.Employee.UnitPosition.Unit)
                .Include(x => x.Person.Employee.UnitPosition.Position)
                .FirstOrDefaultAsync();
        }


        public async Task<Domain.Entities.User> GetUserByUserPass(string Username, string password)
        {
            var user = await _repository.Find<Domain.Entities.User>(cfg =>
                    cfg.ConditionExpression(x => x.Username == Username))
                .Include(x => x.Person)
                .ThenInclude(x => x.Employee)
                .ThenInclude(x => x.Operator)
                .Include(x => x.Person.Employee.UnitPosition)
                .Include(x => x.Person.Employee.UnitPosition.Unit)
                .Include(x => x.Person.Employee.UnitPosition.Position)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                _handledErrorManager.Throw<NotFount>();
            }

            if (user.IsBlocked)
            {
                _handledErrorManager.Throw<UserIsBlocked>();
            }
            if (Encryption.Symmetric.VerifyHash(user.Password, password))
            {
                user.FailedCount = 0;
            }
            else
            {
                user.FailedCount++;
            }

            if (user.FailedCount > 3)
            {
                user.IsBlocked = true;
                user.BlockedReason = "wrong password more than 3 times";
                _repository.Update<Domain.Entities.User>(user);
            }
            await _repository.SaveChangesAsync();

            if (user.FailedCount > 0)
            {
                throw _handledErrorManager.Throw<UserPassWrong>();
            }

            return user;
        }


        public async System.Threading.Tasks.Task Upate(Domain.Entities.User user)
        {
            var entity = await _repository
                .Find<Domain.Entities.User>(c =>
                    c.ObjectId(user.Id))
                .FirstOrDefaultAsync();

            entity = new DynamicUpdator(_repository.Model)
                .Update<Domain.Entities.User>(entity, user);

            _repository.Update(entity);
        }

        public async Task<IQueryable<MenuItem>> GetUserMenues(int userId)
        {
            return (
                 from userrole in _repository.GetQuery<Domain.Entities.UserRole>()
                 join role in _repository.GetQuery<Domain.Entities.Role>()
                     on userrole.RoleId equals role.Id
                 join rolepermission in _repository.GetQuery<Domain.Entities.RolePermission>()
                     on role.Id equals rolepermission.RoleId
                 join permission in _repository.GetQuery<Domain.Entities.Permission>()
                     on rolepermission.PermissionId equals permission.Id
                 join m in _repository.GetQuery<Domain.Entities.MenuItem>()
                     on permission.Id equals m.PermissionId
                 where userrole.UserId == userId
                 select m
             ).OrderBy(x => x.OrderIndex);
        }

        public async Task<List<Claim>> GetClaims(IdentityModel identityModel, Domain.Entities.User user, CancellationToken cancellationToken = new CancellationToken())
        {
            var permissions = await (
                from userrole in _repository.GetQuery<Domain.Entities.UserRole>()
                join role in _repository.GetQuery<Domain.Entities.Role>()
                    on userrole.RoleId equals role.Id
                join rolepermission in _repository.GetQuery<Domain.Entities.RolePermission>()
                    on role.Id equals rolepermission.RoleId
                join permission in _repository.GetQuery<Domain.Entities.Permission>()
                    on rolepermission.PermissionId equals permission.Id
                where userrole.UserId == identityModel.Id
                select new { permission.UniqueName, userrole }
            ).ToListAsync(cancellationToken);

            var gender =
                (await _baseValueService.FindById(user.Person.GenderBaseId ?? 1).FirstOrDefaultAsync(cancellationToken)).Title;

            var claims = new List<Claim>
            {
                new("Id", user.Id.ToString()),
                new("Username", user.Username),
                new("FullName", user.Person.FirstName + " " + user.Person.LastName),
                new("OperatorId", user.Person.Employee.Operator?.Id.ToString() ?? ""),
                new("ExtentionNumber", user.Person.Employee?.Operator?.ExtentionNumber ?? ""),
                new("QueueNumber", user.Person.Employee?.Operator?.QueueNumber ?? ""),
                new("EmployeeId", user.Person.Employee.Id.ToString() ?? ""),
                new("UnitPositionId", user.Person.Employee.UnitPositionId.ToString() ?? ""),
                new("UnitTitle", user.Person.Employee.UnitPosition.Unit.Title ?? ""),
                new("PositionTitle",  user.Person.Employee.UnitPosition.Position.Title ?? ""),
                new("PositionUniqueName",  user.Person.Employee.UnitPosition.Position.UniqueName ?? ""),
                new("UnitId", user.Person.Employee.UnitPosition.UnitId.ToString()),
                new("PositionId", user.Person.Employee.UnitPosition.PositionId.ToString()),
                new("Gender", gender ?? "")
            };

            if (identityModel.RoleId is not 0)
            {
                claims.Add(new Claim("OwnerRoleId", identityModel.RoleId.ToString()));
                claims.Add(new Claim("UserRoleName", (await _roleService.FindById(identityModel.RoleId).FirstOrDefaultAsync(cancellationToken)).Title));
                claims.Add(new Claim("LevelCode", (await _roleService.FindById(identityModel.RoleId).FirstOrDefaultAsync(cancellationToken)).LevelCode));
            }
            else
            {
                var lastRoleId = (await _userSettingService.GetAllByUserId(user.Id, cancellationToken))
                    .FirstOrDefault(x => x.UserId == user.Id && x.KeyWord == "LastRoleId");

                if (lastRoleId is null)
                {
                    lastRoleId = _repository.Insert(new Domain.Entities.UserSetting()
                    {
                        Value = permissions.Select(x => x.userrole.RoleId).FirstOrDefault().ToString(),
                        KeyWord = "LastRoleId",
                        UserId = user.Id
                    }).Entity;
                    await _repository.SaveChangesAsync(cancellationToken);
                }


                claims.Add(new Claim("OwnerRoleId", lastRoleId.Value.ToString()));
                var currentRole = await _roleService.FindById(int.Parse(lastRoleId.Value!)).FirstOrDefaultAsync(cancellationToken);
                claims.Add(new Claim("UserRoleName", currentRole.Title));
                claims.Add(new Claim("LevelCode", currentRole.LevelCode));

            }
            claims.AddRange(permissions.Select(permission => new Claim(ClaimTypes.Role, permission.UniqueName)));

            return claims;
        }
    }
}