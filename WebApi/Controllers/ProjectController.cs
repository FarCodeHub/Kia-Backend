using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Project.Command.Create;
using Application.CommandQueries.Project.Command.Delete;
using Application.CommandQueries.Project.Command.SendProjectInfo;
using Application.CommandQueries.Project.Command.Update;
using Application.CommandQueries.Project.Model;
using Application.CommandQueries.Project.Query.Get;
using Application.CommandQueries.Project.Query.GetAll;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IConfigurationAccessor _configurationAccessor;

        public ProjectController(IConfigurationAccessor configurationAccessor)
        {
            _configurationAccessor = configurationAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetProjectQuery model) => Ok(ServiceResult<ProjectModel>.Set(await Mediator.Send(model)));

      
        [HttpGet]
        public async Task<IActionResult> SendProjectInfo([FromQuery] CreateSendProjectInfoCommand model) => Ok(ServiceResult<bool>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllProjectQuery model) => Ok(ServiceResult<PagedList<List<ProjectModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProjectCommand model) => Ok(ServiceResult<ProjectModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProjectCommand model) => Ok(ServiceResult<ProjectModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<ProjectModel>.Set(await Mediator.Send(new DeleteProjectCommand() { Id = id })));

    }
}
