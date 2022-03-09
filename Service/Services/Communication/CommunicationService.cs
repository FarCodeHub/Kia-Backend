using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;

namespace Service.Services.Communication
{
    public class CommunicationService : ICommunicationService
    {
        private  IRepository _repository;
        private readonly ICaseService _caseService;
        public IRepository Repository { get => _repository; set => _repository = value; }

        public CommunicationService(IRepository repository, ICaseService caseService)
        {
            _repository = repository;
            _caseService = caseService;
        }

        public IQueryable<Domain.Entities.Communication> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Communication>();
        }

        public IQueryable<Domain.Entities.Communication> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Communication>(x => x.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Communication>> Add(Domain.Entities.Communication inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Communication>> Update(Domain.Entities.Communication inpute, CancellationToken cancellationToken)
        {
            var entity = await FindById(inpute.Id).FirstOrDefaultAsync(cancellationToken);

            return _repository.Update(entity);
        }

        public async Task<EntityEntry<Domain.Entities.Communication>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await FindById(id).FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }
    }
}