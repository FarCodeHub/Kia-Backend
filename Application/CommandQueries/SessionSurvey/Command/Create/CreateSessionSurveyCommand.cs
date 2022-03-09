using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Task.Model;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.SessionSurvey.Command.Create
{
    public class CreateSessionSurveyCommand : CommandBase, IRequest<bool>, IMapFrom<CreateSessionSurveyCommand>, ICommand
    {
        public ICollection<SessionSurveyModel> SessionSurveyCreateModels { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateSessionSurveyCommand, Domain.Entities.SessionSurvey>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateSessionSurveyCommandHandler : IRequestHandler<CreateSessionSurveyCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly ISessionSurveyService _sessionSurveyService;
        private readonly IRepository _repository;

        public CreateSessionSurveyCommandHandler(IMapper mapper, ISessionSurveyService sessionSurveyService, IRepository repository)
        {
            _mapper = mapper;
            _sessionSurveyService = sessionSurveyService;
            _repository = repository;
        }


        public async Task<bool> Handle(CreateSessionSurveyCommand request, CancellationToken cancellationToken)
        {

            foreach (var requestSessionSurveyCreateModel in request.SessionSurveyCreateModels)
            {
                await _sessionSurveyService.Add(_mapper.Map<Domain.Entities.SessionSurvey>(requestSessionSurveyCreateModel));
            }
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
