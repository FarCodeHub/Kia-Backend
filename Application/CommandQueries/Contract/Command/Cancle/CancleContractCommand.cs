using Application.CommandQueries.Contract.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Contract.Command.Cancle
{
    public class CancleContractCommand : CommandBase, IRequest<ContractModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class CancleContractCommandHandler : IRequestHandler<CancleContractCommand, ContractModel>
    { 
        private readonly IMapper _mapper;
        private readonly IContractService _contractService;
        private readonly IRepository _repository;

        public CancleContractCommandHandler(IMapper mapper, IContractService contractService, IRepository repository)
        {
            _mapper = mapper;
            _contractService = contractService;
            _repository = repository;
        }

        public async Task<ContractModel> Handle(CancleContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _contractService.Cancle(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<ContractModel>(entity.Entity);
            }
            return null;
        }
    }
}
