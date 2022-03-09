using System;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;
using Service.Services.Reporting;

namespace Application.CommandQueries.Reporting.Query
{
    public class GetAnswersStatisticsQuery : IRequest<ICollection<StatisticModel>>, IQuery
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int QuestionId { get; set; }
    }

    public class GetAnswersStatisticsQueryHandler : IRequestHandler<GetAnswersStatisticsQuery, ICollection<StatisticModel>>
    {
        private readonly IMapper _mapper;
        private readonly IReportingService _reportingService;

        public GetAnswersStatisticsQueryHandler(IMapper mapper, IReportingService reportingService)
        {
            _mapper = mapper;
            _reportingService = reportingService;
        }

        public async System.Threading.Tasks.Task<ICollection<StatisticModel>> Handle(GetAnswersStatisticsQuery request, CancellationToken cancellationToken)
        {
            return await _reportingService.AnswersStatistics(request.From, request.To,request.QuestionId);

        }
    }
}