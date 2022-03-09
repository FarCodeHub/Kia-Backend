using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.BaseValue.Command.Create;
using Application.CommandQueries.BaseValue.Command.Delete;
using Application.CommandQueries.BaseValue.Command.Update;
using Application.CommandQueries.BaseValue.Model;
using Application.CommandQueries.BaseValue.Query.Get;
using Application.CommandQueries.BaseValue.Query.GetAll;
using Application.CommandQueries.BaseValue.Query.GetAllByBaseValueTypeId;
using Application.CommandQueries.BaseValue.Query.GetAllByCategoryUniqueName;
using Application.CommandQueries.BaseValue.Query.GetAllByUniqueName;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class BaseValueController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetBaseValueQuery model) => Ok(ServiceResult<BaseValueModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllQuery model) => Ok(ServiceResult<PagedList<List<BaseValueModel>>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetAllByBaseValueTypeId([FromQuery] GetAllByBaseValueTypeIdQuery model) => Ok(ServiceResult<PagedList<List<BaseValueModel>>>.Set(await Mediator.Send(model)));


        [HttpPost]
        public async Task<IActionResult> GetAllByUniqueName([FromQuery] GetAllByUniqueNameQuery model) => Ok(ServiceResult<PagedList<List<BaseValueModel>>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetAllByCategoryUniqueName([FromQuery] GetAllByCategoryUniqueNameQuery model) => Ok(await Mediator.Send(model));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBaseValueCommand model) => Ok(ServiceResult<BaseValueModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBaseValueCommand model) => Ok(ServiceResult<BaseValueModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<BaseValueModel>.Set(await Mediator.Send(new DeleteBaseValueCommand() { Id = id })));


    }
}