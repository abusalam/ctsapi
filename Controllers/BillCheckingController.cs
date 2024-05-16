using CTS_BE.BAL.Interfaces.billing;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Mvc;
using CTS_BE.BAL.Interfaces.master;
using CTS_BE.Filters;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BillCheckingController : Controller
    {
        protected readonly ITpBillService _tpBillService;
        protected readonly ITokenService _tokenService;
        protected readonly ITokenHasObjectionService _tokenHasObjectionService;
        protected readonly IClaimService _claimService;
        public BillCheckingController(ITpBillService tpBillService,ITokenService tokenService,ITokenHasObjectionService tokenHasObjectionService,IClaimService claimService)
        {
            _tpBillService = tpBillService;
            _tokenService = tokenService;
            _tokenHasObjectionService = tokenHasObjectionService;
            _claimService = claimService;
        }
        [HttpGet("get-bill-details")]
        public async Task<APIResponse<BillCheckingBillDetailsDto>> BillDetails([FromQuery]long tokenId)
        {
            APIResponse<BillCheckingBillDetailsDto> response = new();
            try
            {
                string userRole = _claimService.GetRole();
                TokenDetailsDto tokenDetailsDto = await _tokenService.TokenDeatisById(tokenId);
                if (tokenDetailsDto == null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Token Not found";
                    return response;
                }
                //if (!StatusManager.GetStatus(userRole,(int)Enum.StatusType.BillChecking).Contains((int)tokenDetailsDto.StatusId))
                //{
                //    response.apiResponseStatus = Enum.APIResponseStatus.Info;
                //    response.Message = "This bill has been checked!";
                //    return response;
                //}
                // BillDetailsDetailsByRef billDetailsByRef = await _tpBillService.BillDetailsByRefNo(tokenDetailsDto.ReferenceNo);
                BillDetailsDetailsByRef billDetailsByRef = await _tpBillService.BillDetailsByBillId(tokenDetailsDto.BillId);
                if (billDetailsByRef == null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Bill Not found";
                    return response;
                }
                BillCheckingBillDetailsDto billCheckingBillDetailsDto = new BillCheckingBillDetailsDto
                {
                    TokenDetails = tokenDetailsDto,
                    BillDetailsDetails = billDetailsByRef,
                };
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = billCheckingBillDetailsDto;
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
        [Authorize("permissions:can-bill-check|roles:accountant,dealling-assistant,treasury-officer")]
        [HttpPost("BillCheck")]
        public async Task<APIResponse<string>> BillCheck(BillCheckingDto billCheckingDto)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();
                string userRole = _claimService.GetRole();
                TokenDetailsDto tokenDetailsDto = await _tokenService.TokenDeatisById(billCheckingDto.TokenId);
                if (!StatusManager.GetStatus(userRole, (int)Enum.StatusType.BillChecking).Contains((int)tokenDetailsDto.StatusId))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Info;
                    response.Message = "This bill has been checked!";
                    return response;
                }
                if (await _tokenHasObjectionService.Insert(billCheckingDto, userId,OwnTypeManager.GetOwnType(userRole)))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Bill Checked!";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Bill Checking Faild.";
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
