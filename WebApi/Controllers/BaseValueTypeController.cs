using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.BaseValueType.Command.Create;
using Application.CommandQueries.BaseValueType.Command.Delete;
using Application.CommandQueries.BaseValueType.Command.Update;
using Application.CommandQueries.BaseValueType.Model;
using Application.CommandQueries.BaseValueType.Query.Get;
using Application.CommandQueries.BaseValueType.Query.GetAll;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class BaseValueTypeController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetBaseValueTypeQuery model) => Ok(ServiceResult<BaseValueTypeModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllBaseValueTypeQuery model) => Ok(ServiceResult<PagedList<List<BaseValueTypeModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBaseValueTypeCommand model) => Ok(ServiceResult<BaseValueTypeModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBaseValueTypeCommand model) => Ok(ServiceResult<BaseValueTypeModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<BaseValueTypeModel>.Set(await Mediator.Send(new DeleteBaseValueTypeCommand() { Id = id })));

    }
}