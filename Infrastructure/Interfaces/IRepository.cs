using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Interfaces
{
    public interface IRepository
    {

        #region Methods
        public IModel Model { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        public int SaveChanges();

        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IBaseEntity;

        // get all
        IQueryable<TEntity> GetAll<TEntity>(Action<IEntityCondition<TEntity>> config = null) where TEntity : class, IBaseEntity;

        // find by id
        IQueryable<TEntity> Find<TEntity>(Action<IEntityCondition<TEntity>> config = null) where TEntity : class, IBaseEntity;
     

        // exist
        Task<bool> Exist<TEntity>(Action<IEntityCondition<TEntity>> config = null) where TEntity : class, IBaseEntity;


        // Insert
        EntityEntry<TEntity> Insert<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;


        // Update
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;


        // Delete
        EntityEntry<TEntity> Delete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

        // Count
        Task<int> GetCountAsync<TEntity>(Action<IEntityCondition<TEntity>> config = null) where TEntity :class, IBaseEntity;

        #endregion
    }
}