using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers
{
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        private readonly IClaimService _claimService;
        private const short CURRENT_FINANCIAL_YEAR = 2024;

        public ApiBaseController(
            IClaimService claimService
            )
        {
            _claimService = claimService;
        }

        protected string GetTreasuryCode()
        {
            return _claimService.GetScope();
        }

        protected short GetCurrentFyYear()
        {
            return CURRENT_FINANCIAL_YEAR;
        }

        [HttpPost("echo")]
        [Produces("application/json")]
        [Tags("Pension", "Echo Request in Response")]
        public async Task<JsonAPIResponse<object>> ControlEchoRequestInResponse(object req)
        {
            JsonAPIResponse<object> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Echoing Request",
                Result = req
            };
            return await Task.FromResult(response);
        }

        [HttpPost("date-only")]
        [Produces("application/json")]
        [Tags("Pension", "DateOnly Echo")]
        public async Task<JsonAPIResponse<DateOnlyDTO>> ControlSetDateOnly(DateOnlyDTO dateOnly)
        {
            JsonAPIResponse<DateOnlyDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Writing DateOnly",
                Result = dateOnly
            };
            return await Task.FromResult(response);
        }

        [HttpGet("date-only")]
        [Produces("application/json")]
        [Tags("Pension", "DateOnly Echo")]
        public async Task<JsonAPIResponse<DateOnly>> ControlGetDateOnly()
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
            JsonAPIResponse<DateOnly> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Reading DateOnly",
                Result = dateOnly
            };
            return await Task.FromResult(response);
        }
    
    }
}