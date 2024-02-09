using CTS_BE.BAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReturnMemoController : Controller
    {
        protected readonly ITokenService _tokenService;
        protected readonly IClaimService _claimService;
        public ReturnMemoController(ITokenService tokenService, IClaimService claimService)
        {
            _tokenService = tokenService;
            _claimService = claimService;   
        }
        [HttpGet("GetTokens")]
        public async Task<APIResponse<IEnumerable<TokenList>>> Tokens()
        {
            APIResponse<IEnumerable<TokenList>> response = new();
            string userScope = _claimService.GetScope();
            string userRole = _claimService.GetRole();
            try
            {
                IEnumerable<TokenList> tokenLists = await _tokenService.Tokens(userScope, StatusManager.GetStatus(userRole, (int)Enum.StatusType.ReturnMemo));
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = tokenLists;
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
        [HttpPost("Generate")]
        public async Task<APIResponse<string>> GenerateReturnMemo(ReturnMemoBillDetailsDTO returnMemoBillDetailsDTO)
        {
            APIResponse<string> response = new();
            string userScope = _claimService.GetScope();
            long userId = _claimService.GetUserId();
            string userRole = _claimService.GetRole();
            try
            {
                TokenDetailsDto tokenDetailsDto = await _tokenService.TokenDeatisById(returnMemoBillDetailsDTO.TokenId);
                if (!StatusManager.GetStatus(userRole, (int)Enum.StatusType.ReturnMemo).Contains((int)tokenDetailsDto.StatusId))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Info;
                    response.Message = "Invalid Bill!";
                    return response;
                }
                if(await _tokenService.GenerateReturnMemo(returnMemoBillDetailsDTO.TokenId, returnMemoBillDetailsDTO.ReferenceNo, userId, 0))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = "DONE";
                    response.Message = "Return Memo Generate ";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Info;
                response.Message = "Invalid Bill!";
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
