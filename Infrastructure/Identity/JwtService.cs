using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity
{
    public class JwtService : IJwtService
    {
        private readonly IConfigurationAccessor _configurationAccessor;

        public JwtService(IConfigurationAccessor configurationAccessor)
        {
            _configurationAccessor = configurationAccessor;
        }


        public IdentityModel GenerateToken(IdentityModel identityModel, IList<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationAccessor.GetJwtConfiguration().Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            identityModel.RefreshToken = GenerateRefreshToken();
            claims.Add(new Claim("RefreshToken", identityModel.RefreshToken));

            var token = new JwtSecurityToken(_configurationAccessor.GetJwtConfiguration().Issuer,
                _configurationAccessor.GetJwtConfiguration().Audience,
                claims, // roles and identity informations
                expires: DateTime.Now.AddSeconds(_configurationAccessor.GetJwtConfiguration().ExpirySecondTime), // expiry time
                signingCredentials: credentials); // sign the token by an strong key

            // set token in login model
            identityModel.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            // Specify the refresh token validity period
            identityModel.RefreshTokenExpiryTime = DateTime.Now.AddSeconds(_configurationAccessor.GetJwtConfiguration().RefreshTokenExpirySecondTime);
            return identityModel;
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration config)
        {
            if (token is null) throw new ArgumentNullException(nameof(token));
            if (token.Trim() is "") throw new ArgumentNullException(nameof(token));
            if (config is null) throw new ArgumentNullException(nameof(config));


            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configurationAccessor.GetJwtConfiguration().Issuer,
                ValidAudience = _configurationAccessor.GetJwtConfiguration().Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationAccessor.GetJwtConfiguration().Secret))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}