using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Task = Domain.Entities.Task;

namespace Service.Interfaces
{
    public interface IContractService : ICrudService<Domain.Entities.Contract>, ISearchService<Domain.Entities.Contract>
    {
        Task<IQueryable<Task>> GetAllInvolved(int taskId, int contractTaskId);

        Task<EntityEntry<Domain.Entities.Contract>> Add(Domain.Entities.Contract inpute,
            string[] requestAttachmentsUrl);

        Task<EntityEntry<Domain.Entities.Contract>> Update(Domain.Entities.Contract inpute,
            ICollection<int> involvedEployeesId,
            string[] requestAttachmentsUrl,
            CancellationToken cancellationToken);

        Task<EntityEntry<Domain.Entities.Contract>> Cancle(int id, CancellationToken cancellationToken);

    }
}