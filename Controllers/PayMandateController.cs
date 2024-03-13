using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.paymandate;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PayMandateController : Controller
    {
        private readonly IPaymandateService _paymandateService;
        private readonly IVoucherService _voucherService;
        private readonly IClaimService _claimService;

        public PayMandateController(IPaymandateService paymandateService, IVoucherService voucherService, IClaimService claimService)
        {
            _paymandateService = paymandateService;
            _voucherService = voucherService;
            _claimService = claimService;
        }
        [HttpGet("Sortlist")]
        public async Task<APIResponse<IEnumerable<PayMandateShortListDTO>>> SortList()
        {
            APIResponse<IEnumerable<PayMandateShortListDTO>> response = new();
            try
            {
                IEnumerable<PayMandateShortListDTO> payMandateShortListDTOs = await _paymandateService.TokenForShortList();
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = payMandateShortListDTOs;
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
        [HttpPost("CreateSortlist")]
        public async Task<APIResponse<string>> PayMandateSort(List<CreateShrtListDTO> createShrtListDTOs)
        {
            APIResponse<string> response = new();
            try
            {
                long userId = _claimService.GetUserId();
                if(await _voucherService.InsertNewVoucher(createShrtListDTOs, userId))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Sortlisted Successfully";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Sortlisted Faild";
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
