using System.Collections.Generic;
using System.Security.Claims;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Service.Interfaces
{
    public interface IIdentityService : IService

    {
        string Login(IdentityModel identityModel, List<Claim> claims);
        void Logout(string currenUsername);
        string GetRefreshTokenByUsername(string currenUsername);
        void Save(IdentityModel identityModel);
    }
}