using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.Question.Command.Create;
using Application.CommandQueries.Question.Command.Delete;
using Application.CommandQueries.Question.Command.Update;
using Application.CommandQueries.Question.Model;
using Application.CommandQueries.Question.Query.Get;
using Application.CommandQueries.Question.Query.GetAll;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class QuestionController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetQuestionQuery model) => Ok(ServiceResult<QuestionModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllQuestionQuery model) => Ok(ServiceResult<PagedList<List<QuestionModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateQuestionCommand model) => Ok(ServiceResult<QuestionModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateQuestionCommand model) => Ok(ServiceResult<QuestionModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<QuestionModel>.Set(await Mediator.Send(new DeleteQuestionCommand() { Id = id })));

    }
}
