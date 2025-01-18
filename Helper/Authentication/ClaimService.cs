using CTS_BE.Model.Claims;
using Newtonsoft.Json;

namespace CTS_BE.Helper.Authentication
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor? _contextAccessor;
        private readonly List<ClaimModel.Application> _applications = [];
        private readonly AuthClaimModel logedinUserClaims = new();

        public ClaimService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

            if (
                _contextAccessor?.HttpContext?.Items is not null
                && _contextAccessor.HttpContext.Items.TryGetValue(
                    "userclaimmodel",
                    out var userClaimModel
                )
                && userClaimModel is AuthClaimModel logedinUserClaims
            )
            {
                _applications =
                [
                    .. logedinUserClaims
                        .Claims.Where(claim => claim?.Type == "application")
                        .Select(claim =>
                        {
                            try
                            {
                                return JsonConvert.DeserializeObject<ClaimModel.Application?>(
                                    claim.Value
                                );
                            }
                            catch (JsonReaderException)
                            {
                                // Handle JSON deserialization errors gracefully.
                                return new ClaimModel.Application(); // or a default ClaimModel.Application object if appropriate
                            }
                        }),
                ]; // Important: Convert the IEnumerable to a List
            }
            else
            {
                _applications = [new ClaimModel.Application()];
            }
        }

        public string[] GetUserApplications()
        {
            if (_applications != null)
            {
                return [.. _applications.Select(application => application.Name)];
            }
            else
            {
                return [""];
            }
        }

        public string[] GetRoles()
        {
            if (_applications != null)
            {
                return
                [
                    .. _applications
                        .SelectMany(application => application.Roles)
                        .Select(role => role.Name),
                ];
            }
            else
            {
                return [""];
            }
        }

        public string GetRole()
        {
            string[] userRole = _applications
                .SelectMany(application => application.Roles)
                .Select(role => role.Name)
                .ToArray();
            return userRole[0];
        }

        public int GetRoleIdByApplicationId(int applicationId)
        {
            int roleId = _applications
                .Where(application => application.Id == applicationId)
                .SelectMany(application => application.Roles)
                .Select(role => role.Id)
                .FirstOrDefault();
            return roleId;
        }

        public List<int> GetRoleIdsByApplicationIds(List<int> applicationIds)
        {
            return
            [
                .. _applications
                    .Where(application => applicationIds.Contains(application.Id))
                    .SelectMany(application => application.Roles)
                    .Select(role => role.Id),
            ];
        }

        public List<int> GetLevelIdsByApplicationIds(List<int> applicationIds)
        {
            return _applications
                .Where(application => applicationIds.Contains(application.Id))
                .SelectMany(application => application.Levels)
                .Select(level => level.Id)
                .ToList();
        }

        public List<string> GetScopesByApplicationName(string applicationName)
        {
            return
            [
                .. _applications
                    .Where(application => application.Name == applicationName)
                    .SelectMany(application => application.Levels)
                    .SelectMany(level => level.Scope)
                    .Select(scope => scope) // Extract only letters
                    .Distinct(),
            ];
        }

        public string GetScopeByApplicationName(string applicationName)
        {
            //string  userScope = _applications.Where(application => application.Name == applicationName).SelectMany(appliaction => appliaction.Roles).SelectMany(role => role.Scope).FirstOrDefault();
            return "";
        }

        public string GetScope()
        {
            List<string> scopeValues =
            [
                .. _applications
                    .SelectMany(application => application.Levels)
                    .Where(level => level != null && level.Scope != null)
                    .SelectMany(level => level.Scope),
            ];
            return scopeValues[0];
        }

        public string GetRoleByApplicationName(string applicationName)
        {
            return _applications
                    .Where(application => application.Name == applicationName)
                    .Select(role => role.Name)
                    .FirstOrDefault() ?? "";
        }

        public int GetUserId()
        {
            return logedinUserClaims
                .Claims.Where(claims => claims.Type == "nameid")
                .Select(claim => int.Parse(claim.Value))
                .FirstOrDefault();
        }

        public string GetUserName()
        {
            return logedinUserClaims
                    .Claims.Where(claims => claims.Type == "name")
                    .Select(claim => claim.Value)
                    .FirstOrDefault() ?? "";
        }

        public List<string> GetPermissions()
        {
            return
            [
                .. _applications
                    .SelectMany(application => application.Roles)
                    .SelectMany(role => role.Permissions),
            ];
        }

        public int GetApplicationIdByApplicationName(string applicationName)
        {
            int id = _applications
                .Where(applications => applications.Name == applicationName)
                .Select(application => application.Id)
                .FirstOrDefault();
            return id;
        }

        public short GetFinancialYear()
        {
            if (_contextAccessor?.HttpContext?.Items["FinancialYear"] is short fy)
            {
                return fy;
            }
            else
            {
                return 0;
            }
        }
    }
}
