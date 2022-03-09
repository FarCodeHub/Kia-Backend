using Application.CommandQueries.Operator.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Operator.Command.Create
{
    public class CreateOperatorCommand : CommandBase, IRequest<OperatorModel>, IMapFrom<CreateOperatorCommand>, ICommand
    {
        public int ExtentionNumber { get; set; } = default!;
        public int EmployeeId { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOperatorCommand, Domain.Entities.Operator>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateOperatorCommandHandler : IRequestHandler<CreateOperatorCommand, OperatorModel>
    {
        private readonly IMapper _mapper;
        private readonly IOperatorService _operatorService;
        private readonly IRepository _repository;

        public CreateOperatorCommandHandler(IMapper mapper, IOperatorService operatorService, IRepository repository)
        {
            _mapper = mapper;
            _operatorService = operatorService;
            _repository = repository;
        }


        public async Task<OperatorModel> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operatorService.Add(_mapper.Map<Domain.Entities.Operator>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<OperatorModel>(entity.Entity);
            }

            return null;
        }
    }
}
