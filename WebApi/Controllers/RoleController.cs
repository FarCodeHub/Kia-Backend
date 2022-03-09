using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Role.Command.Create;
using Application.CommandQueries.Role.Command.Delete;
using Application.CommandQueries.Role.Command.Update;
using Application.CommandQueries.Role.Model;
using Application.CommandQueries.Role.Query.Get;
using Application.CommandQueries.Role.Query.GetAll;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class RoleController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetRoleQuery model) => Ok(ServiceResult<RoleModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllRoleQuery model) => Ok(ServiceResult<PagedList<List<RoleModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateRoleCommand model) => Ok(ServiceResult<RoleModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand model) => Ok(ServiceResult<RoleModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<RoleModel>.Set(await Mediator.Send(new DeleteRoleCommand() { Id = id })));

    }
}

