using Application.CommandQueries.Commission.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Commission.Query.Get
{
    public class GetCommissionQuery : IRequest<CommissionModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetCommissionQueryHandler : IRequestHandler<GetCommissionQuery, CommissionModel>
    {
        private readonly ICommissionService _contractService;
        private readonly IMapper _mapper;

        public GetCommissionQueryHandler(IMapper mapper, ICommissionService contractService)
        {
            _mapper = mapper;
            _contractService = contractService;
        }

        public async Task<CommissionModel> Handle(GetCommissionQuery request, CancellationToken cancellationToken)
        {
            var entity = _contractService.FindById(request.Id);

            return await entity
                .ProjectTo<CommissionModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
