namespace CTS_BE.Helper.Authentication
{
    public interface IClaimService
    {
        public string[] GetUserApplications();
        public string[] GetRoles();
        public string GetRole();
        public string GetScopeByApplicationName(string applicationName);
        public string GetRoleByApplicationName(string applicationName);
        public int GetRoleIdByApplicationId(int applicationId);
        public List<int> GetRoleIdsByApplicationIds(List<int> applicationIds);
        public List<int?> GetLevelIdsByApplicationIds(List<int> applicationIds);
        public List<string> GetScopesByApplicationName(string applicationName);
        public string GetScope();
        public int GetUserId();
        public string GetUserName();
        public int GetApplicationIdByApplicationName(string applicationName);

    }
}
