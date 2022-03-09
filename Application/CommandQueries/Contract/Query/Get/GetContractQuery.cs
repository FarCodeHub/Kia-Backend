using Application.CommandQueries.Contract.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Contract.Query.Get
{
    public class GetContractQuery : IRequest<ContractModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetContractQueryHandler : IRequestHandler<GetContractQuery, ContractModel>
    {
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;

        public GetContractQueryHandler(IMapper mapper, IContractService contractService)
        {
            _mapper = mapper;
            _contractService = contractService;
        }

        public async Task<ContractModel> Handle(GetContractQuery request, CancellationToken cancellationToken)
        {
            var entity = _contractService.FindById(request.Id);

            return await entity
                .ProjectTo<ContractModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
