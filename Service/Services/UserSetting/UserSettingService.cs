using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Services.UserSetting
{
    public class UserSettingService : IUserSettingService
    {
        private readonly IRepository _repository;
        private readonly IHandledErrorManager _handledErrorManager;
        public UserSettingService(IRepository repository, IHandledErrorManager handledErrorManager)
        {
            _repository = repository;
            _handledErrorManager = handledErrorManager;
        }

        public async Task<List<Domain.Entities.UserSetting>> GetAllByUserId(int id, CancellationToken cancellationToken)
        {
            return await _repository.GetAll<Domain.Entities.UserSetting>(cfg =>
                cfg.ConditionExpression(x=>x.UserId == id))
                .ToListAsync(cancellationToken);
        }

        public async void Add(Domain.Entities.UserSetting inpute)
        {
             _repository.Insert(inpute);
        }

        public async void Update(Domain.Entities.UserSetting inpute, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.UserSetting>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
                .Update<Domain.Entities.UserSetting>(entity, inpute);

            _repository.Update(entity);
        }


        public async System.Threading.Tasks.Task SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.UserSetting>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

           var vs= _repository.Delete(entity);

        }

        public async System.Threading.Tasks.Task ChangeUserSetting(IdentityModel model)
        {
            var lastRoleId = await _repository
                .Find<Domain.Entities.UserSetting>(x => x.ConditionExpression(x => x.UserId == model.Id && x.KeyWord == "LastRoleId"))
                .FirstOrDefaultAsync();
            if (lastRoleId != null)
            {
                lastRoleId.Value = model.RoleId == 0 ? lastRoleId.Value : model.RoleId.ToString();
                _repository.Update(lastRoleId);
            }
            else
            {
                Add(new Domain.Entities.UserSetting()
                {
                    KeyWord = "LastRoleId",
                    Value = model.RoleId == 0
                        ? (await _repository.GetQuery<Domain.Entities.UserRole>().FirstOrDefaultAsync(x=>x.UserId == model.Id))?.RoleId.ToString() ?? throw _handledErrorManager.Throw<UserWithoutAnyRole>()
                        : model.RoleId.ToString(),
                    UserId = model.Id
                });
            }
            await _repository.SaveChangesAsync(cancellationToken: CancellationToken.None);
        }

        void IUserSettingService.SoftDelete(int id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}