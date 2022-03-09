using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;

namespace Service.Services.ContractProject
{
    public class ContractProjectService : IContractProjectService
    {
        private readonly IRepository _repository;

        public ContractProjectService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.ContractProject> GetAll()
        {
            return _repository.GetQuery<Domain.Entities.ContractProject>();
        }

        public IQueryable<Domain.Entities.ContractProject> FindById(int id)
        {
            return _repository.Find<Domain.Entities.ContractProject>(x => x.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.ContractProject>> Add(Domain.Entities.ContractProject inpute)
        {
            return _repository.Insert(inpute);
        }

        public  async Task<EntityEntry<Domain.Entities.ContractProject>> Update(Domain.Entities.ContractProject inpute, CancellationToken cancellationToken)
        {
            var entity =await FindById(inpute.Id).FirstOrDefaultAsync(cancellationToken);
            return _repository.Update(entity);
        }

        public async Task<EntityEntry<Domain.Entities.ContractProject>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await FindById(id).FirstOrDefaultAsync(cancellationToken);
            return _repository.Delete(entity);
        }
    }
}