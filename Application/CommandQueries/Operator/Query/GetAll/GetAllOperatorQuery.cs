using System.Collections.Generic;
 
using Application.CommandQueries.Operator.Model;
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

namespace Application.CommandQueries.Operator.Query.GetAll
{
    public class GetAllOperatorQuery : Pagination, IRequest<PagedList<List<OperatorModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllOperatorQueryHandler : IRequestHandler<GetAllOperatorQuery, PagedList<List<OperatorModel>>>
    {
        private readonly IOperatorService _operatorService;
        private readonly IMapper _mapper;

        public GetAllOperatorQueryHandler(IMapper mapper, IOperatorService operatorService)
        {
            _mapper = mapper;
            _operatorService = operatorService;
        }

        public async Task<PagedList<List<OperatorModel>>> Handle(GetAllOperatorQuery request, CancellationToken cancellationToken)
        {
            var entities = _operatorService
                    .GetAll()
                    .ProjectTo<OperatorModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<OperatorModel>>()
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
