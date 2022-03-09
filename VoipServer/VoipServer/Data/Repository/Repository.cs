using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Attributes;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.SqlServer.QueryProvider;

namespace VoipServer.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IHierarchicalController _hierarchicalController;
        private readonly IHandledErrorManager _handledErrorManager;
        public Repository(IUnitOfWork unitOfWork,
            ICurrentUserAccessor currentUserAccessor, IHierarchicalController hierarchicalController, IHandledErrorManager handledErrorManager)
        {
            _unitOfWork = unitOfWork;
            _currentUserAccessor = currentUserAccessor;
            this.Model = _unitOfWork.Model();
            _hierarchicalController = hierarchicalController;
            _handledErrorManager = handledErrorManager;
        }

        public IModel Model { get; set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.SaveAsync(cancellationToken);
        }

        public int SaveChanges()
        {
            return _unitOfWork.Save();
        }


        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IBaseEntity
        {
            return Persistence.SqlServer.QueryProvider.QueryProvider.GetQuery<TEntity>(_unitOfWork, _currentUserAccessor);
        }


        public IQueryable<TEntity> GetAll<TEntity>(Action<IEntityCondition<TEntity>> config) where TEntity : class, IBaseEntity
        {
            return config.QueryBuilder(_unitOfWork, _currentUserAccessor);
        }


        public IQueryable<TEntity> Find<TEntity>(Action<IEntityCondition<TEntity>> config)
            where TEntity : class, IBaseEntity
        {
            return config.QueryBuilder(_unitOfWork, _currentUserAccessor);
        }


        public async Task<bool> Exist<TEntity>(Action<IEntityCondition<TEntity>> config) where TEntity : class, IBaseEntity
        {
            return await config.QueryBuilder(_unitOfWork, _currentUserAccessor).AnyAsync();
        }

        public EntityEntry<TEntity> Insert<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (entity is null)  _handledErrorManager.Throw<NotFount>();
            entity.ModifiedAt = DateTime.Now;
            entity.CreatedAt = DateTime.Now;
            entity.ModifiedById = 1;
            entity.CreatedById = 1;
            entity.IsDeleted = false;
            entity.OwnerRoleId = 1;

            if (typeof(IHierarchical).IsAssignableFrom(typeof(TEntity)))
            {
                ((IHierarchical)entity).LevelCode = "0";
            }

            return _unitOfWork.Set<TEntity>().Add(entity);
        }

        public EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            entity.ModifiedById = 1;
            entity.ModifiedAt = DateTime.Now;

            entity = _hierarchicalController.UpdateLevelCode(entity).GetAwaiter().GetResult();

            return _unitOfWork.Set<TEntity>().Update(entity);
        }

        public EntityEntry<TEntity> Delete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            if (entity is null) _handledErrorManager.Throw<NotFount>();
            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedById = 1;

            if (typeof(TEntity).GetCustomAttributes(false).Any(x => x.GetType() == typeof(HasUniqueIndex)))
            {
                foreach (var propertyInfo in typeof(TEntity).GetProperties().Where(x => x.GetCustomAttributes(false).Any(x => x.GetType() == typeof(UniqueIndex))))
                {
                    var findEntityType = Model.FindEntityType(typeof(TEntity));
                    var maxChar = 0;
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        maxChar = findEntityType.GetProperty(propertyInfo.Name).GetMaxLength() - (propertyInfo.GetValue(entity) as string)?.Length ?? 0;
                    }

                    propertyInfo.SetValue(entity, propertyInfo.GetValue(entity) + RandomMaker.GenerateRandomString(maxChar));
                }
            }

            return _unitOfWork.Set<TEntity>().Update(entity);
        }


        public async Task<int> GetCountAsync<TEntity>(Action<IEntityCondition<TEntity>> config) where TEntity : class, IBaseEntity
        {
            return await config.QueryBuilder(_unitOfWork, _currentUserAccessor).CountAsync();

        }
    }
}