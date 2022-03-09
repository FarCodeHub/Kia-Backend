using Application.CommandQueries.Branch.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Branch.Query.Get
{
    public class GetBranchQuery : IRequest<BranchModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetBranchQueryHandler : IRequestHandler<GetBranchQuery, BranchModel>
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;

        public GetBranchQueryHandler(IMapper mapper, IBranchService branchService)
        {
            _mapper = mapper;
            _branchService = branchService;
        }

        public async Task<BranchModel> Handle(GetBranchQuery request, CancellationToken cancellationToken)
        {
            var entity = _branchService.FindById(request.Id);

            return await entity
                .ProjectTo<BranchModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
