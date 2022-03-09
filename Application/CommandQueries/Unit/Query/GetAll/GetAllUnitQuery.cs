using System.Collections.Generic;
 
using Application.CommandQueries.Unit.Model;
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

namespace Application.CommandQueries.Unit.Query.GetAll
{
    public class GetAllUnitQuery : Pagination, IRequest<PagedList<List<UnitModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }


    }

    public class GetAllUnitQueryHandler : IRequestHandler<GetAllUnitQuery, PagedList<List<UnitModel>>>
    {
        private readonly IUnitService _unitService;
        private readonly IMapper _mapper;

        public GetAllUnitQueryHandler(IMapper mapper, IUnitService unitService)
        {
            _mapper = mapper;
            _unitService = unitService;
        }

        public async Task<PagedList<List<UnitModel>>> Handle(GetAllUnitQuery request, CancellationToken cancellationToken)
        {
            var entities = _unitService
                    .GetAll()
                    .ProjectTo<UnitModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
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
