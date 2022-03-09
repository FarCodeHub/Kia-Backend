using Application.CommandQueries.SessionSurvey.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.SessionSurvey.Query.Get
{
    public class GetSessionSurveyQuery : IRequest<SessionSurveyModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetSessionSurveyQueryHandler : IRequestHandler<GetSessionSurveyQuery, SessionSurveyModel>
    {
        private readonly ISessionSurveyService _sessionSurveyService;
        private readonly IMapper _mapper;

        public GetSessionSurveyQueryHandler(IMapper mapper, ISessionSurveyService sessionSurveyService)
        {
            _mapper = mapper;
            _sessionSurveyService = sessionSurveyService;
        }

        public async Task<SessionSurveyModel> Handle(GetSessionSurveyQuery request, CancellationToken cancellationToken)
        {
            var entity = _sessionSurveyService.FindById(request.Id);

            return await entity
                .ProjectTo<SessionSurveyModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
