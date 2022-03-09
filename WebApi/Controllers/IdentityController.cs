using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Exceptions;
using Infrastructure.Identity;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;

namespace WebApi.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly IUserSettingService _userSettingService;
        private readonly IBranchService _branchService;
        private readonly IRepository _repository;
        private readonly IHandledErrorManager _handledErrorManager;
        public IdentityController(IIdentityService identityService, IConfiguration configuration, IUserService userService, IJwtService jwtService, ICurrentUserAccessor currentUserAccessor, IConfigurationAccessor configurationAccessor, IUserSettingService userSettingService, IBranchService branchService, IRepository repository, IHandledErrorManager handledErrorManager)
        {
            _identityService = identityService;
            _configuration = configuration;
            _userService = userService;
            _jwtService = jwtService;
            _currentUserAccessor = currentUserAccessor;
            _configurationAccessor = configurationAccessor;
            _userSettingService = userSettingService;
            _branchService = branchService;
            _repository = repository;
            _handledErrorManager = handledErrorManager;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] IdentityModel model)
        {
            if (model.Username is not null)
            {
                model.Username = model.Username.ToLower();
            }

            model.Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var user = await _userService.GetUserByUserPass(model.Username, model.Password);
            model.Id = user.Id;

            var claims = await _userService.GetClaims(model, user);
            var token = _identityService.Login(model, claims);

            return Ok(ServiceResult<string>.Set(token));
        }


        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] IdentityModel model)
        {
            try
            {
                if (!_currentUserAccessor.IsExpiredToken())
                {
                    if (model.RoleId == 0 && !_currentUserAccessor.IsExpiredToken())
                    {
                        _handledErrorManager.Throw<TokenNotExpiredYet>();
                    }
                }
            }
            catch (Exception e)
            {
                _handledErrorManager.Throw<RefreshTokenHasBeenExpired>();
            }


            var user = await _userService.GetUserById(_currentUserAccessor.GetId());

            if (!_identityService.GetRefreshTokenByUsername(user.Username).Equals(_currentUserAccessor.GetRefreshToken()))
            {
                _handledErrorManager.Throw<RefreshTokenHasBeenChanged>();
            }

            var identityModel = new IdentityModel()
            {
                Username = user.Username,
                RoleId = model.RoleId == 0 ? _currentUserAccessor.GetRoleId() : model.RoleId,
                Ip = _currentUserAccessor.GetIp(),
                Id = user.Id
            };

            await _userSettingService.ChangeUserSetting(identityModel);

            var claims = await _userService.GetClaims(identityModel, user);

            var newToken = _jwtService.GenerateToken(identityModel, claims);
            _identityService.Save(newToken);
            return Ok(ServiceResult<IdentityModel>.Set(newToken));
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _identityService.Logout(_currentUserAccessor.GetUsername());
            return Ok(ServiceResult<bool>.Set(true));
        }
    }
}
