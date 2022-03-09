using Application.CommandQueries.Position.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Position.Command.Create
{
    public class CreatePositionCommand : CommandBase, IRequest<PositionModel>, IMapFrom<CreatePositionCommand>, ICommand
    {
        public string Title { get; set; }
        public string UniqueName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePositionCommand, Domain.Entities.Position>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, PositionModel>
    {
        private readonly IMapper _mapper;
        private readonly IPositionService _positionService;
        private readonly IRepository _repository;

        public CreatePositionCommandHandler(IMapper mapper, IPositionService positionService, IRepository repository)
        {
            _mapper = mapper;
            _positionService = positionService;
            _repository = repository;
        }


        public async Task<PositionModel> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _positionService.Add(_mapper.Map<Domain.Entities.Position>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<PositionModel>(entity.Entity);
            }

            return null;
        }
    }
}
