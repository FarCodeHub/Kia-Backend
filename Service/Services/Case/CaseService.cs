using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;
using Service.Services.BaseValue;

namespace Service.Services.Case
{
    public class CaseService : ICaseService
    {
        private  IRepository _repository;

        public IRepository Repository { get => _repository; set => _repository=value; }
        private readonly TaskResultsAndTypesSingleton _taskResultsAndTypesSingleton;
        public CaseService(IRepository repository)
        {
            _repository = repository;
            _taskResultsAndTypesSingleton = TaskResultsAndTypesSingleton.Instance(_repository);
        }

        public IQueryable<Domain.Entities.Case> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Case>();
        }

        public IQueryable<Domain.Entities.Case> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Case>(x => x.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Case>> Add(Domain.Entities.Case inpute)
        {
            inpute.IsOpen = true;
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Case>> Update(Domain.Entities.Case inpute, CancellationToken cancellationToken)
        {
            var entity = await FindById(inpute.Id).FirstOrDefaultAsync(cancellationToken);
            entity.StatusBaseId = inpute.StatusBaseId;
            entity.ConsultantId = inpute.ConsultantId;
            entity.PresentorId = inpute.PresentorId;
            return _repository.Update(entity);
        }

        public async Task<EntityEntry<Domain.Entities.Case>> Close(int id, int statusId,CancellationToken cancellationToken)
        {
            var entity = await FindById(id).FirstOrDefaultAsync(cancellationToken);
            entity.IsOpen = false;
            entity.ClosedAt = DateTime.Now;
            entity.StatusBaseId = statusId;

            foreach (var task in _repository.GetQuery<Domain.Entities.Task>()
                .Where(x=>
                    x.CustomerId == entity.CustomerId &&
                    x.CaseId == entity.Id &&
                    x.ResultBaseId == null))
            {
                task.ResultBaseId = _taskResultsAndTypesSingleton.CaseClose;
                task.EndAt = DateTime.Now;
                _repository.Update(task);
            }

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Case>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await FindById(id).FirstOrDefaultAsync(cancellationToken);
            return _repository.Delete(entity);
        }  
        
    }
}