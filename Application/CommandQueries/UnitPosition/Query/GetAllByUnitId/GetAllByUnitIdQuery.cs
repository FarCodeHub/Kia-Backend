using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.UnitPosition.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.UnitPosition.Query.GetAllByUnitId
{
    public class GetAllByUnitIdQuery : Pagination, IRequest<PagedList<List<UnitPositionModel>>>, ISearchableRequest, IQuery
    {
        public int UnitId { get; set; }
    }

    public class GetAllByUnitIdHandler : IRequestHandler<GetAllByUnitIdQuery, PagedList<List<UnitPositionModel>>>
    {
        private readonly IUnitPositionService _unitPositionService;
        private readonly IMapper _mapper;

        public GetAllByUnitIdHandler(IMapper mapper, IUnitPositionService unitPositionService)
        {
            _mapper = mapper;
            _unitPositionService = unitPositionService;
        }

        public async Task<PagedList<List<UnitPositionModel>>> Handle(GetAllByUnitIdQuery request, CancellationToken cancellationToken)
        {
            var entities = _unitPositionService.GetAllByUnitId(request.UnitId)
                    .ProjectTo<UnitPositionModel>(_mapper.ConfigurationProvider)
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
