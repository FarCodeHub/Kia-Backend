using System;
using System.Collections.Generic;
using System.Threading;
using Application.CommandQueries.Task.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Application.CommandQueries.Task.Command.End
{


    public class CreateEndCommand : CommandBase, IRequest<TaskModel>, IMapFrom<CreateEndCommand>, ICommand
    {
        public int TaskId { get; set; }
        public int ResultBaseId { get; set; }
        public int CommunicationId { get; set; }
        public DateTime DueAt { get; set; }
        public int? AssignToEmployeeId { get; set; }
        public string? Descriptions { get; set; }

        public ICollection<SessionSurveyModel> SessionSurveyCreateModels { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateEndCommand, Domain.Entities.Task>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateEndCommandHandler : IRequestHandler<CreateEndCommand, TaskModel>
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _sessionService;
        private readonly IRepository _repository;
        private readonly ISessionSurveyService _sessionSurveyService;

        public CreateEndCommandHandler(IMapper mapper, ITaskService sessionService, IRepository repository, ISessionSurveyService sessionSurveyService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _repository = repository;
            _sessionSurveyService = sessionSurveyService;
        }


        public async System.Threading.Tasks.Task<TaskModel> Handle(CreateEndCommand request, CancellationToken cancellationToken)
        {
            var entity = await _sessionService.EndTask(request.TaskId, request.CommunicationId, request.ResultBaseId, request.AssignToEmployeeId, request.DueAt,request.Descriptions);
            if (request.SessionSurveyCreateModels is { Count: > 0 })
            {
                foreach (var requestSessionSurveyCreateModel in request.SessionSurveyCreateModels)
                {
                    var ss = _mapper.Map<Domain.Entities.SessionSurvey>(requestSessionSurveyCreateModel);
                    ss.TaskId = entity.Entity.Id;

                    await _sessionSurveyService.Add(
                        ss);
                }
            }

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<TaskModel>(await _repository.Find<Domain.Entities.Task>(x=>x.ObjectId(request.TaskId)).FirstOrDefaultAsync(cancellationToken));
            }
            return null;
        }

    }
}