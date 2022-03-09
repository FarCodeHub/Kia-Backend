using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Interfaces
{
    public interface ICrudService<T> : IService where T : class, IBaseEntity
    {
        IQueryable<T> GetAll();

        IQueryable<T> FindById(int id);

        Task<EntityEntry<T>> Add(T inpute);

        Task<EntityEntry<T>> Update(T inpute, CancellationToken cancellationToken);

        Task<EntityEntry<T>> SoftDelete(int id, CancellationToken cancellationToken);
    }
}