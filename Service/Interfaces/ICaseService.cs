using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Service.Interfaces
{
    public interface ICaseService : ICrudService<Case>
    {
        IRepository Repository { get; set; }
        Task<EntityEntry<Domain.Entities.Case>> Close(int id, int statusId,CancellationToken cancellationToken);

    }
}