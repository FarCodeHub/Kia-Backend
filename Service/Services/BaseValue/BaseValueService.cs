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

namespace Service.Services.BaseValue
{
    public class BaseValueService : IBaseValueService
    {
        private readonly IRepository _repository;

        public BaseValueService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.BaseValue> GetAll()
        {
            return _repository.GetAll<Domain.Entities.BaseValue>();
        }

        public IQueryable<Domain.Entities.BaseValue> GetAllByBaseValueTypeId(int baseValuetypeId)
        {
            return _repository.GetAll<Domain.Entities.BaseValue>(cfg =>
                cfg.ConditionExpression(x=>x.BaseValueTypeId == baseValuetypeId));
        }

        public IQueryable<Domain.Entities.BaseValue> GetAllByUniqueName(string uniqueName)
        {
            return _repository.GetAll<Domain.Entities.BaseValue>(cfg =>
                cfg.ConditionExpression(x=>x.UniqueName == uniqueName));
        }

        public IQueryable<Domain.Entities.BaseValue> FindById(int id)
        {
            return _repository.Find<Domain.Entities.BaseValue>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.BaseValue>> Add(Domain.Entities.BaseValue inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.BaseValue>> Update(Domain.Entities.BaseValue inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.BaseValue>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Value = inpute.Value;
            entity.Title = inpute.Title;
            entity.UniqueName = inpute.UniqueName;
            entity.IsReadOnly = inpute.IsReadOnly;
            entity.OrderIndex = inpute.OrderIndex;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.BaseValue>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.BaseValue>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.BaseValue> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.BaseValue>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.BaseValue>(queries))
                    );
        }

        public IQueryable<Domain.Entities.BaseValue> GetAllByBaseValueTypeUniqueName(string uniquename )
        {
           
            return _repository
                .GetAll<Domain.Entities.BaseValue>(c =>
                    c.ConditionExpression(x => x.BaseValueType.UniqueName == uniquename));
        }

        public IQueryable<Domain.Entities.BaseValue> GetByBaseValueUniqueName(string uniquename)
        {
            return _repository
                .Find<Domain.Entities.BaseValue>(c =>
                    c.ConditionExpression(x => x.UniqueName == uniquename));
        }

    }
}