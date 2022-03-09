using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ITraverse
    {
        Task<ICollection<T>> FindAllParents<T>(ICollection<T> allEnities, int? id, Expression<Func<T, bool>> conditionExpression) where T : class, IHierarchical;

    }
}