using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Unit.Command.Create;
using Application.CommandQueries.Unit.Command.Delete;
using Application.CommandQueries.Unit.Command.Update;
using Application.CommandQueries.Unit.Model;
using Application.CommandQueries.Unit.Query.Get;
using Application.CommandQueries.Unit.Query.GetAll;
using Application.CommandQueries.Unit.Query.GetAllByBranchId;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class UnitController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetUnitQuery model) => Ok(ServiceResult<UnitModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllUnitQuery model) => Ok(ServiceResult<PagedList<List<UnitModel>>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetAllByBranchId([FromQuery] GetAllByBranchId model) => Ok(ServiceResult<PagedList<List<UnitModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUnitCommand model) => Ok(ServiceResult<UnitModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUnitCommand model) => Ok(ServiceResult<UnitModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<UnitModel>.Set(await Mediator.Send(new DeleteUnitCommand() { Id = id })));

    }
}

