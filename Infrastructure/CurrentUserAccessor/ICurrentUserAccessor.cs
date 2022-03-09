namespace Infrastructure.CurrentUserAccessor
{
    public interface ICurrentUserAccessor
    {
        public int GetId();
        public string GetRefreshToken();
        public int GetRoleId();
        public string GetRoleLevelCode();
        public int GetCompanyId();
        public int GetYearId();
        public int GetLanguageId();
        public string GetIp();
        public string GetCultureTwoIsoName();
        public string GetUsername();
        public bool IsExpiredToken();
        public int GetUnitPositionId();
        public int GetUnitId();
        public int GetPositionId();
        public string GetExtentionNumber();
        public string GetQueueNumber();
    }
}