using System.Collections.Generic;
using Application.CommandQueries.Commission.Model;
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

namespace Application.CommandQueries.Commission.Query.GetAll
{
    public class GetAllCommissionQuery : Pagination, IRequest<PagedList<List<CommissionModel>>>, ISearchableRequest, IQuery
    {
        public List<Condition> Queries { get; set; }

    }

    public class GetAllCommissionQueryHandler : IRequestHandler<GetAllCommissionQuery, PagedList<List<CommissionModel>>>
    {
        private readonly ICommissionService _contractService;
        private readonly IMapper _mapper;

        public GetAllCommissionQueryHandler(IMapper mapper, ICommissionService contractService)
        {
            _mapper = mapper;
            _contractService = contractService;
        }

        public async Task<PagedList<List<CommissionModel>>> Handle(GetAllCommissionQuery request, CancellationToken cancellationToken)
        {
            var entities = _contractService
                    .GetAll()
                    .ProjectTo<CommissionModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Queries)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<CommissionModel>>()
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
