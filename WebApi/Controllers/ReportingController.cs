using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.Reporting.Query;
using Domain.Entities;
using Domain.Entities.reverse;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using ServiceStack;

namespace WebApi.Controllers
{
    public class ReportingController : BaseController
    {
        private readonly IReportingService _reportingService;

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksStatistics([FromQuery] GetTasksStatisticsQuery model) => Ok(ServiceResult<ICollection<StatisticModel>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetQuestionsStatistics([FromQuery] GetQuestionStatisticsQuery model) => Ok(ServiceResult<ICollection<StatisticModel>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetSeperatedAnswersStatistics([FromQuery] GetSeperatedAnswersStatisticsQuery model)
        {

            var a = await Mediator.Send(model);
            return Ok(a.ToArray());
        }
       // public async Task<IActionResult> GetSeperatedAnswersStatistics([FromQuery] GetSeperatedAnswersStatisticsQuery model) => Ok(ServiceResult<IDictionary<string, IDictionary<string, double>>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetAnswersStatistics([FromQuery] GetAnswersStatisticsQuery model) => Ok(ServiceResult<ICollection<StatisticModel>>.Set(await Mediator.Send(model)));


        [HttpGet]
        public async Task<IActionResult> GetCommissionsStatistics([FromQuery] GetCommissionStatisticsQuery model) => Ok(ServiceResult<ICollection<StatisticModel>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetCommunicationsReletiveStatistics([FromQuery] GetCommunicationReletiveStatisticsQuery model) => Ok(ServiceResult<ICollection<StatisticModel>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetCommunicationsStatistics([FromQuery] GetCommunicationStatisticsQuery model) => Ok(ServiceResult<ICollection<StatisticModel>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetCensuses([FromQuery] GetCensusesQuery model) => Ok(ServiceResult<CensusesSpResult>.Set(await Mediator.Send(model)));

      
    }
}
