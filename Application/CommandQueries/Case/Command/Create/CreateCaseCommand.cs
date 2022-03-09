using Application.CommandQueries.Case.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Case.Command.Create
{
    public class CreateCaseCommand : CommandBase, IRequest<CaseModel>, IMapFrom<CreateCaseCommand>, ICommand
    {
        public int CustomerId { get; set; }
        public int? ConsultantId { get; set; }
        public int? PresentorId { get; set; }
        public int StatusBaseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCaseCommand, Domain.Entities.Case>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCaseCommandHandler : IRequestHandler<CreateCaseCommand, CaseModel>
    {
        private readonly IMapper _mapper;
        private readonly ICaseService _contractService;
        private readonly IRepository _repository;

        public CreateCaseCommandHandler(IMapper mapper, ICaseService contractService, IRepository repository)
        {
            _mapper = mapper;
            _contractService = contractService;
            _repository = repository;
        }


        public async Task<CaseModel> Handle(CreateCaseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _contractService.Add(_mapper.Map<Domain.Entities.Case>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CaseModel>(entity.Entity);
            }
            return null;
        }
    }
}
