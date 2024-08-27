using System.Net.Mime;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v1")]
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
        protected void FillErrorMesageFromDataSource<T>(JsonAPIResponse<T> response) where T : BaseDTO
        {
            if(response.Result?.DataSource != null) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = ((dynamic)response.Result.DataSource).Message;
            }
        }

        protected void FillException<T>(JsonAPIResponse<T> response, Exception exception) where T : BaseDTO
        {
            response.Message = exception?.InnerException?.Message ?? exception?.Message;
            response.ApiResponseStatus = Enum.APIResponseStatus.Error;
            response.Result = null;
        }
    }
}