using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Person.Command.Create;
using Application.CommandQueries.Person.Command.Delete;
using Application.CommandQueries.Person.Command.Update;
using Application.CommandQueries.Person.Model;
using Application.CommandQueries.Person.Query.Get;
using Application.CommandQueries.Person.Query.GetAll;
using Application.CommandQueries.Person.Query.NationalCodeIsExist;
using Application.CommandQueries.Person.Query.SearchPerson;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class PersonController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPersonQuery model) => Ok(ServiceResult<PersonModel>.Set(await Mediator.Send(model)));


        [HttpGet]
        public async Task<IActionResult> SearchPerson([FromQuery] SearchPersonQuery model) => Ok(ServiceResult<List<PersonModel>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> NationalCodeIdExist([FromQuery] NationalCodeIdExistQuery model) => Ok(ServiceResult<bool>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllPersonQuery model) => Ok(ServiceResult<PagedList<List<PersonModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePersonCommand model) => Ok(ServiceResult<PersonModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePersonCommand model) => Ok(ServiceResult<PersonModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> UpdateAvatar([FromBody] UpdatePersonAvatarCommand model) => Ok(ServiceResult<PersonModel>.Set(await Mediator.Send(model)));


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<PersonModel>.Set(await Mediator.Send(new DeletePersonCommand() { Id = id })));
  

    }
}

