using Application.CommandQueries.UnitPosition.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.UnitPosition.Command.Delete
{
    public class DeleteUnitPositionCommand : CommandBase, IRequest<UnitPositionModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteUnitPositionCommandHandler : IRequestHandler<DeleteUnitPositionCommand, UnitPositionModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitPositionService _unitPositionService;
        private readonly IRepository _repository;

        public DeleteUnitPositionCommandHandler(IMapper mapper, IUnitPositionService unitPositionService, IRepository repository)
        {
            _mapper = mapper;
            _unitPositionService = unitPositionService;
            _repository = repository;
        }

        public async Task<UnitPositionModel> Handle(DeleteUnitPositionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitPositionService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UnitPositionModel>(entity.Entity);
            }
            return null;
        }
    }
}
