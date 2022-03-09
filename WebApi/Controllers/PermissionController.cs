using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Permission.Command.Create;
using Application.CommandQueries.Permission.Command.Delete;
using Application.CommandQueries.Permission.Command.Update;
using Application.CommandQueries.Permission.Model;
using Application.CommandQueries.Permission.Query.Get;
using Application.CommandQueries.Permission.Query.GetAll;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class PermissionController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPermissionQuery model) => Ok(ServiceResult<PermissionModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllPermissionQuery model) => Ok(ServiceResult<PagedList<List<PermissionModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePermissionCommand model) => Ok(ServiceResult<PermissionModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePermissionCommand model) => Ok(ServiceResult<PermissionModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<PermissionModel>.Set(await Mediator.Send(new DeletePermissionCommand() { Id = id })));

    }
}
