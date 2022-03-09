using System;
using System.Collections.Generic;
using Application.CommandQueries.Task.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Task.Command.Create
{
    public class CreateTaskCommand : CommandBase, IRequest<TaskModel>, IMapFrom<CreateTaskCommand>, ICommand
    {
        public int? ParentId { get; set; }
        public int? CommunicationId { get; set; }
        public int TypeBaseId { get; set; } = default!;
        public int EmployeeId { get; set; } = default!;
        public int CustomerId { get; set; } = default!;
        public int? ProjectId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime DuoAt { get; set; } = default!;
        public DateTime? EndAt { get; set; }
        public int Status { get; set; } = default!;
        public string? StatusTitle { get; set; }
        public int? ResultBaseId { get; set; }
        public string? Descriptions { get; set; }

        public IList<SessionSurveyModel> TaskSurveysModel { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateTaskCommand, Domain.Entities.Task>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskModel>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _sessionService;
        private readonly IRepository _repository;

        public CreateTaskCommandHandler(IMapper mapper, ITaskService sessionService, IRepository repository)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _repository = repository;
        }


        public async Task<TaskModel> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = await _sessionService.Add(_mapper.Map<Domain.Entities.Task>(request),
                _mapper.Map<IList<Domain.Entities.SessionSurvey>>(request.TaskSurveysModel));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<TaskModel>(entity.Entity);
            }
            return null;
        }
    }
}
