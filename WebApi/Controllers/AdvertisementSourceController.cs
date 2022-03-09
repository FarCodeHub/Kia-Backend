using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.AdvertismentSource.Command.Create;
using Application.CommandQueries.AdvertismentSource.Command.Delete;
using Application.CommandQueries.AdvertismentSource.Command.Update;
using Application.CommandQueries.AdvertismentSource.Model;
using Application.CommandQueries.AdvertismentSource.Query.Get;
using Application.CommandQueries.AdvertismentSource.Query.GetAll;
using Application.CommandQueries.AdvertismentSource.Query.IsUsablefeedbackNumber;
using Infrastructure.Models;

namespace WebApi.Controllers
{
    public class AdvertisementSourceController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAdvertisementSourceQuery model) => Ok(ServiceResult<AdvertisementSourceModel>.Set(await Mediator.Send(model)));


        [HttpGet]
        public async Task<IActionResult> IsUsableFeedbackNumber([FromQuery] IsUsableFeedbackNumberQuery model) => Ok(ServiceResult<bool>.Set(await Mediator.Send(model)));


        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetAllAdvertisementSourceQuery model) => Ok(ServiceResult<PagedList<List<AdvertisementSourceModel>>>.Set(await Mediator.Send(model)));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateAdvertisementSourceCommand model) => Ok(ServiceResult<AdvertisementSourceModel>.Set(await Mediator.Send(model)));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAdvertisementSourceCommand model) => Ok(ServiceResult<AdvertisementSourceModel>.Set(await Mediator.Send(model)));

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id) => Ok(ServiceResult<AdvertisementSourceModel>.Set(await Mediator.Send(new DeleteAdvertisementSourceCommand() { Id = id })));

    }
}