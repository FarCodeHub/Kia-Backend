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
    public class GetTasksStatisticsQuery : IRequest<ICollection<StatisticModel>>, IQuery
    {
        public int? BranchId { get; set; }
        public int? UnitId { get; set; }
        public int[] EmployeeIds { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public ReportingService.GroupBy GroupBy { get; set; }
    }

    public class GetTasksStatisticsQueryHandler : IRequestHandler<GetTasksStatisticsQuery, ICollection<StatisticModel>>
    {
        private readonly IMapper _mapper;
        private readonly IReportingService _reportingService;

        public GetTasksStatisticsQueryHandler(IMapper mapper,  IReportingService reportingService)
        {
            _mapper = mapper;
            _reportingService = reportingService;
        }

        public async System.Threading.Tasks.Task<ICollection<StatisticModel>> Handle(GetTasksStatisticsQuery request, CancellationToken cancellationToken)
        {
            return await _reportingService.TasksStatistics(request.BranchId,request.UnitId,request.EmployeeIds,request.From,request.To,request.GroupBy);

        }
    }
}
