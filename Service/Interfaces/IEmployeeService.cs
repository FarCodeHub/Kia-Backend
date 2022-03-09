using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Service.Interfaces
{
    public interface IEmployeeService : ICrudService<Domain.Entities.Employee>, ISearchService<Domain.Entities.Employee>
    {
        IRepository Repository { get; set; }

        Task<EntityEntry<Domain.Entities.Employee>> Add(Domain.Entities.Employee inpute);

        Task<EntityEntry<Domain.Entities.Employee>> Update(Domain.Entities.Employee inpute,
            Domain.Entities.Person personInput, string profilePhotoUrl,
            CancellationToken cancellationToken);

        Task<Domain.Entities.Employee> FindByPositionUniqueName(string positionUniqueName);

    }
}