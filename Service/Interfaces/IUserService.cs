using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Service.Interfaces
{
    public interface IUserService : ICrudService<User>
    {
        System.Threading.Tasks.Task<User> GetUserByUsername(string Username);
        System.Threading.Tasks.Task<User> GetUserById(int id);

        System.Threading.Tasks.Task<User> GetUserByUserPass(string Username, string password);

        System.Threading.Tasks.Task<List<Claim>> GetClaims(IdentityModel identityModel, User user,
            CancellationToken cancellationToken = new CancellationToken());


        System.Threading.Tasks.Task<IQueryable<MenuItem>> GetUserMenues(int userId);


    }
}