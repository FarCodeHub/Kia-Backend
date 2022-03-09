using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Driver;
using Service.Interfaces;

namespace WebApi.Controllers
{
    public class AuditMonitoringController : BaseController
    {
        private readonly IAuditMonitoringService _auditMonitoringService;

        public AuditMonitoringController(IAuditMonitoringService auditMonitoringService)
        {
            _auditMonitoringService = auditMonitoringService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] AuditMonitoringModel model, [FromQuery] AuditMonitoringProjectionModel projectionModel)
        {
            return Ok(ServiceResult<IFindFluent<BsonDocument, BsonDocument>>.Set(_auditMonitoringService.GetAll(model, projectionModel)));
        }
    }
}
