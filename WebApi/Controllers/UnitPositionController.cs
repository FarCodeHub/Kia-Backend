using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.UnitPosition.Command.Create;
using Application.CommandQueries.UnitPosition.Command.Delete;
using Application.CommandQueries.UnitPosition.Command.Update;
using Application.CommandQueries.UnitPosition.Model;
using Application.CommandQueries.UnitPosition.Query.Get;
using Application.CommandQueries.UnitPosition.Query.GetAll;
using Application.CommandQueries.UnitPosition.Query.GetAllByUnitId;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class UnitPositionController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetUnitPositionQuery model) => Ok(ServiceResult<UnitPositionModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllUnitPositionQuery model) => Ok(ServiceResult< PagedList<List<UnitPositionModel>>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetAllByUnitId([FromQuery] GetAllByUnitIdQuery model) => Ok(ServiceResult< PagedList<List<UnitPositionModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUnitPositionCommand model) => Ok(ServiceResult<UnitPositionModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUnitPositionCommand model) => Ok(ServiceResult<UnitPositionModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<UnitPositionModel>.Set(await Mediator.Send(new DeleteUnitPositionCommand() { Id = id })));
    }
}
