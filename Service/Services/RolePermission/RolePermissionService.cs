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

namespace Service.Services.RolePermission
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRepository _repository;

        public RolePermissionService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.RolePermission> GetAll()
        {
            return _repository.GetAll<Domain.Entities.RolePermission>();
        }

        public IQueryable<Domain.Entities.RolePermission> GetByPermissionIdAndRoleId(int permissionId, int roleId)
        {
            return _repository.Find<Domain.Entities.RolePermission>(cfg =>
                cfg.ConditionExpression(x => x.PermissionId == permissionId && x.RoleId == roleId));
        }

        public IQueryable<Domain.Entities.RolePermission> FindById(int id)
        {
            return _repository.Find<Domain.Entities.RolePermission>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.RolePermission>> Add(Domain.Entities.RolePermission inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.RolePermission>> Update(Domain.Entities.RolePermission inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.RolePermission>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
                .Update<Domain.Entities.RolePermission>(entity, inpute);

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.RolePermission>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.RolePermission>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.RolePermission> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.RolePermission>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.RolePermission>(queries))
                    .Paginate(pagination));
        }
    }
}