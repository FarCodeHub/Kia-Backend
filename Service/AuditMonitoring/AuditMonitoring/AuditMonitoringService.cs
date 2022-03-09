using System;
using System.Linq;
using Audit.Core;
using Infrastructure.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Persistence.AuditProvider;
using Service.Interfaces;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Service.AuditMonitoring.AuditMonitoring
{
    public class AuditMonitoringService : IAuditMonitoringService
    {
        private readonly IAuditMontiroringRepository _auditMontiroringRepository;


        public AuditMonitoringService(IAuditMontiroringRepository auditMontiroringRepository)
        {
            _auditMontiroringRepository = auditMontiroringRepository;
        }


        public IFindFluent<BsonDocument, BsonDocument> GetAll(AuditMonitoringModel auditMonitoringModel,
            AuditMonitoringProjectionModel auditMonitoringProjection)
        {
            auditMonitoringModel.LoggedInUserId = "2";
            auditMonitoringModel.LoggedInRoleId = "2";
            //auditMonitoringModel.StartDate = DateTime.UtcNow.AddDays(-5);
            //auditMonitoringModel.EndDate = DateTime.UtcNow.AddDays(2);
            //auditMonitoringModel.ResponseStatusCode = "401";
            auditMonitoringModel.HttpMethod = "GET";

            var filter = FilterCreator(auditMonitoringModel);
            var founded = _auditMontiroringRepository.Table().Find(filter)
                .MyProject(project => project
                    .EventTypes(auditMonitoringProjection.EventType)
                    .EndDates(auditMonitoringProjection.EndDate)
                    .LoggedInUserIds(auditMonitoringProjection.LoggedInUserId)
                    .LoggedInUserRoleIds(auditMonitoringProjection.LoggedInRoleId)
                    .LoggedInUserRoleNames(auditMonitoringProjection.LoggedInRoleName)
                    .LoggedInUsernames(auditMonitoringProjection.LoggedInUsername)
                    .StartDates(auditMonitoringProjection.StartDate)
                    .Actions(auditMonitoringProjection.Action)
                    .Environments(auditMonitoringProjection.Environment)
                    .HttpMethods(auditMonitoringProjection.HttpMethod)
                    .ResponseStatusCodes(auditMonitoringProjection.ResponseStatusCode)
                    .ResponseStatuss(auditMonitoringProjection.ResponseStatus)
                    ._ids(auditMonitoringProjection._id)
                    ._ts(auditMonitoringProjection._t)
                )
                .Skip(0).Limit(1000);


            founded // دریافت تمام رکورد ها از دیتابیس به صورت کوئری
                .ToList() //تبدیل به لیست
                .ConvertAll(BsonTypeMapper.MapToDotNetValue) // تبدیل بی سون به جیسون قابل فهم برای دات نت
                .Select(JsonConvert.SerializeObject) // تبدیل به جیسون
                .Select(AuditEvent.FromJson) // تبدیل به آئودیت ایونت از جیسون
                .ToList(); // تبدیل به لیست


            return founded;
        }

        private FilterDefinition<BsonDocument> FilterCreator(AuditMonitoringModel auditMonitoringModel)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("LoggedInUserId._v", auditMonitoringModel.LoggedInUserId);

            if (!auditMonitoringModel.StartDate.Equals(DateTime.MinValue))
                filter &= Builders<BsonDocument>.Filter.Gte("StartDate", auditMonitoringModel.StartDate);

            if (!auditMonitoringModel.EndDate.Equals(DateTime.MinValue))
                filter &= Builders<BsonDocument>.Filter.Lte("EndDate", auditMonitoringModel.EndDate);

            if (auditMonitoringModel.HttpMethod is not null)
                filter &= Builders<BsonDocument>.Filter.Eq("Action.HttpMethod", auditMonitoringModel.HttpMethod);

            if (auditMonitoringModel.ResponseStatus is not null)
                filter &= Builders<BsonDocument>.Filter.Eq("Action.ResponseStatus", auditMonitoringModel.ResponseStatus);

            if (auditMonitoringModel.ResponseStatusCode is not null)
                filter &= Builders<BsonDocument>.Filter.Eq("Action.ResponseStatusCode", auditMonitoringModel.ResponseStatusCode);


            return filter;
        }
    }
}