using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Task.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.Task.Command.Start
{
    public class CreateStartCommand : CommandBase, IRequest<TaskModel>, IMapFrom<CreateStartCommand>, ICommand
    {
        public int TaskId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateStartCommand, Domain.Entities.Task>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateStartCommandHandler : IRequestHandler<CreateStartCommand, TaskModel>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _sessionService;
        private readonly IRepository _repository;

        public CreateStartCommandHandler(IMapper mapper, ITaskService sessionService, IRepository repository)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _repository = repository;
        }


        public async Task<TaskModel> Handle(CreateStartCommand request, CancellationToken cancellationToken)
        {
            var entity = await _sessionService.StartTask(request.TaskId);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<TaskModel>(entity);
            }
            return null;
        }
    }
}