using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.Task.Command.Create;
using Application.CommandQueries.Task.Command.End;
using Application.CommandQueries.Task.Command.Start;
using Application.CommandQueries.Task.Model;
using Application.CommandQueries.Task.Query.Get;
using Application.CommandQueries.Task.Query.GetAll;
using Application.CommandQueries.Task.Query.GetAllCommunication;
using Application.CommandQueries.Task.Query.GetAllToDue;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class TaskController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetTaskQuery model) => Ok(ServiceResult<TaskModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Start([FromBody] CreateStartCommand model) => Ok(ServiceResult<TaskModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> End([FromBody] CreateEndCommand model) => Ok(ServiceResult<TaskModel>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllTaskQuery model)
        {
            return Ok(ServiceResult<PagedList<List<TaskModel>>>.Set(await Mediator.Send(model)));
        } 
        
        [HttpPost]
        public async Task<IActionResult> GetAllCommunication([FromBody] GetAllCommunicationQuery model)
        {
            return Ok(ServiceResult<PagedList<List<CommunicationModel>>>.Set(await Mediator.Send(model)));
        } 
        
        [HttpPost]
        public async Task<IActionResult> GetAllToDue([FromBody] GetAllToDueQuery model)
        {
            return Ok(ServiceResult<PagedList<List<ToDueTaskModel>>>.Set(await Mediator.Send(model)));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTaskCommand model) => Ok(ServiceResult<TaskModel>.Set(await Mediator.Send(model)));

    }
}
