using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Branch.Command.Create;
using Application.CommandQueries.Branch.Command.Delete;
using Application.CommandQueries.Branch.Command.Update;
using Application.CommandQueries.Branch.Model;
using Application.CommandQueries.Branch.Query.Get;
using Application.CommandQueries.Branch.Query.GetAll;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class BranchController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetBranchQuery model) => Ok(ServiceResult<BranchModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllBranchQuery model) => Ok(ServiceResult<PagedList<List<BranchModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBranchCommand model) => Ok(ServiceResult<BranchModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBranchCommand model) => Ok(ServiceResult<BranchModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<BranchModel>.Set(await Mediator.Send(new DeleteBranchCommand() { Id = id })));


    }
}
