using System.Collections.Generic;

using Application.CommandQueries.BaseValueType.Model;
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

namespace Application.CommandQueries.BaseValueType.Query.GetAll
{
    public class GetAllBaseValueTypeQuery : Pagination, IRequest<PagedList<List<BaseValueTypeModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }
    }

    public class GetAllBaseValueTypeQueryHandler : IRequestHandler<GetAllBaseValueTypeQuery, PagedList<List<BaseValueTypeModel>>>
    {
        private readonly IBaseValueTypeService _baseValueTypeService;
        private readonly IMapper _mapper;

        public GetAllBaseValueTypeQueryHandler(IMapper mapper, IBaseValueTypeService baseValueTypeService)
        {
            _mapper = mapper;
            _baseValueTypeService = baseValueTypeService;
        }

        public async Task<PagedList<List<BaseValueTypeModel>>> Handle(GetAllBaseValueTypeQuery request, CancellationToken cancellationToken)
        {
            var entities =  _baseValueTypeService.GetAll()
                .ProjectTo<BaseValueTypeModel>(_mapper.ConfigurationProvider)
                .MakeStringSearchQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return new PagedList<List<BaseValueTypeModel>>()
            {
                Data =  await entities
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
