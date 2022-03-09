using System.Collections.Generic;
 
using Application.CommandQueries.Position.Model;
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

namespace Application.CommandQueries.Position.Query.GetAll
{
    public class GetAllByUnitIdQuery : Pagination, IRequest<PagedList<List<PositionModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllPositionQueryHandler : IRequestHandler<GetAllByUnitIdQuery, PagedList<List<PositionModel>>>
    {
        private readonly IPositionService _positionService;
        private readonly IMapper _mapper;

        public GetAllPositionQueryHandler(IMapper mapper, IPositionService positionService)
        {
            _mapper = mapper;
            _positionService = positionService;
        }

        public async Task<PagedList<List<PositionModel>>> Handle(GetAllByUnitIdQuery request, CancellationToken cancellationToken)
        {
            var entities = _positionService
                    .GetAll()
                    .ProjectTo<PositionModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;
            return new PagedList<List<PositionModel>>()
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
