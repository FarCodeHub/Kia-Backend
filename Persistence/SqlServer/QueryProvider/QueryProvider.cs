using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Infrastructure.Attributes;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Interfaces;
using System.Linq.Dynamic.Core;
using Infrastructure.Models;

namespace Persistence.SqlServer.QueryProvider
{
    public static class QueryProvider
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(IUnitOfWork unitOfWork, ICurrentUserAccessor currentUserAccessor) where TEntity : class, IBaseEntity
        {
            if (typeof(TEntity).GetCustomAttributes(false).Any(x => x.GetType() == typeof(AccessLimitControll)))
            {
                return (from all in unitOfWork.Set<TEntity>()
                        join r in unitOfWork.Set<Role>() on all.OwnerRoleId equals r.Id
                        where r.LevelCode.StartsWith(currentUserAccessor.GetRoleLevelCode())
                        select all);
            }

            return unitOfWork.Set<TEntity>();
        }

        public static IQueryable<TEntity> QueryBuilder<TEntity>(this Action<IEntityCondition<TEntity>>? config, IUnitOfWork unitOfWork, ICurrentUserAccessor currentUserAccessor) where TEntity : class, IBaseEntity
        {
            if (config == null)
            {
                return GetQuery<TEntity>(unitOfWork, currentUserAccessor);
            }
            var condition = new EntityConfition<TEntity>();
            config.Invoke(condition);

            return GetQuery<TEntity>(unitOfWork, currentUserAccessor)
                .ObjectId(condition._objectId, unitOfWork.Model())
                .Condition(condition._condition)
                .Paginate(condition._pagination)
                .IncludeDeleted(condition._isDeletedIncluded)
                .Traking(condition._asNoTraking);
        }



        public static IQueryable<TEntity> Condition<TEntity>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> conditionExpression)
            where TEntity : IBaseEntity
        {
            if (conditionExpression is null) { return query; }
            return query.Where(conditionExpression);
        }



        public static IQueryable<TEntity> IncludeDeleted<TEntity>(this IQueryable<TEntity> query,
            bool isDeletedIncluded)
        where TEntity : IBaseEntity
        {
            return isDeletedIncluded ? query : query.Where(x => x.IsDeleted != true);
        }


        public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query,
            Pagination pagination)
        {
            if (pagination is null)
            {
                return query;
            }
            return pagination.Take == 0 ? query : query.Skip(pagination.Skip).Take(pagination.Take);
        }


        public static IQueryable<TEntity> Traking<TEntity>(this IQueryable<TEntity> query,
            bool asNoTraking)
        where TEntity : IBaseEntity
        {
            return (IQueryable<TEntity>)(!asNoTraking ? query : query.AsNoTracking());
        }


        public static IQueryable<TEntity> ObjectId<TEntity>(this IQueryable<TEntity> query,
            object id, IModel model)
            where TEntity : IBaseEntity
        {

            if (id is null || model is null)
            {
                return query;
            }

            var entityType = model.FindEntityType(typeof(TEntity));
            var primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();
            var primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primayKeyValue = null;

            try
            {
                primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }


            var pe = Expression.Parameter(typeof(TEntity), "entity");
            var expressionTree = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(Expression.Property(pe, primaryKeyName),
                    Expression.Constant(primayKeyValue, primaryKeyType)), new[] { pe });

            return query.Condition(expressionTree);

        }


        //public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, string propertyName, string sortDirection) where TEntity : IBaseEntity
        //{
        //    var methodName = sortDirection == "Asc" ? "OrderBy" : "OrderByDescending";
        //    var type = typeof(TEntity);
        //    var property = type.GetProperty(propertyName);
        //    var parameter = Expression.Parameter(type, "p");
        //    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        //    var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        //    var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType },
        //        query.Expression, Expression.Quote(orderByExpression));
        //    return query.Provider.CreateQuery<TEntity>(resultExpression);
        //}
        public static IOrderedQueryable<TEntity> OrderByMultipleColumns<TEntity>(this IQueryable<TEntity> query, string propertyNames = "Id desc")
        {
            var orderdQuery = query.OrderBy(propertyNames);
            return orderdQuery;
        }

    }
}