using CTS_BE.Model.Claims;
using Newtonsoft.Json;

namespace CTS_BE.Helper.Authentication
{
    public class ClaimService: IClaimService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private List<ClaimModel.Application> _applications = new List<ClaimModel.Application>();
        private AuthClaimModel logedinUserClaims = new AuthClaimModel();
        public ClaimService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            logedinUserClaims = (AuthClaimModel)_contextAccessor.HttpContext.Items["userclaimmodel"];
            if (logedinUserClaims != null)
            {
                _applications = logedinUserClaims.claims.Where(claims => claims.Type == "application").Select(claim => JsonConvert.DeserializeObject<ClaimModel.Application>(claim.Value))
                    .ToList();
            }

        }
            public string[] GetUserApplications()
        {
            string[] usersApplication = _applications.Select(application => application.Name).ToArray();
            return usersApplication;
        }
        public string[] GetRoles()
        {
            string[] userRole = _applications.SelectMany(application => application.Roles).Select(role => role.Name).ToArray();
            return userRole;
        }
        public string GetRole()
        {
            string[] userRole = _applications.SelectMany(application => application.Roles).Select(role => role.Name).ToArray();
            return userRole[0];
        }
        public int GetRoleIdByApplicationId(int applicationId)
        {
            int roleId = _applications.Where(application => application.Id == applicationId).SelectMany(application => application.Roles).Select(role => role.Id).FirstOrDefault();
            return roleId;
        }
        public List<int> GetRoleIdsByApplicationIds(List<int> applicationIds)
        {
            List<int> roleIds = _applications
                .Where(application => applicationIds.Contains(application.Id))
                .SelectMany(application => application.Roles)
                .Select(role => role.Id)
                .ToList();

            return roleIds;
        }
        public List<int?> GetLevelIdsByApplicationIds(List<int> applicationIds)
        {
            List<int?> levelIds = _applications
                .Where(application => applicationIds.Contains(application.Id))
                .SelectMany(application => application.Levels)
                .Select(level => level.Id)
                .ToList();

            return levelIds;
        }
        public List<string> GetScopesByApplicationName(string applicationName)
        {
            List<string> userScopes = _applications
            .Where(application => application.Name == applicationName)
            .SelectMany(application => application.Levels)
            .SelectMany(level => level.Scope)
            .Select(scope=>scope) // Extract only letters
            .Distinct()  // Add this line to get unique letter combinations
            .ToList();
            return userScopes;
        }
        public string GetScopeByApplicationName(string applicationName)
        {
            //string  userScope = _applications.Where(application => application.Name == applicationName).SelectMany(appliaction => appliaction.Roles).SelectMany(role => role.Scope).FirstOrDefault();
            return "";
        }
        public string GetScope()
        {
            List<string> scopeValues = _applications
            .SelectMany(application => application.Levels)
            .Where(level => level != null && level.Scope != null)
            .SelectMany(level => level.Scope)
            .ToList();
            return scopeValues[0];
        }
        public string GetRoleByApplicationName(string applicationName)
        {
            string roleName = _applications.Where(application => application.Name == applicationName).Select(role=>role.Name).FirstOrDefault();
            return roleName;
        }
        public int GetUserId()
        {
            return logedinUserClaims.claims.Where(claims => claims.Type == "nameid").Select(claim => int.Parse(claim.Value)).FirstOrDefault();
        }
        public string GetUserName()
        {
            return logedinUserClaims.claims.Where(claims => claims.Type == "name").Select(claim => claim.Value).FirstOrDefault();
        }
        public List<string> GetPermissions()
        {
            var a = logedinUserClaims;
            string[] userPermission = _applications.SelectMany(application => application.Roles).SelectMany(role => role.Permissions).ToArray();
            return userPermission.ToList();
            // return _applications.FirstOrDefault().Roles.SelectMany(role => role.Permissions).ToList();
        }
        public int GetApplicationIdByApplicationName(string applicationName)
        {
            int id = _applications.Where(applications=>applications.Name == applicationName).Select(application=> application.Id).FirstOrDefault();
            return id;
        }
    }
}
