using System.Security.Claims;
using CTS_BE.Model.Claims;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace CTS_BE.Helper.Authentication
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly AuthClaimModel logedinUserClaims = new();

        public ClaimService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor =
                contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));

            if (
                _contextAccessor.HttpContext?.Items?.TryGetValue(
                    "userclaimmodel",
                    out var userClaimObj
                ) == true
                && userClaimObj is AuthClaimModel userClaimModel
            )
            {
                logedinUserClaims = userClaimModel;
            }
        }

        public string[] GetRoles()
        {
            return logedinUserClaims
                    ?.claims?.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToArray() ?? [];
        }

        public string GetRole()
        {
            return logedinUserClaims
                    ?.claims?.Where(claim => claim.Type == "role")
                    .Select(claim => claim.Value)
                    .FirstOrDefault() ?? string.Empty;
        }

        public string GetScope()
        {
            return logedinUserClaims?.claims?.FirstOrDefault(c => c.Type == "scope")?.Value
                ?? string.Empty;
        }

        public int GetUserId()
        {
            return logedinUserClaims
                .claims.Where(claim => claim.Type == "nameid")
                .Select(claim => int.TryParse(claim.Value, out var id) ? id : 0)
                .FirstOrDefault();
        }

        public string GetUserName()
        {
            return logedinUserClaims
                    .claims.Where(claim => claim.Type == "name")
                    .Select(claim => claim.Value)
                    .FirstOrDefault() ?? string.Empty;
        }

        //public string[] GetPermission()
        //{
        //    return logedinUserClaims
        //            ?.claims?.Where(claim => claim.Type == "permissions")
        //            .Select(claim => claim.Value)
        //            .Distinct()
        //            .ToArray() ?? Array.Empty<string>();
        //}
        public string[] GetPermissions()
        {
            return logedinUserClaims
                    ?.claims?.Where(claim => claim.Type == "permissions") // singular, assuming consistent naming
                    .Select(claim => claim.Value)
                    .Distinct()
                    .ToArray() ?? Array.Empty<string>();
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
