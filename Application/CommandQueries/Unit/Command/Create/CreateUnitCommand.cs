using System.Collections.Generic;
using Application.CommandQueries.Unit.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Unit.Command.Create
{
    public class CreateUnitCommand : CommandBase, IRequest<UnitModel>, IMapFrom<CreateUnitCommand>, ICommand
    {
        public int BranchId { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public List<int> PositionList { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUnitCommand, Domain.Entities.Unit>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, UnitModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitService _unitService;
        private readonly IRepository _repository;
        private readonly IUnitPositionService _unitPositionService;
        private readonly IMediator _mediator;
        public CreateUnitCommandHandler(IMapper mapper, IUnitService unitService, IRepository repository, IUnitPositionService unitPositionService, IMediator mediator)
        {
            _mapper = mapper;
            _unitService = unitService;
            _repository = repository;
            _unitPositionService = unitPositionService;
            _mediator = mediator;
        }


        public async Task<UnitModel> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitService.Add(_mapper.Map<Domain.Entities.Unit>(request),request.PositionList);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UnitModel>(entity.Entity);
            }
            return null;
        }
    }
}
