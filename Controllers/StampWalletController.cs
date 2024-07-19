using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.BAL.Services.stamp;
using CTS_BE.Common;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
using CTS_BE.Enum;
using CTS_BE.Filters;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StampWalletController : Controller
    {
        private readonly IStampWalletService _stampWalletService;

        public StampWalletController(IStampWalletService stampWalletService)
        {
            _stampWalletService = stampWalletService;
        }

        [HttpPost("CreateOrUpdateStampWallet")]
        public async Task<APIResponse<bool>> CreateOrUpdateStampWallet(StampWalletInsertDTO stampWallet)
        {
            APIResponse<bool> response = new();
            try
            {
                if (stampWallet != null)
                {
                    if(await _stampWalletService.CreateOrUpdateStampWallet(stampWallet))
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.WalletUpdated;
                        response.result = true;
                        return response;
                    }
                    else
                    {
                        response.apiResponseStatus = Enum.APIResponseStatus.Error;
                        response.result = false;
                        response.Message = AppConstants.InvalidTreasuryCode;
                        return response;
                    }
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                response.Message = AppConstants.MissingField;
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        
        [HttpGet("getStampWalletBalanceByTreasuryCode")]
        public async Task<APIResponse<StampWalletBalanceDTO>> getStampWalletBalanceByTreasuryCode(string treasuryCode, long combinationId)
        {
            APIResponse<StampWalletBalanceDTO> response = new();
            try
            {
                        StampWalletBalanceDTO balance = await _stampWalletService.GetWalletBalanceByTreasuryCode(treasuryCode, combinationId);
                        response.apiResponseStatus = Enum.APIResponseStatus.Success;
                        response.Message = AppConstants.DataFound;
                        response.result = balance;
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

