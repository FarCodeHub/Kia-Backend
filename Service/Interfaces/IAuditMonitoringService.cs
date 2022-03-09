using Infrastructure.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Service.Interfaces
{
    public interface IAuditMonitoringService
    {
        public IFindFluent<BsonDocument, BsonDocument> GetAll(AuditMonitoringModel auditMonitoringModel,
            AuditMonitoringProjectionModel auditMonitoringProjection);
    }
}