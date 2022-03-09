using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Utilities
{
    public class Traverse : ITraverse
    {
        private readonly IRepository _repository;

        public Traverse(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<T>> FindAllParents<T>(ICollection<T> allEnities, int? id, Expression<Func<T, bool>> conditionExpression) where T : class, IHierarchical
        {
            if (id == null)
            {
                return allEnities;
            }
            var parent = await _repository.Find<T>(x => x.ObjectId(id)).FirstOrDefaultAsync(conditionExpression);

            if (allEnities == null)
            {
                allEnities = new List<T>();
                allEnities.Add(parent);
            }

            if (parent == null)
                return allEnities;

            allEnities.Add(parent);
            if (parent.ParentId == null)
            {
                return allEnities;
            }
            return await FindAllParents(allEnities, parent.ParentId, conditionExpression);
        }
    }
}