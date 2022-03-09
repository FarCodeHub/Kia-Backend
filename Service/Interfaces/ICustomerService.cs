using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Service.Interfaces
{
    public interface ICustomerService : ICrudService<Domain.Entities.Customer>
    {
        IRepository Repository { get; set; }

        Task<EntityEntry<Domain.Entities.Customer>> Add(Domain.Entities.Customer inpute,
            Domain.Entities.Person inputePerson);

        Task<EntityEntry<Domain.Entities.Customer>> AddWithOutCase(Domain.Entities.Customer inpute);

        Task<EntityEntry<Domain.Entities.Customer>> Update(Domain.Entities.Customer inpute,
            Domain.Entities.Person personInput,bool fullUpdate,
            CancellationToken cancellationToken);
        
        Task<EntityEntry<Domain.Entities.Customer>> Merge(int idBase, int idMerge);

        Task<EntityEntry<Domain.Entities.Customer>> Block(int id,
            CancellationToken cancellationToken);


    }
}