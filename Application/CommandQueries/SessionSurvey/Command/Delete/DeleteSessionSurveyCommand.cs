using Application.CommandQueries.SessionSurvey.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.SessionSurvey.Command.Delete
{
    public class DeleteSessionSurveyCommand : CommandBase, IRequest<SessionSurveyModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteSessionSurveyCommandHandler : IRequestHandler<DeleteSessionSurveyCommand, SessionSurveyModel>
    {
        private readonly IMapper _mapper;
        private readonly ISessionSurveyService _sessionSurveyService;
        private readonly IRepository _repository;

        public DeleteSessionSurveyCommandHandler(IMapper mapper, ISessionSurveyService sessionSurveyService, IRepository repository)
        {
            _mapper = mapper;
            _sessionSurveyService = sessionSurveyService;
            _repository = repository;
        }

        public async Task<SessionSurveyModel> Handle(DeleteSessionSurveyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _sessionSurveyService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<SessionSurveyModel>(entity.Entity);
            }
            return null;
        }
    }
}
