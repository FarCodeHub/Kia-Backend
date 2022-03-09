using Infrastructure.ConfigurationAccessor.Models;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.ConfigurationAccessor
{
    public class ConfigurationAccessor : IConfigurationAccessor
    {
        private readonly IConfiguration _configuration;

        public ConfigurationAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtConfigurationModel GetJwtConfiguration()
        {
            return _configuration.GetSection("Jwt").Get<JwtConfigurationModel>();
        }

        public RedisConfigurationModel GetRedisConfiguration()
        {
            return _configuration.GetSection("Redis").Get<RedisConfigurationModel>();
        }

        public SwaggerConfigurationModel GetSwaggerConfiguration()
        {
            return _configuration.GetSection("Swagger").Get<SwaggerConfigurationModel>();
        }

        public CorsConfigurationModel GetCorsConfiguration()
        {
            return _configuration.GetSection("Cors").Get<CorsConfigurationModel>();
        }

        public AuditMongoDbConfigurationModel GetAuditMongoDbConfiguration()
        {
            return _configuration.GetSection("AuditMongoDb").Get<AuditMongoDbConfigurationModel>();
        }

        public IoPath GetIoPaths()
        {
            return _configuration.GetSection("IoPath").Get<IoPath>();
        }

        public SmsConfigurationModel GetSmsConfiguration()
        {
            return _configuration.GetSection("Sms").Get<SmsConfigurationModel>();
        }

        public ConnectionStringModel GetConnectionString()
        {
            return _configuration.GetSection("ConnectionStrings").Get<ConnectionStringModel>();
        }

        public AssymetricKeysModel GetAssymetricKeys()
        {
            return _configuration.GetSection("AsymetricKeyPair").Get<AssymetricKeysModel>();
        }

        public LandingConfigurationModel LandingConfiguration()
        {
            return _configuration.GetSection("Landing").Get<LandingConfigurationModel>();
        }
    }
}