using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.Case.Command.Create;
using Application.CommandQueries.Case.Command.Delete;
using Application.CommandQueries.Case.Command.Update;
using Application.CommandQueries.Case.Model;
using Application.CommandQueries.Case.Query.Get;
using Application.CommandQueries.Case.Query.GetAll;
using Application.CommandQueries.Contract.Query.GetAllInvolved;
using Application.CommandQueries.Employee.Model;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CaseController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCaseQuery model) => Ok(ServiceResult<CaseModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllCaseQuery model) => Ok(ServiceResult<PagedList<List<CaseModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCaseCommand model) => Ok(ServiceResult<CaseModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCaseCommand model) => Ok(ServiceResult<CaseModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<CaseModel>.Set(await Mediator.Send(new DeleteCaseCommand() { Id = id })));

        [HttpGet]
        public async Task<IActionResult> GetAllInvolved([FromQuery] GetAllInvolvedQuery model) => Ok(ServiceResult<PagedList<List<EmployeeModel>>>.Set(await Mediator.Send(model)));
    }
}
