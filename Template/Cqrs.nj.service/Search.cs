using Application.$fileinputname$.Model;
using Application.Common.Common.Interfaces;
using Application.Common.Common.Model;
using Infrastructure.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Models;
using MediatR;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Service.Interfaces;

namespace $rootnamespace$.$fileinputname$.Query.Search
{
    public class Search$fileinputname$Query : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<SearchQueryItem> Queries { get; set; }
    }

    public class Search$fileinputname$QueryHandler : IRequestHandler<Search$fileinputname$Query, ServiceResult>
    {
        private readonly I$fileinputname$Service _$fileinputname$Service;
        private readonly IMapper _mapper;

        public Search$fileinputname$QueryHandler(IMapper mapper, I$fileinputname$Service $fileinputname$Service)
        {
            _mapper = mapper;
            _$fileinputname$Service = $fileinputname$Service;
        }

        public async Task<ServiceResult> Handle(Search$fileinputname$Query request, CancellationToken cancellationToken)
        {
            return ServiceResult.Set(
                await _$fileinputname$Service.Search(new Pagination()
                {
                    Skip = request.Skip,
                    Take = request.Take,
                    OrderByProperty = request.OrderByProperty,
                    SortDirection = request.SortDirection
                }, request.Queries)
                    .ProjectTo<$fileinputname$Model>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken));
        }
    }
}
