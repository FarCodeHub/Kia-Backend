using System.Linq;
using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IRolePermissionService : ICrudService<Domain.Entities.RolePermission>, ISearchService<Domain.Entities.RolePermission>
    {
        public IQueryable<Domain.Entities.RolePermission> GetByPermissionIdAndRoleId(int permissionId, int roleId);

    }
}