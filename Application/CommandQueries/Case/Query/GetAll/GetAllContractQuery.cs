using System.Collections.Generic;
using Application.CommandQueries.Case.Model;
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

namespace Application.CommandQueries.Case.Query.GetAll
{
    public class GetAllCaseQuery : Pagination, IRequest<PagedList<List<CaseModel>>>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }

    }

    public class GetAllCaseQueryHandler : IRequestHandler<GetAllCaseQuery, PagedList<List<CaseModel>>>
    {
        private readonly ICaseService _contractService;
        private readonly IMapper _mapper;

        public GetAllCaseQueryHandler(IMapper mapper, ICaseService contractService)
        {
            _mapper = mapper;
            _contractService = contractService;
        }

        public async Task<PagedList<List<CaseModel>>> Handle(GetAllCaseQuery request, CancellationToken cancellationToken)
        {
            var entities = _contractService
                    .GetAll()
                   
                    .ProjectTo<CaseModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<CaseModel>>()
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
