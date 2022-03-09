using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Service.Interfaces
{
    public interface IProjectService : ICrudService<Domain.Entities.Project>, ISearchService<Domain.Entities.Project>
    {
        Task<EntityEntry<Domain.Entities.Project>> Update(Domain.Entities.Project inpute,
            string projectFileReletiveAddress,
            CancellationToken cancellationToken);

        Task<EntityEntry<Domain.Entities.Project>> Add(Domain.Entities.Project inpute,
            string projectFileReletiveAddress);
    }
}