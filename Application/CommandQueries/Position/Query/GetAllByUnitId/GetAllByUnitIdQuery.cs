using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Position.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.Position.Query.GetAllByUnitId
{
    public class GetAllByUnitIdQuery : Pagination, IRequest<PagedList<List<PositionModel>>>, ISearchableRequest, IQuery
    {
        public int UnitId { get; set; }

    }

    public class GetAllByUnitIdQueryHandler : IRequestHandler<GetAllByUnitIdQuery, PagedList<List<PositionModel>>>
    {
        private readonly IPositionService _positionService;
        private readonly IMapper _mapper;

        public GetAllByUnitIdQueryHandler(IMapper mapper, IPositionService positionService)
        {
            _mapper = mapper;
            _positionService = positionService;
        }

        public async Task<PagedList<List<PositionModel>>> Handle(GetAllByUnitIdQuery request, CancellationToken cancellationToken)
        {
            var entities = _positionService
                    .GetAll()
                    .ProjectTo<PositionModel>(_mapper.ConfigurationProvider)
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
