using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Service.Interfaces
{
    public interface IPersonService : ICrudService<Domain.Entities.Person>, ISearchService<Domain.Entities.Person>
    {
        IRepository Repository { get; set; }

        Task<EntityEntry<Domain.Entities.Person>> Update(Domain.Entities.Person inpute,
            string profilePhotoUrl, bool fullUpdate,
            CancellationToken cancellationToken);


        Task<EntityEntry<Domain.Entities.Person>>
            Add(Domain.Entities.Person inpute, string profilePhotoUrl);

        Task<EntityEntry<Domain.Entities.Person>> UpdateAvatar(int id, string profilePhotoUrl,
            CancellationToken cancellationToken);



    }
}