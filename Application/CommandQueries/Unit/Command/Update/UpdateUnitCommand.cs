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


namespace Application.CommandQueries.Unit.Command.Update
{
    public class UpdateUnitCommand : CommandBase, IRequest<UnitModel>, IMapFrom<Domain.Entities.Unit>, ICommand
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public List<int> PositionList { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUnitCommand, Domain.Entities.Unit>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, UnitModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitService _unitService;
        private readonly IRepository _repository;
        private readonly IUnitPositionService _unitPositionService;


        public UpdateUnitCommandHandler(IMapper mapper, IUnitService unitService, IRepository repository, IUnitPositionService unitPositionService)
        {
            _mapper = mapper;
            _unitService = unitService;
            _repository = repository;
            _unitPositionService = unitPositionService;
        }

        public async Task<UnitModel> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {

            var updateEntity =await _unitService.Update(_mapper.Map<Domain.Entities.Unit>(request),request.PositionList,cancellationToken);


            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return  _mapper.Map<UnitModel>(updateEntity.Entity);
            }
            return null;
        }
    }
}
