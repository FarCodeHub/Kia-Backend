using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Service.Interfaces
{
    public interface IConfigurationService : IService
    {
        public IRepository Repository { get; set; }
        IQueryable<Domain.Entities.Configuration> GetByKey(string key);
        Task<EntityEntry<Domain.Entities.Configuration>> Update(Domain.Entities.Configuration input);
    }
}