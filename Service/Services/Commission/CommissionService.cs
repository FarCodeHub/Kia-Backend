using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;

namespace Service.Services.Commission
{
    public class CommissionService: ICommissionService
    {
        private readonly IRepository _repository;

        public CommissionService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.Commission> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Commission>();
        }

        public IQueryable<Domain.Entities.Commission> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Commission>(x => x.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Commission>> Add(Domain.Entities.Commission inpute)
        {
           return _repository.Insert(inpute);
        }

        public  async Task<EntityEntry<Domain.Entities.Commission>> Update(Domain.Entities.Commission inpute, CancellationToken cancellationToken)
        {
            var entity = await FindById(inpute.Id).FirstOrDefaultAsync(cancellationToken);
            entity.IsPaid = inpute.IsPaid;
            entity.PaidAt = inpute.PaidAt;
            entity.Amount = inpute.Amount;

            return _repository.Update(entity);
        }

        public async Task<EntityEntry<Domain.Entities.Commission>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await FindById(id).FirstOrDefaultAsync(cancellationToken);
            return _repository.Delete(entity);
        }
    }
}