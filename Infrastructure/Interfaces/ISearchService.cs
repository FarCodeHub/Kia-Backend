using System.Collections.Generic;
using System.Linq;
using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface ISearchService<T> : IService where T : class, IBaseEntity
    {
        IQueryable<T> Search(Pagination pagination, List<SearchQueryItem> queries);
    }
}