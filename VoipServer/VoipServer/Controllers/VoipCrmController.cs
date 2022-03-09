using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AsterNET.Manager.Action;
using AsterNET.Manager.Response;
using Domain.Entities;
using Infrastructure.Configurations.RedisConfigurations;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VoipServer.Data.Context.MySql;
using VoipServer.Data.Entities;
using VoipServer.Services;

namespace VoipServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VoipCrmController : Controller
    {
        private readonly IVoipMySqlUnitOfWork _voipMySqlUnitOfWork;
        private readonly IRepository _repository;
        private readonly IRedisDataProvider _redisDataProvider;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public VoipCrmController(IVoipMySqlUnitOfWork voipMySqlUnitOfWork, IRepository repository, IRedisDataProvider redisDataProvider, ICurrentUserAccessor currentUserAccessor)
        {
            _voipMySqlUnitOfWork = voipMySqlUnitOfWork;
            _repository = repository;
            _redisDataProvider = redisDataProvider;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecordedSound([FromQuery] string uniqueid)
        {
            var call = await _voipMySqlUnitOfWork.Cdr.OrderBy(x => x.id)
                .FirstOrDefaultAsync(x => x.uniqueid.Equals(uniqueid));
            return Ok(ServiceResult<string>.Set($"http://192.168.3.233{call.record}"));
        }


        [HttpGet]
        public async Task<IActionResult> GetStatus([FromQuery] string extention)
        {
            var unit = await (from up in _repository.GetQuery<UnitPosition>()
                              join u in _repository.GetQuery<Unit>()
                                  on up.UnitId equals u.Id
                              where up.Id == _currentUserAccessor.GetUnitPositionId()
                              select u).FirstOrDefaultAsync();

          
            var operators = _repository.GetQuery<Operator>()
                            .Where(x => x.Employee.UnitPosition.Unit.LevelCode.StartsWith(unit.LevelCode));

            if (!string.IsNullOrEmpty(extention))
            {
                operators = operators.Where(x => x.ExtentionNumber == extention);
            }


            var statuses = new Dictionary<string, string>();

            foreach (var @operator in await operators.ToListAsync())
            {
                var actionRes = VoipCoreManager.Instance.Manager().SendAction(new ExtensionStateAction()
                {
                    Exten = @operator.ExtentionNumber,
                    Context = "internal1",
                    ActionId = "1"
                });

                if (actionRes is ExtensionStateResponse extensionStateResponse)
                {
                    if (extensionStateResponse.Status is 1 or 2 or 8 or 16)
                    {
                        statuses.Add(@operator.ExtentionNumber, "busy");
                    }
                    else if (extensionStateResponse.Status is -1 or 4)
                    {
                        statuses.Add(@operator.ExtentionNumber, "unregistered");
                    }
                    else if (extensionStateResponse.Status is 0)
                    {
                        statuses.Add(@operator.ExtentionNumber, "ready");
                    }
                }
            }

            return Ok(ServiceResult<Dictionary<string, string>>.Set(statuses));
        }

        [HttpGet]
        public async Task<IActionResult> Call([FromQuery] string from, [FromQuery] string to)
        {
            VoipCoreManager.Instance.MakeCall(from, to, true);
            return Ok(ServiceResult<bool>.Set(true));
        }

        [HttpGet]
        public async Task<IActionResult> TurnOnDnd([FromQuery] string extention)
        {
            if (string.IsNullOrEmpty(extention))
            {
                extention = _currentUserAccessor.GetExtentionNumber();
            }
            if (VoipCoreManager.Instance.TurnOnDnd(extention))
            {
                return Ok(ServiceResult<bool>.Set(true));
            }
            return Ok(ServiceResult<bool>.Set(false));
        }

        [HttpGet]
        public async Task<IActionResult> TurnOffDnd([FromQuery] string extention)
        {
            if (string.IsNullOrEmpty(extention))
            {
                extention = _currentUserAccessor.GetExtentionNumber();
            }
            if (VoipCoreManager.Instance.TurnOffDnd(extention))
            {
                return Ok(ServiceResult<bool>.Set(true));
            }
            return Ok(ServiceResult<bool>.Set(false));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCallsHistory([FromQuery] string number)
        {
            var calls = await _voipMySqlUnitOfWork.Cdr.OrderBy(x => x.id).Where(x => x.src.Contains(number) || x.dst.Contains(number))
                .ToListAsync();
            return Ok(ServiceResult<List<cdr>>.Set(calls));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> DeliverySignal([FromBody] string signalId)
        {
            var res = _redisDataProvider.Remove(signalId);
            return Ok(ServiceResult<bool>.Set(res));
        }
    }
}