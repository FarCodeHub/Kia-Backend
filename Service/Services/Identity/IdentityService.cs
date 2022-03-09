using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Configurations.RedisConfigurations;
using Infrastructure.Exceptions;
using Infrastructure.Identity;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;

namespace Service.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IJwtService _jwtService;
        private readonly IRedisDataProvider _redisDataProvider;
        private readonly IRepository _repository;
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly IUserService _userService;
        private readonly IHandledErrorManager _handledErrorManager;
        public IdentityService(IJwtService jwtService, 
            IRedisDataProvider redisDataProvider,
            IRepository repository, IConfigurationAccessor configurationAccessor, IUserService userService, IHandledErrorManager handledErrorManager)
        {
            _jwtService = jwtService;
            _redisDataProvider = redisDataProvider;
            _repository = repository;
            _configurationAccessor = configurationAccessor;
            _userService = userService;
            _handledErrorManager = handledErrorManager;
        }

        public string Login(IdentityModel identityModel,List<Claim> claims)
        {
            var login = _redisDataProvider.Get<IdentityModel>(identityModel.Username);
            if (login != null && string.IsNullOrEmpty(identityModel.AccessToken))
                // if user is already logged in: 
                // check if RefreshToken is valid or not : 
                // if yes => user can not generate another access token, so it has to REGENERATE the access token
            {
                if (login.Username == identityModel.Username && login.RefreshTokenExpiryTime > DateTime.Now)
                {
                    if (DateTimeOffset
                        .FromUnixTimeSeconds(long.Parse(GetPrincipalFromExpiredToken(login.AccessToken).Claims
                            .FirstOrDefault(x => x.Type == "exp")?.Value ?? "1")).LocalDateTime > DateTime.Now)
                    {
                        // if current ip address same as the lates logged-in ip address
                        // if not, user must know that someone might have taken advantage of its password
                        //if (identityModel.Ip.Equals(login.Ip))
                        //{
                        //    throw new Exception(
                        //        $"{identityModel.Username} is already logged-in");
                        //}

                        //if (!identityModel.Ip.Equals(login.Ip))
                        //{
                        //    throw new Exception(
                        //        $"{identityModel.Username} are already logged-in from another IpAddress:{login.Ip}");
                        //}
                    }
                }
            }
            var jwt = _jwtService.GenerateToken(identityModel, claims);
            Save(jwt);
            return jwt.AccessToken;
        }



        public void Logout(string currenUsername)
        {
            var loggedin = _redisDataProvider.Get<IdentityModel>(currenUsername);
            if (loggedin is null)
            {
                return;
            }
            _redisDataProvider.Remove(loggedin.Username);
        }

        public string GetRefreshTokenByUsername(string currenUsername)
        {
            var refreshToken = _redisDataProvider.Get<IdentityModel>(currenUsername)?.RefreshToken;
            if (refreshToken is null)
            {
                _handledErrorManager.Throw<Authorization>();
            }
            return refreshToken;
        }


        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            if (token is null) throw new ArgumentNullException(nameof(token));
            if (token.Trim() is "") throw new ArgumentNullException(nameof(token));

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
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                _handledErrorManager.Throw<InvalidToken>();
            return principal;
        }


        public void Save(IdentityModel identityModel)
        {
            if (identityModel is null) throw new ArgumentNullException(nameof(identityModel));

            var loggedIn = _redisDataProvider.Get<IdentityModel>(identityModel.Username);
            if (loggedIn != null)
            {
                loggedIn.RefreshToken = identityModel.RefreshToken;
                loggedIn.AccessToken = identityModel.AccessToken;
                loggedIn.RefreshTokenExpiryTime = identityModel.RefreshTokenExpiryTime;
                _redisDataProvider.Update(loggedIn.Username, loggedIn, TimeSpan.FromSeconds(_configurationAccessor.GetRedisConfiguration().ExpirySecondtime));
            }
            else
            {
                _redisDataProvider.Set(identityModel.Username, identityModel, TimeSpan.FromSeconds(_configurationAccessor.GetRedisConfiguration().ExpirySecondtime));
            }
        }
    }
}