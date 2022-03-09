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
    public class GetQuestionStatisticsQuery : IRequest<ICollection<StatisticModel>>, IQuery
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class GetQuestionStatisticsQueryHandler : IRequestHandler<GetQuestionStatisticsQuery, ICollection<StatisticModel>>
    {
        private readonly IMapper _mapper;
        private readonly IReportingService _reportingService;

        public GetQuestionStatisticsQueryHandler(IMapper mapper, IReportingService reportingService)
        {
            _mapper = mapper;
            _reportingService = reportingService;
        }

        public async System.Threading.Tasks.Task<ICollection<StatisticModel>> Handle(GetQuestionStatisticsQuery request, CancellationToken cancellationToken)
        {
            return await _reportingService.QuestionStatistics(request.From, request.To);

        }
    }
}