using Application.CommandQueries.Position.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Position.Command.Delete
{
    public class DeletePositionCommand : CommandBase, IRequest<PositionModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, PositionModel>
    {
        private readonly IMapper _mapper;
        private readonly IPositionService _positionService;
        private readonly IRepository _repository;

        public DeletePositionCommandHandler(IMapper mapper, IPositionService positionService, IRepository repository)
        {
            _mapper = mapper;
            _positionService = positionService;
            _repository = repository;
        }

        public async Task<PositionModel> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _positionService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<PositionModel>(entity.Entity);
            }

            return null;
        }
    }
}
