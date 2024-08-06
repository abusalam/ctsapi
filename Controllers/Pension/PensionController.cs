using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1")]
    public class PensionController : ApiBaseController
    {
        public PensionController(
                IClaimService claimService
            ) : base(claimService) {}

        [HttpPost("echo")]
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