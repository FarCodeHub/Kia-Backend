using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.CurrentUserAccessor
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "Id")?.Value ?? "0");
        }

        public string GetRoleLevelCode()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "LevelCode")?.Value;
        }


        public string GetRefreshToken()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "RefreshToken")?.Value;
        }

        public int GetRoleId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "OwnerRoleId")?.Value ?? throw new Exception("token exception"));
        }
         
        public string GetExtentionNumber()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "ExtentionNumber")?.Value ?? throw new Exception("token exception");
        }

        public string GetQueueNumber()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "QueueNumber")?.Value ?? throw new Exception("token exception");
        }

        public int GetUnitPositionId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "UnitPositionId")?.Value ?? throw new Exception("token exception"));
        }

        public int GetUnitId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "UnitId")?.Value ?? throw new Exception("token exception"));
        }

        public int GetPositionId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "PositionId")?.Value ?? throw new Exception("token exception"));
        }

        public int GetCompanyId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "CompanyId")?.Value ?? throw new Exception("token exception"));
        }

        public string GetCultureTwoIsoName()
        {
            if (_httpContextAccessor.HttpContext != null &&
                _httpContextAccessor.HttpContext.Request.Path.Value != null &&
                _httpContextAccessor.HttpContext.Request.Path.HasValue &&
                _httpContextAccessor.HttpContext.Request.Path.Value.Length >= 4 &&
                _httpContextAccessor.HttpContext.Request.Path.Value[0] == '/' &&
                _httpContextAccessor.HttpContext.Request.Path.Value[3] == '/')
            {
                var cultureCode = _httpContextAccessor.HttpContext.Request.Path.Value.Substring(1, 2);

                return CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .FirstOrDefault(culture => string.Equals(
                        culture.Name,
                        cultureCode,
                        StringComparison.CurrentCultureIgnoreCase))?
                    .TwoLetterISOLanguageName ?? "fa";
            }
            else
            {
                return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "cultureTwoIsoName")?.Value ?? "fa";
            }
        }


        public int GetYearId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "YearId")?.Value ?? throw new Exception("token exception"));
        }

        public int GetLanguageId()
        {
            return int.Parse(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "LanguageId")?.Value ?? throw new Exception("token exception"));
        }

        public string GetIp()
        {
            return _httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }

        public string GetUsername()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "Username")?.Value ?? throw new Exception("token exception");
        }

        public bool IsExpiredToken()
        {
            var b = (DateTimeOffset
                .FromUnixTimeSeconds(long.Parse(_httpContextAccessor?.HttpContext?.User?.Claims
                    .FirstOrDefault(x => x.Type == "exp")?.Value!)));

            var c = DateTimeOffset.Now;


            var d = b - c < TimeSpan.FromSeconds(10);


            return d;
        }
    }
}