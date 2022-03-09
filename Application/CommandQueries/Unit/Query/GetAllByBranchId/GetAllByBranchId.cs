using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Unit.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer.QueryProvider;
using Service.Interfaces;

namespace Application.CommandQueries.Unit.Query.GetAllByBranchId
{
    public class GetAllByBranchId : Pagination, IRequest<PagedList<List<UnitModel>>>, ISearchableRequest, IQuery
    {
        public int BranchId { get; set; }
    }

    public class GetAllByBranchIdQueryHandler : IRequestHandler<GetAllByBranchId, PagedList<List<UnitModel>>>
    {
        private readonly IUnitService _unitService;
        private readonly IMapper _mapper;

        public GetAllByBranchIdQueryHandler(IMapper mapper, IUnitService unitService)
        {
            _mapper = mapper;
            _unitService = unitService;
        }

        public async Task<PagedList<List<UnitModel>>> Handle(GetAllByBranchId request, CancellationToken cancellationToken)
        {
            var entities = _unitService.GetAllByBranchId(request.BranchId)
                    .ProjectTo<UnitModel>(_mapper.ConfigurationProvider)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;           
            
            return new PagedList<List<UnitModel>>()
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
