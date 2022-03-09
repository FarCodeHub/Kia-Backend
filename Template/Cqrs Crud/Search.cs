using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.$fileinputname$.Model;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Persistence.SqlServer;
using Application.Common.Model;

namespace $rootnamespace$.$fileinputname$.Query.Search
{
    public class $safeitemname$ : Pagination,IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<SearchQueryItem> Queries { get; set; }
    }

    public class $safeitemname$Handler : IRequestHandler<$safeitemname$, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public $safeitemname$Handler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle($safeitemname$ request, CancellationToken cancellationToken)
        {
              return ServiceResult.Set(
                (await _repository
                    .GetAll<Domain.Entities.$fileinputname$>(config => config
                        .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.$fileinputname$>(request.Queries))
                         .Paginate(new Pagination()
                        {
                             Skip = request.Skip,
                             Take = request.Take,
                             OrderByProperty = request.OrderByProperty,
                             SortDirection = request.SortDirection
                        }))
                    .ProjectTo<$fileinputname$Model>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)));
        }
    }
}
    