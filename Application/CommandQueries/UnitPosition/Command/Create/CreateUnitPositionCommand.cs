using Application.CommandQueries.UnitPosition.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.UnitPosition.Command.Create
{
    public class CreateUnitPositionCommand : CommandBase, IRequest<UnitPositionModel>, IMapFrom<CreateUnitPositionCommand>, ICommand
    {
        public int UnitId { get; set; } = default!;
        public int PositionId { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUnitPositionCommand, Domain.Entities.UnitPosition>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateUnitPositionCommandHandler : IRequestHandler<CreateUnitPositionCommand, UnitPositionModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitPositionService _unitPositionService;
        private readonly IRepository _repository;

        public CreateUnitPositionCommandHandler(IMapper mapper, IUnitPositionService unitPositionService, IRepository repository)
        {
            _mapper = mapper;
            _unitPositionService = unitPositionService;
            _repository = repository;
        }


        public async Task<UnitPositionModel> Handle(CreateUnitPositionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitPositionService.Add(_mapper.Map<Domain.Entities.UnitPosition>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UnitPositionModel>(entity.Entity);
            }
            return null;
        }
    }
}
