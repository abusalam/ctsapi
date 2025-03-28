using System.Security;
using CTS_BE.Common;
using CTS_BE.Enum;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.Model.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static CTS_BE.Model.Claims.ClaimModel;

namespace CTS_BE.Filters
{
    public class AppAuthFilterAttribute : IAuthorizationFilter
    {
        private readonly string[] _rolesAllowed;
        private readonly string[] _permissionsAllowed;
        private readonly IClaimService _claimService;

        public AppAuthFilterAttribute(string rolesPermissions, IClaimService claimService)
        {
            _claimService = claimService;

            var parts = rolesPermissions.Split(';', StringSplitOptions.RemoveEmptyEntries);

            var rolesPart = parts.FirstOrDefault(p =>
                p.StartsWith("roles:", StringComparison.OrdinalIgnoreCase)
            );
            var permsPart = parts.FirstOrDefault(p =>
                p.StartsWith("permissions:", StringComparison.OrdinalIgnoreCase)
            );

            _rolesAllowed =
                rolesPart
                    ?.Substring("roles:".Length)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim())
                    .ToArray() ?? Array.Empty<string>();

            _permissionsAllowed =
                permsPart
                    ?.Substring("permissions:".Length)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToArray() ?? Array.Empty<string>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            JsonAPIResponse<bool> response = new();

            // If user claims are present, check their roles & permissions
            if (context.HttpContext.Items["userclaimmodel"] != null)
            {
                var userRoles = _claimService.GetRoles();
                var userPermissions = _claimService.GetPermissions();

                // Case-insensitive intersection
                var rolesMatch =
                    !_rolesAllowed.Any()
                    || userRoles.Intersect(_rolesAllowed, StringComparer.OrdinalIgnoreCase).Any();

                var permsMatch =
                    !_permissionsAllowed.Any()
                    || userPermissions
                        .Intersect(_permissionsAllowed, StringComparer.OrdinalIgnoreCase)
                        .Any();

                if (rolesMatch && permsMatch)
                {
                    return;
                }

                // Forbidden if either check fails
                response.Message = ErrorMessages.Unauthorized_Acess;
                response.ApiResponseStatus = APIResponseStatus.Error;
                response.Result = false;

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                };
                return;
            }

            // Unauthenticated if no claim model at all
            response.Message = ErrorMessages.UnAuthenticated;
            response.ApiResponseStatus = APIResponseStatus.Error;
            response.Result = false;

            context.Result = new JsonResult(response)
            {
                StatusCode = StatusCodes.Status401Unauthorized,
            };
        }
    }

    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(string rolesPermissions)
            : base(typeof(AppAuthFilterAttribute))
        {
            Arguments = new object[] { rolesPermissions };
        }
    }
}
