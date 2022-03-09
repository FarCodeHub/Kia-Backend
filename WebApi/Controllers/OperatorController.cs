using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.Operator.Command.Create;
using Application.CommandQueries.Operator.Command.Deactive;
using Application.CommandQueries.Operator.Command.Delete;
using Application.CommandQueries.Operator.Model;
using Application.CommandQueries.Operator.Query.Get;
using Application.CommandQueries.Operator.Query.GetAll;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class OperatorController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetOperatorQuery model) => Ok(ServiceResult<OperatorModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllOperatorQuery model) => Ok(ServiceResult<PagedList<List<OperatorModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateOperatorCommand model) => Ok(ServiceResult<OperatorModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Deactive([FromBody] DeactiveOperatorCommand model) => Ok(ServiceResult<OperatorModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<OperatorModel>.Set(await Mediator.Send(new DeleteOperatorCommand() { Id = id })));

    }
}
