using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models
{
    public class AuditMonitoringModel : Pagination
    {
        [BsonDateTimeOptions]
        public DateTime StartDate { get; set; }
        [BsonDateTimeOptions]
        public DateTime EndDate { get; set; }
        public string LoggedInRoleId { get; set; }
        public string LoggedInUserId { get; set; }
        public string _id { get; set; }
        public string _t { get; set; }
        public string Environment { get; set; }
        public string EventType { get; set; }
        public object Action { get; set; }
        public string HttpMethod { get; set; }
        public string ResponseStatusCode { get; set; }
        public string ResponseStatus { get; set; }
        public string UserId { get; set; }

    }

    public class AuditMonitoringProjectionModel
    {
        public bool StartDate { get; set; } = false;
        public bool EndDate { get; set; } = false;
        public bool LoggedInRoleId { get; set; } = false;
        public bool LoggedInUserId { get; set; } = false;
        public bool LoggedInRoleName { get; set; } = false;
        public bool LoggedInUsername { get; set; } = false;
        public bool _id { get; set; } = false;
        public bool _t { get; set; } = false;
        public bool Environment { get; set; } = false;
        public bool EventType { get; set; } = false;
        public bool Action { get; set; } = false;
        public bool HttpMethod { get; set; } = false;
        public bool ResponseStatusCode { get; set; } = false;
        public bool ResponseStatus { get; set; } = false;
        public bool UserId { get; set; } = false;

    }
}