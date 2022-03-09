using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Service.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRepository _repository;
        private readonly IRolePermissionService _rolePermissionService;

        public RoleService(IRepository repository, IRolePermissionService rolePermissionService)
        {
            _repository = repository;
            _rolePermissionService = rolePermissionService;
        }

        public IQueryable<Domain.Entities.Role> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Role>();
        }

        public IQueryable<Domain.Entities.Role> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Role>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Role>> Add(Domain.Entities.Role inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Role>> Add(Domain.Entities.Role inpute,List<int> permissionsId)
        {
            var role = _repository.Insert(inpute);
            foreach (var permission in permissionsId)
            {
                await _rolePermissionService.Add(new Domain.Entities.RolePermission() { PermissionId = permission, Role = role.Entity });
            }

            return role;
        }

        public async Task<EntityEntry<Domain.Entities.Role>> Update(Domain.Entities.Role inpute, List<int> permissionsId,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Role>(c =>
                    c.ObjectId(inpute.Id)).Include(x => x.RolePermissionRoles)
                .FirstOrDefaultAsync(cancellationToken);

            entity.ParentId = inpute.ParentId;
            entity.UniqueName = inpute.UniqueName;
            entity.Title = inpute.Title;
            entity.Description = inpute.Description;

            foreach (var removedPermission in entity.RolePermissionRoles.Select(x => x.PermissionId).Except(permissionsId))
            {
                var deletingEntity = await _rolePermissionService.GetByPermissionIdAndRoleId(removedPermission, entity.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                await _rolePermissionService.SoftDelete(deletingEntity.Id, cancellationToken);
            }

            foreach (var addedPermission in permissionsId.Except(entity.RolePermissionRoles.Select(x => x.PermissionId)))
            {
                await _rolePermissionService.Add(new Domain.Entities.RolePermission()
                {
                    RoleId = entity.Id,
                    PermissionId = addedPermission
                });
            }

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Role>> Update(Domain.Entities.Role inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Role>(c =>
                    c.ObjectId(inpute.Id)).Include(x => x.RolePermissionRoles)
                .FirstOrDefaultAsync(cancellationToken);

            entity.ParentId = inpute.ParentId;
            entity.UniqueName = inpute.UniqueName;
            entity.Title = inpute.Title;
            entity.Description = inpute.Description;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Role>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Role>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Role> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Role>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Role>(queries))
                    .Paginate(pagination));
        }
    }
}