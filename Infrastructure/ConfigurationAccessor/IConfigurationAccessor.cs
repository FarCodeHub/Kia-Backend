using Infrastructure.ConfigurationAccessor.Models;

namespace Infrastructure.ConfigurationAccessor
{
    public interface IConfigurationAccessor
    {
        public ConnectionStringModel GetConnectionString();
        public JwtConfigurationModel GetJwtConfiguration();
        public RedisConfigurationModel GetRedisConfiguration();
        public SwaggerConfigurationModel GetSwaggerConfiguration();
        public CorsConfigurationModel GetCorsConfiguration();
        public AuditMongoDbConfigurationModel GetAuditMongoDbConfiguration();
        public IoPath GetIoPaths();
        public SmsConfigurationModel GetSmsConfiguration();

        public AssymetricKeysModel GetAssymetricKeys();
        public LandingConfigurationModel LandingConfiguration();
    }
}