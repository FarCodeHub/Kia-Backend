
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Service.Interfaces
{
    public interface IUserSettingService : IService
    {
        Task<List<Domain.Entities.UserSetting>> GetAllByUserId(int id, CancellationToken cancellationToken);

        void Add(Domain.Entities.UserSetting inpute);

        void Update(Domain.Entities.UserSetting inpute, CancellationToken cancellationToken);

        void SoftDelete(int id, CancellationToken cancellationToken);

        Task ChangeUserSetting(IdentityModel model);

    }
}