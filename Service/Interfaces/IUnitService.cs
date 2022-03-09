using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Service.Interfaces
{
    public interface IUnitService : ICrudService<Domain.Entities.Unit>, ISearchService<Domain.Entities.Unit>
    {
        IQueryable<Unit> GetAllByBranchId(int branchId);
        public Task<EntityEntry<Domain.Entities.Unit>> Add(Domain.Entities.Unit inpute, List<int> positionsId);

        public Task<EntityEntry<Domain.Entities.Unit>> Update(Domain.Entities.Unit inpute, List<int> positionsId,
            CancellationToken cancellationToken);
    }
}