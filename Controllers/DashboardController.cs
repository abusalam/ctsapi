using CTS_BE.BAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DashboardController : Controller
    {
        private readonly ITokenService _tokenService;
        protected readonly IClaimService _claimService;
        public DashboardController(ITokenService tokenService, IClaimService claimService)
        {
            _tokenService = tokenService;
            _claimService = claimService;   
        }
        [HttpGet("count")]
        public async Task<APIResponse<TokenCount>> Count()
        {
            APIResponse<TokenCount> response = new();
            try
            {
                string userRole = _claimService.GetRole();
                string userScope = _claimService.GetScope();
                TokenCount tokenCount = new TokenCount
                {
                    AllTokens = await _tokenService.AllTokensCount(),
                    BillCheckingPending = await _tokenService.TokenCountByStatus(userScope, StatusManager.GetStatus(userRole, (int)Enum.StatusType.BillChecking)),
                    ReturnMemoPending = await _tokenService.TokenCountByStatus(userScope, StatusManager.GetStatus(userRole, (int)Enum.StatusType.ReturnMemo)),
                };
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = tokenCount;
                response.Message = "Data Collect Successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
