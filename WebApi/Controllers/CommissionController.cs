using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.Commission.Command.Create;
using Application.CommandQueries.Commission.Command.Delete;
using Application.CommandQueries.Commission.Command.Update;
using Application.CommandQueries.Commission.Model;
using Application.CommandQueries.Commission.Query.Get;
using Application.CommandQueries.Commission.Query.GetAll;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CommissionController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCommissionQuery model) => Ok(ServiceResult<CommissionModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllCommissionQuery model) => Ok(ServiceResult<PagedList<List<CommissionModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCommissionCommand model) => Ok(ServiceResult<CommissionModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCommissionCommand model) => Ok(ServiceResult<CommissionModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<CommissionModel>.Set(await Mediator.Send(new DeleteCommissionCommand() { Id = id })));
    }
}
