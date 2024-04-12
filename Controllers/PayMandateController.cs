﻿using CTS_BE.BAL.Interfaces.paymandate;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PayMandateController : Controller
    {
        private readonly IPaymandateService _paymandateService;
        public PayMandateController(IPaymandateService paymandateService)
        {
            _paymandateService = paymandateService;
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
        [HttpPost("newShortList")]

        public async Task<APIResponse<string>> NewShortlist(List<NewShortlistDTO> newShortlistDTO)
        {
            APIResponse<string> response = new APIResponse<string>();
            try
            {
                if (await _paymandateService.NewShortList(1,newShortlistDTO))
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Paymandate Shortlisted Successfully.";
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = "Shortlist Faild.";
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
