using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.Customer.Command.Create;
using Application.CommandQueries.Customer.Command.Delete;
using Application.CommandQueries.Customer.Command.Merge;
using Application.CommandQueries.Customer.Command.Update;
using Application.CommandQueries.Customer.Model;
using Application.CommandQueries.Customer.Query.Get;
using Application.CommandQueries.Customer.Query.GetAll;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CustomerController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCustomerQuery model) => Ok(ServiceResult<CustomerModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllCustomerQuery model) => Ok(ServiceResult<PagedList<List<CustomerModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCustomerCommand model) => Ok(ServiceResult<CustomerModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand model) => Ok(ServiceResult<CustomerModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> FullUpdate([FromBody] UpdateCustomerCommand model)
        {
            model.FullUpdate = true;
            return Ok(ServiceResult<CustomerModel>.Set(await Mediator.Send(model)));
        }

        [HttpPut]
        public async Task<IActionResult> Merge([FromBody] CreateMergeCommand model) => Ok(ServiceResult<CustomerModel>.Set(await Mediator.Send(model)));


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<CustomerModel>.Set(await Mediator.Send(new DeleteCustomerCommand() { Id = id })));


        //[HttpPost]
        //public async Task<IActionResult> Search([FromBody] SearchCustomerQuery model) => Ok(ServiceResult<List<CustomerModel>>.Set(await Mediator.Send(model)));

    }
}
