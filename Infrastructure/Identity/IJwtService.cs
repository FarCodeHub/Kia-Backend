using System.Collections.Generic;
using System.Security.Claims;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Identity
{
    public interface IJwtService : IService
    {
        IdentityModel GenerateToken(IdentityModel identityModel, IList<Claim> claims);
    }
}