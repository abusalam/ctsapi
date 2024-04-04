using CTS_BE.BAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IClaimService _claimService;
        public TokenController(ITokenService tokenService, IClaimService claimService)
        {
            _tokenService = tokenService;
            _claimService = claimService;
        }
        [Authorize("permissions:can-receive-bill|roles:clerk")]
        [HttpPost("GenerateToken")]
        public async Task<APIResponse<string>> GenerateToken(TokenDTO tokenDTO)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();
                int tokenNo = await _tokenService.InsterNewToken(tokenDTO,userId);
                if (tokenNo!=0)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.result = tokenNo.ToString();
                    response.Message = "Token Generated Successfully";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = "Token Generated Failed!";
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpGet("GetAllTokens")]
        public async Task<APIResponse<IEnumerable<TokenList>>> AllTokens()
        {
            APIResponse<IEnumerable<TokenList>> response = new();
            //string userScope = _claimService.GetScope();
            string userScope = "BAA";
            try
            {
                IEnumerable<TokenList> tokenLists = await _tokenService.AllTokens(userScope);
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
        //[HttpGet("GetTokens")]
        //public async Task<APIResponse<IEnumerable<TokenList>>> Tokens([FromQuery]TokenListQueryParameters tokenListQueryParameters)
        //{
        //    APIResponse<IEnumerable<TokenList>> response = new();
        //    string userScope = _claimService.GetScope();
        //    string userRole = _claimService.GetRole();
        //    string listType = tokenListQueryParameters.ListType;
        //    try
        //    {
        //        IEnumerable<TokenList> tokenLists = await _tokenService.Tokens(userScope, StatusManager.GetStatus(userRole, listType));
        //        response.apiResponseStatus = Enum.APIResponseStatus.Success;
        //        response.result = tokenLists;
        //        response.Message = "Data Collect Successfully";
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.apiResponseStatus = Enum.APIResponseStatus.Error;
        //        response.Message = ex.Message;
        //        return response;
        //    }
        //}
        [HttpPost("GetTokens")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<TokenList>>>> Tokens(string listType,DynamicListQueryParameters dynamicListQueryParameters)
        {
            APIResponse<DynamicListResult<IEnumerable<TokenList>>> response = new();
            string userScope = "BAA";
            string userRole = "dealling-assistant";
            try
            {
                List<int> statuses = StatusManager.GetStatus(userRole, listType);
                DynamicListResult<IEnumerable<TokenList>> tokenLists = await _tokenService.Tokens(userScope, statuses ,dynamicListQueryParameters.filterParameters,dynamicListQueryParameters.PageIndex,dynamicListQueryParameters.PageSize,dynamicListQueryParameters.sortParameters);
                tokenLists.DataCount = await _tokenService.TokenCountByStatus(userScope, statuses, dynamicListQueryParameters.filterParameters);
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
        //[HttpGet("GetNumberOfToken")]
        //public async Task<APIResponse<TokenCount>> NumberOfToken()
        //{
        //    APIResponse<TokenCount> response = new();
        //    try
        //    {
        //        TokenCount tokenCount = new TokenCount
        //        {
        //            AllTokens = await _tokenService.AllTokensCount(),
        //            BillCheckingPending = await _tokenService.TokenCountByStatus((int)Enum.TokenStatus.BillRreceived),
        //        };
        //        response.apiResponseStatus = Enum.APIResponseStatus.Success;
        //        response.result = tokenCount;
        //        response.Message = "Data Collect Successfully";
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.apiResponseStatus = Enum.APIResponseStatus.Error;
        //        response.Message = ex.Message;
        //        return response;
        //    }
        //}
    }
}
