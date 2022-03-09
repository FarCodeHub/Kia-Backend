using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.SessionSurvey.Command.Create;
using Application.CommandQueries.SessionSurvey.Model;
using Application.CommandQueries.SessionSurvey.Query.Get;
using Application.CommandQueries.SessionSurvey.Query.GetAll;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class SessionSurvey : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetSessionSurveyQuery model) => Ok(ServiceResult<SessionSurveyModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllSessionSurveyQuery model) => Ok(ServiceResult<PagedList<List<SessionSurveyModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSessionSurveyCommand model) => Ok(ServiceResult<bool>.Set(await Mediator.Send(model)));
    }
}