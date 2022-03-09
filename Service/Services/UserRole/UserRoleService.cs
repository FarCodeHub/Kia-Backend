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

namespace Service.Services.UserRole
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IRepository _repository;

        public UserRoleService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.UserRole> GetAll()
        {
            return _repository.GetAll<Domain.Entities.UserRole>();
        }

        public IQueryable<Domain.Entities.UserRole> FindById(int id)
        {
            return _repository.Find<Domain.Entities.UserRole>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.UserRole>> Add(Domain.Entities.UserRole inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.UserRole>> Add(Domain.Entities.UserRole inpute,
            string[] requestAttachmentsUrl)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.UserRole>> Update(Domain.Entities.UserRole inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.UserRole>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
                .Update<Domain.Entities.UserRole>(entity, inpute);

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.UserRole>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.UserRole>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.UserRole> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.UserRole>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.UserRole>(queries))
                    .Paginate(pagination));
        }
    }
}