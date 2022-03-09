using Application.CommandQueries.Operator.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.Operator.Command.Deactive
{
    public class DeactiveOperatorCommand : CommandBase, IRequest<OperatorModel>, IMapFrom<Domain.Entities.Operator>, ICommand
    {
        public int Id { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeactiveOperatorCommand, Domain.Entities.Operator>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateOperatorCommandHandler : IRequestHandler<DeactiveOperatorCommand, OperatorModel>
    {
        private readonly IMapper _mapper;
        private readonly IOperatorService _operatorService;
        private readonly IRepository _repository;

        public UpdateOperatorCommandHandler(IMapper mapper, IOperatorService operatorService, IRepository repository)
        {
            _mapper = mapper;
            _operatorService = operatorService;
            _repository = repository;
        }

        public async Task<OperatorModel> Handle(DeactiveOperatorCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Operator>(request);
            entity.IsActive = false;
            var updatedEntity = await _operatorService.Update(entity, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<OperatorModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
