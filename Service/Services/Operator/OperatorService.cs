using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;

namespace Service.Services.Operator
{
    public class OperatorService : IOperatorService
    {
        private readonly IRepository _repository;
        private readonly IHandledErrorManager _handledErrorManager;
        public OperatorService(IRepository repository, IHandledErrorManager handledErrorManager)
        {
            _repository = repository;
            _handledErrorManager = handledErrorManager;
        }

        public IQueryable<Domain.Entities.Operator> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Operator>();
        }

        public IQueryable<Domain.Entities.Operator> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Operator>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Operator>> Add(Domain.Entities.Operator inpute)
        {
            inpute.QueueNumber = $"1{inpute.ExtentionNumber}";
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Operator>> Update(Domain.Entities.Operator inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Operator>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.EmployeeId = inpute.EmployeeId;
            entity.ExtentionNumber = inpute.ExtentionNumber;
            entity.IsActive = inpute.IsActive;
            entity.QueueNumber = $"1{inpute.ExtentionNumber}";


            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Operator>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Operator>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Operator> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Operator>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Operator>(queries))
                    .Paginate(pagination));
        }
    }
}