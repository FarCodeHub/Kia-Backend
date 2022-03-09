using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.SqlServer;
using Service.Interfaces;

namespace $rootnamespace$.Services.$fileinputname$
{
    public class $fileinputname$Service : I$fileinputname$Service
    {
        private readonly IRepository _repository;

        public $fileinputname$Service(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.$fileinputname$> GetAll(Pagination pagination)
        {
           return _repository.GetAll<Domain.Entities.$fileinputname$>(cfg =>
                    cfg.Paginate(pagination));
        }

        public IQueryable<Domain.Entities.$fileinputname$> FindById(int id)
        {
            return _repository.Find<Domain.Entities.$fileinputname$>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.$fileinputname$>> Add(Domain.Entities.$fileinputname$ inpute)
        {
           return await _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.$fileinputname$>> Update(Domain.Entities.$fileinputname$ inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.$fileinputname$>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
                .Update<Domain.Entities.$fileinputname$>(entity, inpute);

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.$fileinputname$>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.$fileinputname$>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

           return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.$fileinputname$> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.$fileinputname$>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.$fileinputname$>(queries))
                    .Paginate(pagination));
        }
    }
}