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
    public class GetCommunicationReletiveStatisticsQuery : IRequest<ICollection<StatisticModel>>, IQuery
    {
        public int? BranchId { get; set; }
        public int? UnitId { get; set; }
        public int[] EmployeeIds { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public ReportingService.GroupBy GroupBy { get; set; }
    }

    public class GetCommunicationReletiveStatisticsQueryHandler : IRequestHandler<GetCommunicationReletiveStatisticsQuery, ICollection<StatisticModel>>
    {
        private readonly IMapper _mapper;
        private readonly IReportingService _reportingService;

        public GetCommunicationReletiveStatisticsQueryHandler(IMapper mapper, IReportingService reportingService)
        {
            _mapper = mapper;
            _reportingService = reportingService;
        }

        public async System.Threading.Tasks.Task<ICollection<StatisticModel>> Handle(GetCommunicationReletiveStatisticsQuery request, CancellationToken cancellationToken)
        {
            return await _reportingService.CommunicationReletiveStatistics(request.BranchId, request.UnitId, request.EmployeeIds, request.From, request.To, request.GroupBy);

        }
    }
}
