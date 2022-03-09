using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Employee.Command.Create;
using Application.CommandQueries.Employee.Command.Delete;
using Application.CommandQueries.Employee.Command.Update;
using Application.CommandQueries.Employee.Model;
using Application.CommandQueries.Employee.Query.Get;
using Application.CommandQueries.Employee.Query.GetAll;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class EmployeeController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetEmployeeQuery model) => Ok(ServiceResult<EmployeeModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllEmployeeQuery model) => Ok(ServiceResult<PagedList<List<EmployeeModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateEmployeeCommand model) => Ok(ServiceResult<object>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeCommand model) => Ok(ServiceResult<EmployeeModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<EmployeeModel>.Set(await Mediator.Send(new DeleteEmployeeCommand() { Id = id })));

        //[HttpPost]
        //public async Task<IActionResult> Search([FromBody] SearchEmployeeQuery model) => Ok(ServiceResult<List<EmployeeModel>>.Set(await Mediator.Send(model)));

    }
}
