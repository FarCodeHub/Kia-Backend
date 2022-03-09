using System.Collections.Generic;
 
using Application.CommandQueries.Branch.Model;
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

namespace Application.CommandQueries.Branch.Query.GetAll
{
    public class GetAllBranchQuery : Pagination, IRequest<PagedList<List<BranchModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }

    }

    public class GetAllBranchQueryHandler : IRequestHandler<GetAllBranchQuery, PagedList<List<BranchModel>>>
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;

        public GetAllBranchQueryHandler(IMapper mapper, IBranchService branchService)
        {
            _mapper = mapper;
            _branchService = branchService;
        }

        public async Task<PagedList<List<BranchModel>>> Handle(GetAllBranchQuery request, CancellationToken cancellationToken)
        {
            var entities = _branchService
                    .GetAll()
                    .ProjectTo<BranchModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<BranchModel>>()
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
