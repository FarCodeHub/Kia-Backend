using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.UserRole.Command.Create;
using Application.CommandQueries.UserRole.Command.Delete;
using Application.CommandQueries.UserRole.Command.Update;
using Application.CommandQueries.UserRole.Model;
using Application.CommandQueries.UserRole.Query.Get;
using Application.CommandQueries.UserRole.Query.GetAll;
using Application.CommandQueries.UserRole.Query.GetAllowedUserRoles;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class UserRoleController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetUserRoleQuery model) => Ok(ServiceResult<UserRoleModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllUserRoleQuery model) => Ok(ServiceResult<PagedList<List<UserRoleModel>>>.Set(await Mediator.Send(model)));


        [HttpGet]
        public async Task<IActionResult> GetAllowedUserRole() => Ok(ServiceResult< PagedList<List<UserRoleModel>>>.Set(await Mediator.Send(new GetAllowedUserRolesQuery())));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserRoleCommand model) => Ok(ServiceResult<UserRoleModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRoleCommand model) => Ok(ServiceResult<UserRoleModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<UserRoleModel>.Set(await Mediator.Send(new DeleteUserRoleCommand(){Id = id})));
    }
}
