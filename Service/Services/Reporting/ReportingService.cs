using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.reverse;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Services.Reporting
{
    public class ReportingService : IReportingService
    {
        private readonly IRepository _repository;
        private readonly IApplicationUnitOfWorkProcedures _applicationUnitOfWorkProcedures;
        public ReportingService(IRepository repository, IApplicationUnitOfWorkProcedures applicationUnitOfWorkProcedures)
        {
            _repository = repository;
            _applicationUnitOfWorkProcedures = applicationUnitOfWorkProcedures;
        }



        public async Task<ICollection<StatisticModel>> CommissionStatistics(int? branchId, int? unitId, int[] employeeIds, DateTime? from, DateTime? to, GroupBy groupBy)
        {
            var persianCalendar = new PersianCalendar();
            var query = _repository.GetQuery<Domain.Entities.Commission>();

            if (employeeIds != null)
            {
                query = query
                    .Where(x => employeeIds.Contains(x.EmployeeId));
            }

            if (unitId != null)
            {
                query = query
                    .Where(x => x.Employee.UnitPosition.UnitId == unitId);
            }

            if (branchId != null)
            {
                query = query
                    .Where(x => x.Employee.UnitPosition.Unit.BranchId == branchId);
            }

            if (from != null)
            {
                query = query
                    .Where(x => x.CreatedAt >= from);
            }

            if (to != null)
            {
                query = query
                  .Where(x => x.CreatedAt >= from);
            }

            var temp = (await query
                    .Where(x =>
                                x.CreatedAt >= from &&
                                x.CreatedAt <= to)
                    .ToListAsync())
                .Select(x => new
                {
                    data = x.Amount,
                    month = persianCalendar.GetMonth(x.CreatedAt),
                    hour = persianCalendar.GetHour(x.CreatedAt),
                    dayOfMonth = persianCalendar.GetDayOfMonth(x.CreatedAt),
                })
                .GroupBy(x => groupBy == GroupBy.DayOfMonth ? x.dayOfMonth : x.month)
                .Select(g => new
                {
                    Label = g.Key,
                    Value = g.Sum(y => y.data)
                });


            return temp
                .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                .ToList();
        }

        public async Task<ICollection<StatisticModel>> ContractsStatistics(int? branchId, int? unitId,
            int[] employeeIds, DateTime from, DateTime to, GroupBy groupBy)
        {
            var query = _repository.GetQuery<Domain.Entities.Contract>();
            var persianCalendar = new PersianCalendar();

            if (employeeIds != null)
            {
                query = query
                    .Where(x => employeeIds.Contains(x.Task.EmployeeId));
            }

            if (unitId != null)
            {
                query = query
                    .Where(x => x.Task.Employee.UnitPosition.UnitId == unitId);
            }

            if (branchId != null)
            {
                query = query
                    .Where(x => x.Task.Employee.UnitPosition.Unit.BranchId == branchId);
            }

            var temp = (await query
                    .ToListAsync())
                .Select(x => new
                {
                    month = persianCalendar.GetMonth(x.CreatedAt),
                    dayOfMonth = persianCalendar.GetDayOfMonth(x.CreatedAt),
                });

            var res = temp.GroupBy(x => groupBy == GroupBy.DayOfMonth ? x.dayOfMonth : x.month)
                .Select(g => new
                {
                    Label = g.Key,
                    Value = (double)g.Count()
                });

            return res
                .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                .ToList();

        }


        public async Task<ICollection<StatisticModel>> TasksStatistics(int? branchId, int? unitId, int[] employeeIds, DateTime from, DateTime to, GroupBy groupBy)
        {
            var query = _repository.GetQuery<Domain.Entities.Task>();
            var persianCalendar = new PersianCalendar();

            if (employeeIds != null)
            {
                query = query
                    .Where(x => employeeIds.Contains(x.EmployeeId));
            }

            if (unitId != null)
            {
                query = query
                    .Where(x => x.Employee.UnitPosition.UnitId == unitId);
            }

            if (branchId != null)
            {
                query = query
                    .Where(x => x.Employee.UnitPosition.Unit.BranchId == branchId);
            }

            var timeLength = (to - from).Days;


            var temp = (await query.Include(x => x.ResultBase)
                    .Where(x =>
                                x.CreatedAt >= from &&
                                x.CreatedAt <= to && x.ResultBaseId != null)
                    .ToListAsync())
                .Select(x => new
                {
                    data = x?.ResultBase?.Title,
                    month = persianCalendar.GetMonth(x.CreatedAt),
                    hour = persianCalendar.GetHour(x.CreatedAt),
                    dayOfMonth = persianCalendar.GetDayOfMonth(x.CreatedAt),
                });

            if (groupBy == GroupBy.Type)
            {
                var res = temp.GroupBy(x => x.data)
                    .Select(g => new
                    {
                        Label = g.Key,
                        Value = g.Count()
                    });
                return res
                    .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                    .ToList();
            }
            else
            {
                var res2 = temp.GroupBy(x => groupBy == GroupBy.Hour ? x.hour : groupBy == GroupBy.DayOfMonth ? x.dayOfMonth : x.month)
                    .Select(g => new
                    {
                        Label = g.Key,
                        Value = (double)g.Count()
                    });

                return res2
                    .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                    .ToList();

            }
        }


        public async Task<IDictionary<string, IDictionary<string, double>>> SeparatedAnswersStatistics(DateTime from, DateTime to, int questionId)
        {
            IDictionary<string, IDictionary<string, double>> staticsDictionary =
                new Dictionary<string, IDictionary<string, double>>();

            ICollection<KeyValuePair<string, ICollection<StatisticModel>>> staticsCollection = new List<KeyValuePair<string, ICollection<StatisticModel>>>();

            var query = _repository.GetQuery<Domain.Entities.SessionSurvey>().Where(x => x.QuestionId == questionId);
            var persianCalendar = new PersianCalendar();


            var temp = (await query.Include(x => x.AnswerNavigation)
                    .Where(x =>
                                x.CreatedAt >= from &&
                                x.CreatedAt <= to && x.AnswerId != null)
                    .ToListAsync())
                .Select(x => new
                {
                    data = x.AnswerNavigation.Title,
                    month = persianCalendar.GetMonth(x.CreatedAt),
                });

            var answersTypes = temp.Select(x => x.data).Distinct();

            foreach (var answerNavigationTitle in answersTypes)
            {
                var res2 = temp.Where(x => x.data == answerNavigationTitle)
                        .GroupBy(x => x.month)
                        .Select(g => new
                        {
                            Label = g.Key,
                            Value = (double)g.Count()
                        });

                staticsDictionary.Add(answerNavigationTitle, res2.ToDictionary(x => x.Label.ToString(), y => y.Value));
                staticsCollection.Add(new KeyValuePair<string, ICollection<StatisticModel>>(answerNavigationTitle, res2.ToCollectionStatisticModel(x => x.Label.ToString(), arg => arg.Value)));
            }


            return staticsDictionary;
        }

        public async Task<ICollection<StatisticModel>> AnswersStatistics(DateTime @from, DateTime to, int questionId)
        {
            var query = _repository.GetQuery<Domain.Entities.SessionSurvey>().Where(x => x.QuestionId == questionId);
            var persianCalendar = new PersianCalendar();


            var temp = (await query.Include(x => x.AnswerNavigation)
                    .Where(x =>
                        x.CreatedAt >= from &&
                        x.CreatedAt <= to && x.AnswerId != null)
                    .ToListAsync())
                .Select(x => new
                {
                    data = x.AnswerNavigation.Title,
                    month = persianCalendar.GetMonth(x.CreatedAt),
                });

            var res = temp.GroupBy(x => x.data)
                .Select(g => new
                {
                    Label = g.Key,
                    Value = g.Count()
                });
            return res
                .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                .ToList();
        }


        public async Task<ICollection<StatisticModel>> QuestionStatistics(DateTime from, DateTime to)
        {
            var persianCalendar = new PersianCalendar();
            var query = _repository.GetQuery<Domain.Entities.Question>();

            query = query
                .Where(x => x.CreatedAt.Date >= from.Date);

            query = query
                .Where(x => x.CreatedAt.Date <= to.Date);

            var temp = (await query
                        .Where(x =>
                            x.CreatedAt >= from &&
                            x.CreatedAt <= to)
                        .ToListAsync())
                    .Select(x => new
                    {
                        data = x.Title,
                        month = persianCalendar.GetMonth(x.CreatedAt),
                        hour = persianCalendar.GetHour(x.CreatedAt),
                        dayOfMonth = persianCalendar.GetDayOfMonth(x.CreatedAt),
                    })
                ;

            var res = temp.GroupBy(x => x.data)
                .Select(g => new
                {
                    Label = g.Key,
                    Value = g.Count()
                });
            return res
                .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                .ToList();
        }


        public async Task<ICollection<StatisticModel>> CommunicationReletiveStatistics(int? branchId, int? unitId, int[] employeeIds, DateTime from, DateTime to, GroupBy groupBy)
        {
            var persianCalendar = new PersianCalendar();
            var query = _repository.GetQuery<Domain.Entities.Communication>();

            if (employeeIds != null)
            {
                query = query
                    .Where(x => employeeIds.Contains(x.EmployeeId));
            }

            if (unitId != null)
            {
                query = query
                    .Where(x => x.Employee.UnitPosition.UnitId == unitId);
            }

            if (branchId != null)
            {
                query = query
                    .Where(x => x.Employee.UnitPosition.Unit.BranchId == branchId);
            }


            query = query
                .Where(x => x.CreatedAt.Date >= from.Date);

            query = query
                .Where(x => x.CreatedAt.Date <= to.Date);

            var timeLength = (to - from).Days;

            var temp = (await query
                        .Where(x =>
                            x.CreatedAt >= from &&
                            x.CreatedAt <= to)
                        .ToListAsync())
                    .Select(x => new
                    {
                        data = x.TypeBaseId,
                        month = persianCalendar.GetMonth(x.CreatedAt),
                        hour = persianCalendar.GetHour(x.CreatedAt),
                        dayOfMonth = persianCalendar.GetDayOfMonth(x.CreatedAt),
                    })
                ;


            if (groupBy == GroupBy.Type)
            {
                var res = temp.GroupBy(x => x.data)
                    .Select(g => new
                    {
                        Label = g.Key,
                        Value = g.Count()
                    });
                return res
                    .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                    .ToList();
            }
            else
            {
                var res2 = temp.GroupBy(x => groupBy == GroupBy.Hour ? x.hour : groupBy == GroupBy.DayOfMonth ? x.dayOfMonth : x.month)
                    .Select(g => new
                    {
                        Label = g.Key,
                        Value = (double)((double)g.Count() / (double)timeLength)
                    });
                return res2
                    .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                    .ToList();
            }



        }

        public async Task<ICollection<StatisticModel>> CommunicationStatistics(int? branchId, int? unitId, int[] employeeIds, DateTime from, DateTime to,
            GroupBy groupBy)
        {
            var persianCalendar = new PersianCalendar();
            var query = _repository.GetQuery<Domain.Entities.Communication>();

            if (employeeIds != null)
            {
                query = query
                    .Where(x => employeeIds.Contains(x.EmployeeId));
            }

            if (unitId != null)
            {
                query = query
                    .Where(x => x.Employee.UnitPosition.UnitId == unitId);
            }

            if (branchId != null)
            {
                query = query
                    .Where(x => x.Employee.UnitPosition.Unit.BranchId == branchId);
            }


            query = query
                .Where(x => x.CreatedAt.Date >= from.Date);

            query = query
                .Where(x => x.CreatedAt.Date <= to.Date);

            var temp = (await query
                        .Where(x =>
                            x.CreatedAt >= from &&
                            x.CreatedAt <= to)
                        .ToListAsync())
                    .Select(x => new
                    {
                        data = x.TypeBaseId,
                        month = persianCalendar.GetMonth(x.CreatedAt),
                        hour = persianCalendar.GetHour(x.CreatedAt),
                        dayOfMonth = persianCalendar.GetDayOfMonth(x.CreatedAt),
                    })
                ;


            if (groupBy == GroupBy.Type)
            {
                var res = temp.GroupBy(x => x.data)
                    .Select(g => new
                    {
                        Label = g.Key,
                        Value = g.Count()
                    });
                return res
                    .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                    .ToList();
            }
            else
            {
                var res2 = temp.GroupBy(x =>
                        groupBy == GroupBy.Hour ? x.hour : groupBy == GroupBy.DayOfMonth ? x.dayOfMonth : x.month)
                    .Select(g => new
                    {
                        Label = g.Key,
                        Value = g.Count()
                    });
                return res2
                    .Select(item => StatisticModel.ToStatisticModel(item.Label.ToString(), item.Value))
                    .ToList();
            }
        }


        public async Task<CensusesSpResult> Census(int? branchId, int? unitId,int? userId, DateTime from, DateTime to)
        {
            var census = await _applicationUnitOfWorkProcedures
                .CensusesSpAsync(from, to, userId, unitId, branchId);
            return census;
        }


        public enum GroupBy
        {
            Type,
            Hour,
            DayOfMonth,
            Month
        }

        public enum Type
        {
            Total,
            Hour,
            DayOfMonth,
            Month
        }
    }
}