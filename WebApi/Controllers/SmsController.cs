using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.CommandQueries.Sms.Command.Create;
using Application.CommandQueries.Sms.Model;
using Application.CommandQueries.Sms.Query.GetAllReceived;
using Application.CommandQueries.Sms.Query.GetAllSended;
using Application.CommandQueries.Sms.Query.GetAllToSend;
using Infrastructure.Models;
using SmsIrRestful;

namespace WebApi.Controllers
{
    public class SmsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllReceived([FromQuery] GetAllReceivedQuery model) => Ok(ServiceResult<PagedList<List<ReceivedMessages>>>.Set(await Mediator.Send(model)));

        [HttpGet]
        public async Task<IActionResult> GetAllSended([FromQuery] GetAllSendedQuery model) => Ok(ServiceResult<PagedList<List<SentMessage>>>.Set(await Mediator.Send(model)));


        [HttpPost]
        public async Task<IActionResult> GetAllToSend([FromBody] GetAllToSendQuery model) => Ok(ServiceResult<PagedList<List<PersonToSendModel>>>.Set(await Mediator.Send(model)));


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSendSmsCommand model) => Ok(ServiceResult<int>.Set(await Mediator.Send(model)));
    }
}
