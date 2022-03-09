using System.Collections.Generic;
using Application.CommandQueries.Contract.Model;
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

namespace Application.CommandQueries.Contract.Query.GetAll
{
    public class GetAllContractQuery : Pagination, IRequest<PagedList<List<ContractModel>>>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }

    }

    public class GetAllContractQueryHandler : IRequestHandler<GetAllContractQuery, PagedList<List<ContractModel>>>
    {
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;

        public GetAllContractQueryHandler(IMapper mapper, IContractService contractService)
        {
            _mapper = mapper;
            _contractService = contractService;
        }

        public async Task<PagedList<List<ContractModel>>> Handle(GetAllContractQuery request, CancellationToken cancellationToken)
        {
            var entities = _contractService
                    .GetAll()
                    .ProjectTo<ContractModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<ContractModel>>()
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
