using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.BaseValue.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.BaseValue.Query.GetAllByUniqueName
{
    public class GetAllByUniqueNameQuery : Pagination, IRequest<PagedList<List<BaseValueModel>>>, ISearchableRequest, IQuery
    {
        public string UniqueName { get; set; }
        public Condition[] Conditions { get; set; }

    }

    public class GetAllByUniqueNameQueryHandler : IRequestHandler<GetAllByUniqueNameQuery, PagedList<List<BaseValueModel>>>
    {
        private readonly IBaseValueService _baseValueService;
        private readonly IMapper _mapper;

        public GetAllByUniqueNameQueryHandler(IMapper mapper, IBaseValueService baseValueService)
        {
            _mapper = mapper;
            _baseValueService = baseValueService;
        }

        public async Task<PagedList<List<BaseValueModel>>> Handle(GetAllByUniqueNameQuery request, CancellationToken cancellationToken)
        {
            var entities = _baseValueService.GetAllByUniqueName(request.UniqueName)
                    .ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<BaseValueModel>>()
            {
                Data = await entities
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entities
                        .CountAsync(cancellationToken)
                    : 0
            };
        }
    }
}
