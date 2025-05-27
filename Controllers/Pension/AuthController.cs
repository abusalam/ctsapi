using System.Diagnostics;
using System.Reflection;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class AuthController(IClaimService claimService, IConfiguration configuration)
        : ApiBaseController(claimService)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IClaimService _claimService = claimService;

        [HttpGet("auth/get-version")]
        [Tags("Pension: Auth")]
        [OpenApi]
        public JsonAPIResponse<string> GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Version: v" + fileVersionInfo.ProductVersion,
                Result = fileVersionInfo.ProductVersion,
            };
        }

        [Authorize("RequiredRoleOrPermission")]
        [HttpGet("auth/login")]
        [Tags("Pension: Auth")]
        [OpenApi]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Login()
        {
            try
            {
                return Ok(new { status = "ValidToken" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, msg = ex.Message });
            }
        }

        [Authorize("RequiredRoleOrPermission")]
        [HttpGet("auth/logout")]
        [Tags("Pension: Auth")]
        [OpenApi]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Logout()
        {
            try
            {
                return Ok(new { status = "ValidToken" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, msg = ex.Message });
            }
        }

        [Authorize("RequiredRoleOrPermission")]
        [HttpGet("auth/roles")]
        [Tags("Pension: Auth")]
        [OpenApi]
        public JsonAPIResponse<RoleDTO> GetRoles()
        {
            JsonAPIResponse<RoleDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "User roles fetched successfully!",
            };

            try
            {
                var roles = _claimService.GetRoles();

                response.Result = new RoleDTO
                {
                    RoleName = roles?.FirstOrDefault() ?? string.Empty,
                };
            }
            catch (Exception ex)
            {
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [Authorize("RequiredRoleOrPermission")]
        [HttpGet("auth/permissions")]
        [Tags("Pension: Auth")]
        [OpenApi]
        public JsonAPIResponse<PermissionDTO> GetPermissions()
        {
            JsonAPIResponse<PermissionDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "User permissions fetched successfully!",
            };

            try
            {
                var permissions = _claimService.GetPermissions();
                response.Result = new PermissionDTO
                {
                    PermissionNames = permissions?.ToList() ?? new List<string>(),
                };
            }
            catch (Exception ex)
            {
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }
    }
}
