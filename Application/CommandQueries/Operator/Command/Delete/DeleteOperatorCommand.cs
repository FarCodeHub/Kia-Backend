using Application.CommandQueries.Operator.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Operator.Command.Delete
{
    public class DeleteOperatorCommand : CommandBase, IRequest<OperatorModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteOperatorCommandHandler : IRequestHandler<DeleteOperatorCommand, OperatorModel>
    {
        private readonly IMapper _mapper;
        private readonly IOperatorService _operatorService;
        private readonly IRepository _repository;

        public DeleteOperatorCommandHandler(IMapper mapper, IOperatorService operatorService, IRepository repository)
        {
            _mapper = mapper;
            _operatorService = operatorService;
            _repository = repository;
        }

        public async Task<OperatorModel> Handle(DeleteOperatorCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operatorService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<OperatorModel>(entity.Entity);
            }

            return null;
        }
    }
}
