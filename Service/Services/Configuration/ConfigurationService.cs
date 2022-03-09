using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;

namespace Service.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private IRepository _repository;
        public IRepository Repository { get => _repository; set => _repository = value; }

        public ConfigurationService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.Configuration> GetByKey(string key)
        {
            return _repository.GetAll<Domain.Entities.Configuration>(x => x.ConditionExpression(x => x.Key == key));
        }

        public async Task<EntityEntry<Domain.Entities.Configuration>> Update(Domain.Entities.Configuration input)
        {
            var entity = await _repository.GetQuery<Domain.Entities.Configuration>()
                .FirstOrDefaultAsync(x => x.Key == input.Key);
            entity.Value = input.Value;
            return _repository.Update(entity);
        }
    }
}