using System.Collections.Generic;
using Application.CommandQueries.SessionSurvey.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using Persistence.SqlServer.QueryProvider;

namespace Application.CommandQueries.SessionSurvey.Query.GetAll
{
    public class GetAllSessionSurveyQuery : Pagination, IRequest<PagedList<List<SessionSurveyModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }
    }

    public class GetAllSessionSurveyQueryHandler : IRequestHandler<GetAllSessionSurveyQuery, PagedList<List<SessionSurveyModel>>>
    {
        private readonly ISessionSurveyService _sessionSurveyService;
        private readonly IMapper _mapper;

        public GetAllSessionSurveyQueryHandler(IMapper mapper, ISessionSurveyService sessionSurveyService)
        {
            _mapper = mapper;
            _sessionSurveyService = sessionSurveyService;
        }

        public async Task<PagedList<List<SessionSurveyModel>>> Handle(GetAllSessionSurveyQuery request, CancellationToken cancellationToken)
        {
            var entities = _sessionSurveyService
                    .GetAll()
                    .ProjectTo<SessionSurveyModel>(_mapper.ConfigurationProvider)
                    .MakeStringSearchQuery(request.Conditions)
                    .OrderByMultipleColumns(request.OrderByProperty)
                ;

            return new PagedList<List<SessionSurveyModel>>()
            {
                Data = await entities
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entities
                        .CountAsync(cancellationToken)
                    : 0
            };


        }
    }
}
