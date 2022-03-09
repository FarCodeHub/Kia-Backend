using System;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.Reporting.Query
{
    public class GetSeperatedAnswersStatisticsQuery : IRequest<IDictionary<string, IDictionary<string, double>>>, IQuery
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int QuestionId { get; set; }
    }

    public class GetSeperatedAnswersStatisticsQueryHandler : IRequestHandler<GetSeperatedAnswersStatisticsQuery, IDictionary<string, IDictionary<string, double>>>
    {
        private readonly IMapper _mapper;
        private readonly IReportingService _reportingService;

        public GetSeperatedAnswersStatisticsQueryHandler(IMapper mapper, IReportingService reportingService)
        {
            _mapper = mapper;
            _reportingService = reportingService;
        }

        public async System.Threading.Tasks.Task<IDictionary<string, IDictionary<string, double>>> Handle(GetSeperatedAnswersStatisticsQuery request, CancellationToken cancellationToken)
        {
            return await _reportingService.SeparatedAnswersStatistics(request.From, request.To, request.QuestionId);

        }
    }
}