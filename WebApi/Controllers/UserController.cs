using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.User.Command.Create;
using Application.CommandQueries.User.Command.Delete;
using Application.CommandQueries.User.Command.Update;
using Application.CommandQueries.User.Model;
using Application.CommandQueries.User.Query.Get;
using Application.CommandQueries.User.Query.GetAll;
using Application.CommandQueries.User.Query.GetMenues;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetUserQuery model) => Ok(ServiceResult<UserModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllUserQuery model) => Ok(ServiceResult<PagedList<List<UserModel>>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetMenues([FromQuery] GetMenuesQuery model) => Ok(ServiceResult<List<MenueItemModel>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserCommand model) => Ok(ServiceResult<UserModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand model) => Ok(ServiceResult<bool>.Set(await Mediator.Send(model)));  
        
        [HttpPut]
        public async Task<IActionResult> UpdateUserSetting([FromBody] UpdateUserSettingCommand model) => Ok(ServiceResult<bool>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<UserModel>.Set(await Mediator.Send(new DeleteUserCommand() { Id = id })));

    }
}

