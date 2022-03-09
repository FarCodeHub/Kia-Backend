namespace Infrastructure.ConfigurationAccessor.Models
{
    public class JwtConfigurationModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int ExpirySecondTime { get; set; }
        public int RefreshTokenExpirySecondTime { get; set; }

    }
}