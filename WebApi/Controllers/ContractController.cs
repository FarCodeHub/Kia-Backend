using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.Contract.Command.Cancle;
using Application.CommandQueries.Contract.Command.Create;
using Application.CommandQueries.Contract.Command.Update;
using Application.CommandQueries.Contract.Model;
using Application.CommandQueries.Contract.Query.Get;
using Application.CommandQueries.Contract.Query.GetAll;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ContractController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetContractQuery model) => Ok(ServiceResult<ContractModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllContractQuery model) => Ok(ServiceResult<PagedList<List<ContractModel>>>.Set(await Mediator.Send(model)));
 
        


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateContractCommand model) => Ok(ServiceResult<ContractModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateContractCommand model) => Ok(ServiceResult<ContractModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<ContractModel>.Set(await Mediator.Send(new CancleContractCommand() { Id = id })));

    }
}
