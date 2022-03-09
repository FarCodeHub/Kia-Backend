using System.Collections.Generic;
 
using Application.CommandQueries.UnitPosition.Model;
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

namespace Application.CommandQueries.UnitPosition.Query.GetAll
{
    public class GetAllUnitPositionQuery : Pagination, IRequest<PagedList<List<UnitPositionModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllUnitPositionQueryHandler : IRequestHandler<GetAllUnitPositionQuery, PagedList<List<UnitPositionModel>>>
    {
        private readonly IUnitPositionService _unitPositionService;
        private readonly IMapper _mapper;

        public GetAllUnitPositionQueryHandler(IMapper mapper, IUnitPositionService unitPositionService)
        {
            _mapper = mapper;
            _unitPositionService = unitPositionService;
        }

        public async Task<PagedList<List<UnitPositionModel>>> Handle(GetAllUnitPositionQuery request, CancellationToken cancellationToken)
        {
            var entities = _unitPositionService
                    .GetAll()
                    .ProjectTo<UnitPositionModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<UnitPositionModel>>()
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
