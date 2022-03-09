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

namespace Service.Services.BaseValueType
{
    public class BaseValueTypeService : IBaseValueTypeService
    {
        private readonly IRepository _repository;

        public BaseValueTypeService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.BaseValueType> GetAll()
        {
            return _repository.GetQuery<Domain.Entities.BaseValueType>();
        }

        public IQueryable<Domain.Entities.BaseValueType> FindById(int id)
        {
            return _repository.Find<Domain.Entities.BaseValueType>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.BaseValueType>> Add(Domain.Entities.BaseValueType inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.BaseValueType>> Update(Domain.Entities.BaseValueType inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.BaseValueType>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.SubSystem = inpute.SubSystem;
            entity.Title = inpute.Title;
            entity.ParentId = inpute.ParentId;
            entity.UniqueName = inpute.UniqueName;
            entity.GroupName = inpute.GroupName;
            entity.IsReadOnly = inpute.IsReadOnly;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.BaseValueType>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.BaseValueType>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.BaseValueType> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.BaseValueType>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.BaseValueType>(queries))
                    .Paginate(pagination));
        }
    }
}