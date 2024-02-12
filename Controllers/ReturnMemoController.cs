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
        [HttpPost("Generate")]
        public async Task<APIResponse<string>> GenerateReturnMemo(ReturnMemoTokenDetailsDTO returnMemoTokenDetailsDTO)
        {
            APIResponse<string> response = new();
            string userScope = _claimService.GetScope();
            long userId = _claimService.GetUserId();
            string userRole = _claimService.GetRole();
            try
            {
                TokenDetailsDto tokenDetailsDto = await _tokenService.TokenDeatisById(returnMemoTokenDetailsDTO.TokenId);
                if (tokenDetailsDto.StatusId!= (int) Enum.TokenStatus.ObjectedbyTreasuryOfficer)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Info;
                    response.Message = "Invalid Bill!";
                    return response;
                }
                if(await _tokenService.GenerateReturnMemo(returnMemoTokenDetailsDTO.TokenId, returnMemoTokenDetailsDTO.ReferenceNo, userId, 0))
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
        [HttpGet("ReturnMemoBillDetails/{tokenID}")]
        public async Task<APIResponse<ReturnMemoBillDetailsDTO>> ReturnMemoBillDetails(long tokenId)
        {
            APIResponse<ReturnMemoBillDetailsDTO> response = new();
            string userScope = _claimService.GetScope();
            long userId = _claimService.GetUserId();
            string userRole = _claimService.GetRole();
            try
            {
                TokenDetailsDto tokenDetailsDto = await _tokenService.TokenDeatisById(tokenId);
                if (!StatusManager.GetStatus(userRole, (int)Enum.StatusType.ReturnMemo).Contains((int)tokenDetailsDto.StatusId))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Info;
                    response.Message = "Invalid Bill!";
                    return response;
                }
                ReturnMemoBillDetailsDTO returnMemoBillDetailsDTO = await _tokenService.ReturnMemoBillDetails(tokenDetailsDto.TokenId);
                if (returnMemoBillDetailsDTO!=null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = returnMemoBillDetailsDTO;
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
        [HttpGet("ReturnMemoCount")]
        public async Task<APIResponse<ReturnMemoCountDTO>> ReturnMemoCount()
        {
            APIResponse<ReturnMemoCountDTO> response = new();
            string userScope = _claimService.GetScope();
            long userId = _claimService.GetUserId();
            string userRole = _claimService.GetRole();
            try
            {
                ReturnMemoCountDTO returnMemoCountDTO = new ReturnMemoCountDTO
                {
                    AwatingReturnMemo = await _tokenService.TokenCountByStatus(userScope,StatusManager.GetStatus(userRole, "awating-return-memo")),
                    GeneratedReturnMemo = await _tokenService.TokenCountByStatus(userScope,StatusManager.GetStatus(userRole, "generated-return-memo"))
                };
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = returnMemoCountDTO;
                response.Message = "";
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
