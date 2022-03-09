using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.reverse;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Service.Services.Reporting;

namespace Service.Interfaces
{
    public interface IReportingService : IService
    {
        Task<CensusesSpResult> Census(int? branchId, int? unitId, int? userId, DateTime from, DateTime to);

        Task<ICollection<StatisticModel>> TasksStatistics(int? branchId, int? unitId, int[] employeeIds, DateTime from,
            DateTime to, ReportingService.GroupBy groupBy);

        Task<ICollection<StatisticModel>> CommissionStatistics(int? branchId, int? unitId, int[] employeeIds, DateTime? from,
            DateTime? to, ReportingService.GroupBy groupBy);

        Task<ICollection<StatisticModel>> CommunicationReletiveStatistics(int? branchId, int? unitId, int[] employeeIds, DateTime from, DateTime to, ReportingService.GroupBy groupBy);

        Task<ICollection<StatisticModel>> QuestionStatistics(DateTime from, DateTime to);

        Task<IDictionary<string, IDictionary<string, double>>> SeparatedAnswersStatistics(DateTime from, DateTime to,
            int questionId);

        Task<ICollection<StatisticModel>> AnswersStatistics(DateTime from, DateTime to, int questionId);

        Task<ICollection<StatisticModel>> ContractsStatistics(int? branchId, int? unitId,
            int[] employeeIds, DateTime from, DateTime to, ReportingService.GroupBy groupBy);

        Task<ICollection<StatisticModel>> CommunicationStatistics(int? branchId, int? unitId,
            int[] employeeIds,DateTime from, DateTime to,
            ReportingService.GroupBy groupBy);



    }
}