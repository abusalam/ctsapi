using CTS_BE.BAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly IBranchService _branchService;

        public BankController(IBankService bankService, IBranchService branchService)
        {
            _bankService = bankService;
            _branchService = branchService;
        }
        [HttpGet("get-banks")]
        public async Task<APIResponse<IEnumerable<DropdownDTO>>> Banks()
        {
            APIResponse<IEnumerable<DropdownDTO>> response = new();
            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = "";
                response.result= await _bankService.AllBanks();
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
        [HttpGet("get-bank-branchs")]
        public async Task<APIResponse<IEnumerable<DropdownDTO>>> BranchsByBank(short bankCode)
        {
            APIResponse<IEnumerable<DropdownDTO>> response = new();
            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = "";
                response.result = await _branchService.GetBranchsByBankCode(bankCode);
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
        [HttpGet("get-branch")]
        public async Task<APIResponse<BranchDeatilsDTO>> BranchByCode(short branchCode)
        {
            APIResponse<BranchDeatilsDTO> response = new();
            try
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = "";
                response.result = await _branchService.GetBranchByBranchCode(branchCode);
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = Ex.Message;
                return response;
            }
        }
    }
}
