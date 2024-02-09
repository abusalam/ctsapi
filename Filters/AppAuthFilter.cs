using CTS_BE.Common;
using CTS_BE.Helper.Authentication;
using CTS_BE.Model.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace CTS_BE.Filters
{
    //public class DBTDashboardAuthFilter

    public class AppAuthFilterAttribute : IAuthorizationFilter
    {
        private readonly string[] _roles;
        private readonly string[] _permissions;
        private readonly IClaimService _claimService;
        public AppAuthFilterAttribute(string rolesPermissions, IClaimService claimService)
        {
            string[] parts = rolesPermissions.Split('|');

            // Extract permissions
            string permissionsPart = parts.FirstOrDefault(p => p.StartsWith("permissions:"));
            _permissions = permissionsPart?.Substring("permissions:".Length).Split(',');

            // Extract roles
            string rolesPart = parts.FirstOrDefault(p => p.StartsWith("roles:"));
            _roles = rolesPart?.Substring("roles:".Length).Split(',');

            _claimService = claimService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (context.HttpContext.Items["userclaimmodel"] != null)
            {
                String[] roles = _claimService.GetRoles();

                if (_roles.Length == 1 && string.IsNullOrEmpty(_roles[0]))
                {
                    /*its all role*/
                }
                else if (!roles.All(element => _roles.Contains(element)))
                {
                    context.Result = new JsonResult(new { message = ErrorMessages.Unauthorized_Acess }) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }
            }
            else
            {
                context.Result = new JsonResult(new { message = ErrorMessages.UnAuthenticated }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

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
