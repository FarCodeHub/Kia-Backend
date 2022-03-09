using Application.CommandQueries.Unit.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Unit.Command.Delete
{
    public class DeleteUnitCommand : CommandBase, IRequest<UnitModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, UnitModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitService _unitService;
        private readonly IRepository _repository;

        public DeleteUnitCommandHandler(IMapper mapper, IUnitService unitService, IRepository repository)
        {
            _mapper = mapper;
            _unitService = unitService;
            _repository = repository;
        }

        public async Task<UnitModel> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UnitModel>(entity.Entity);
            }

            return null;
        }
    }
}
