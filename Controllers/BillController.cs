using CTS_BE.BAL.Interfaces.billing;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BillController : Controller
    {
        private readonly ITpBillService _tpBillService;
        public BillController(ITpBillService tpBillService)
        {
            _tpBillService = tpBillService;
        }
        [Authorize("permissions:can-receive-bill|roles:clerk")]
        [HttpGet("GetBills")]
        public async Task<APIResponse<IEnumerable<BillsListDTO>>> GetBills()
        {
            APIResponse<IEnumerable<BillsListDTO>> response = new();
            try
            {
                IEnumerable<BillsListDTO> allBills = await _tpBillService.NewBills();
                if (allBills != null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = allBills;
                    response.Message = "Data Collect Successfully";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Failed";
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpGet("GetRefBillDetails/{billRefNo}")]
        public async Task<APIResponse<BillDetailsDetailsByRef>> GetRefBillDetails(string billRefNo)
        {
            APIResponse<BillDetailsDetailsByRef> response = new();
            try
            {
                BillDetailsDetailsByRef bailDeatils = await _tpBillService.BillDetailsByRefNo(billRefNo);
                if (bailDeatils != null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.result = bailDeatils;
                    response.Message = "Data Collect Successfully";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Failed";
                return response;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpGet("GetNumberOfBills/{billStatus}")]
        public async Task<APIResponse<int>> NumberOfBills(int billStatus)
        {
            APIResponse<int> response = new();
            try
            {
                int numberOfBiles =  await _tpBillService.BillCountByStatus(billStatus);
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.result = numberOfBiles;
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
