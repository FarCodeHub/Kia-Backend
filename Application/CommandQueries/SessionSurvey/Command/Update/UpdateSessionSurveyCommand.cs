using Application.CommandQueries.SessionSurvey.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.SessionSurvey.Command.Update
{
    public class UpdateSessionSurveyCommand : CommandBase, IRequest<SessionSurveyModel>, IMapFrom<Domain.Entities.SessionSurvey>, ICommand
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateSessionSurveyCommand, Domain.Entities.SessionSurvey>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateSessionSurveyCommandHandler : IRequestHandler<UpdateSessionSurveyCommand, SessionSurveyModel>
    {
        private readonly IMapper _mapper;
        private readonly ISessionSurveyService _sessionSurveyService;
        private readonly IRepository _repository;

        public UpdateSessionSurveyCommandHandler(IMapper mapper, ISessionSurveyService sessionSurveyService, IRepository repository)
        {
            _mapper = mapper;
            _sessionSurveyService = sessionSurveyService;
            _repository = repository;
        }

        public async Task<SessionSurveyModel> Handle(UpdateSessionSurveyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _sessionSurveyService.FindById(request.Id).FirstOrDefaultAsync(cancellationToken);

            // update properties

            var updatedEntity = await _sessionSurveyService.Update(entity, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<SessionSurveyModel>(updatedEntity.Entity);
            }
            return null;
        }
    }
}
