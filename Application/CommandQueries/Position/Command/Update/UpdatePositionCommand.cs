using Application.CommandQueries.Position.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.Position.Command.Update
{
    public class UpdatePositionCommand : CommandBase, IRequest<PositionModel>, IMapFrom<Domain.Entities.Position>, ICommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePositionCommand, Domain.Entities.Position>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, PositionModel>
    {
        private readonly IMapper _mapper;
        private readonly IPositionService _positionService;
        private readonly IRepository _repository;

        public UpdatePositionCommandHandler(IMapper mapper, IPositionService positionService, IRepository repository)
        {
            _mapper = mapper;
            _positionService = positionService;
            _repository = repository;
        }

        public async Task<PositionModel> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {

            var updatedEntity = await _positionService.Update(_mapper.Map<Domain.Entities.Position>(request), cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<PositionModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
