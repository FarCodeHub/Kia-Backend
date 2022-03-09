using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Service.Interfaces
{
    public interface IRoleService : ICrudService<Domain.Entities.Role>, ISearchService<Domain.Entities.Role>
    {
        public Task<EntityEntry<Domain.Entities.Role>> Add(Domain.Entities.Role inpute, List<int> permissionsId);

        Task<EntityEntry<Domain.Entities.Role>> Update(Domain.Entities.Role inpute, List<int> permissionsId,
            CancellationToken cancellationToken);
    }
}