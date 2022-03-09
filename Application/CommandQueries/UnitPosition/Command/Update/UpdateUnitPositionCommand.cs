using Application.CommandQueries.UnitPosition.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.UnitPosition.Command.Update
{
    public class UpdateUnitPositionCommand : CommandBase, IRequest<UnitPositionModel>, IMapFrom<Domain.Entities.UnitPosition>, ICommand
    {
        public int Id { get; set; }
        public int UnitId { get; set; } = default!;
        public int PositionId { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUnitPositionCommand, Domain.Entities.UnitPosition>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateUnitPositionCommandHandler : IRequestHandler<UpdateUnitPositionCommand, UnitPositionModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitPositionService _unitPositionService;
        private readonly IRepository _repository;

        public UpdateUnitPositionCommandHandler(IMapper mapper, IUnitPositionService unitPositionService, IRepository repository)
        {
            _mapper = mapper;
            _unitPositionService = unitPositionService;
            _repository = repository;
        }

        public async Task<UnitPositionModel> Handle(UpdateUnitPositionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitPositionService.FindById(request.Id).FirstOrDefaultAsync(cancellationToken);

            // update properties

            var updatedEntity = await _unitPositionService.Update(entity, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UnitPositionModel>(updatedEntity.Entity);
            }
            return null;
        }
    }
}
