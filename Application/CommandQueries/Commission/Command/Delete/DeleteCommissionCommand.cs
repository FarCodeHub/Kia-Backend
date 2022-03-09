using Application.CommandQueries.Commission.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Commission.Command.Delete
{
    public class DeleteCommissionCommand : CommandBase, IRequest<CommissionModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteCommissionCommandHandler : IRequestHandler<DeleteCommissionCommand, CommissionModel>
    {
        private readonly IMapper _mapper;
        private readonly ICommissionService _contractService;
        private readonly IRepository _repository;

        public DeleteCommissionCommandHandler(IMapper mapper, ICommissionService contractService, IRepository repository)
        {
            _mapper = mapper;
            _contractService = contractService;
            _repository = repository;
        }

        public async Task<CommissionModel> Handle(DeleteCommissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _contractService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CommissionModel>(entity.Entity);
            }
            return null;
        }
    }
}
