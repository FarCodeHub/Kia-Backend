using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Position.Command.Create;
using Application.CommandQueries.Position.Command.Delete;
using Application.CommandQueries.Position.Command.Update;
using Application.CommandQueries.Position.Model;
using Application.CommandQueries.Position.Query.Get;
using Application.CommandQueries.Position.Query.GetAll;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class PositionController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPositionQuery model) => Ok(ServiceResult<PositionModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllByUnitIdQuery model) => Ok(ServiceResult<PagedList<List<PositionModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePositionCommand model) => Ok(ServiceResult<PositionModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePositionCommand model) => Ok(ServiceResult<PositionModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<PositionModel>.Set(await Mediator.Send(new DeletePositionCommand() { Id = id })));


    }
}
