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

namespace Service.Services.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository _repository;

        public PermissionService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.Permission> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Permission>();
        }

        public IQueryable<Domain.Entities.Permission> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Permission>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Permission>> Add(Domain.Entities.Permission inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Permission>> Update(Domain.Entities.Permission inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Permission>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.ParentId = inpute.ParentId;
            entity.UniqueName = inpute.UniqueName;
            entity.Title = inpute.Title;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Permission>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Permission>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Permission> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Permission>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Permission>(queries))
                    .Paginate(pagination));
        }
    }
}