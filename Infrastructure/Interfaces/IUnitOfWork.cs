using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IModel Model();
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity;
        Task<int> SaveAsync(CancellationToken cancellationToken);
        public int Save();
        public DbContext Context();


    }
}