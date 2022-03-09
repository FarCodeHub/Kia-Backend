namespace Infrastructure.ConfigurationAccessor.Models
{
    public class AuditMongoDbConfigurationModel
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
        public bool UseBson { get; set; }
    }
}