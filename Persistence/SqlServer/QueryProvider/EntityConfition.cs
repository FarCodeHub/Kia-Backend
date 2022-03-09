using System;
using System.Linq.Expressions;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Infrastructure.Interfaces;


// ReSharper disable InconsistentNaming

namespace Persistence.SqlServer.QueryProvider
{
    public class EntityConfition<TEntity> : IEntityCondition<TEntity> where TEntity : IBaseEntity
    {
        internal Expression<Func<TEntity, bool>> _condition = null;

        internal bool _isDeletedIncluded = false;
        internal bool _asNoTraking = false;
        internal object _objectId = null;
        internal IModel _entityType = default;
        internal Pagination _pagination = default;

        public IEntityCondition<TEntity> ConditionExpression(Expression<Func<TEntity, bool>> conditionExpression)
        {
            _condition = conditionExpression;
            return this;
        }

        public IEntityCondition<TEntity> Paginate(Pagination pagination)
        {
            _pagination = pagination;
            return this;
        }


        public IEntityCondition<TEntity> IsDeletedIncluded(bool isDeletedIncluded)
        {
            _isDeletedIncluded = isDeletedIncluded;
            return this;
        }

        public IEntityCondition<TEntity> AsNoTraking(bool asNoTraking)
        {
            _asNoTraking = asNoTraking;
            return this;
        }

        public IEntityCondition<TEntity> ObjectId(object objectId, IModel entityType = default)
        {
            _objectId = objectId;
            _entityType = entityType;
            return this;
        }

    }
}