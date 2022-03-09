using System.Collections.Generic;
using Application.CommandQueries.BaseValue.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using Persistence.SqlServer.QueryProvider;

namespace Application.CommandQueries.BaseValue.Query.GetAll
{
    public class GetAllQuery : Pagination, IRequest<PagedList<List<BaseValueModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }
    }

    public class GetAllBaseValueQueryHandler : IRequestHandler<GetAllQuery, PagedList<List<BaseValueModel>>>
    {
        private readonly IBaseValueService _baseValueService;
        private readonly IMapper _mapper;

        public GetAllBaseValueQueryHandler(IMapper mapper, IBaseValueService baseValueService)
        {
            _mapper = mapper;
            _baseValueService = baseValueService;
        }

        public async Task<PagedList<List<BaseValueModel>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var entities = _baseValueService
                    .GetAll()
                    .ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty);


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
