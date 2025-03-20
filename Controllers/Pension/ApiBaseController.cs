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
    public class ApiBaseController(IClaimService claimService) : ControllerBase
    {
        private readonly IClaimService _claimService = claimService;

        protected string GetTreasuryCode()
        {
            return _claimService.GetScope();
        }

        protected short GetCurrentFyYear()
        {
            return _claimService.GetFinancialYear();
        }

        protected void FillErrorMessageFromDataSource<T>(JsonAPIResponse<T> response)
            where T : BaseDTO
        {
            if (response.Result?.DataSource != null)
            {
                response.Message = ((dynamic)response.Result.DataSource).Message;
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
            }
        }

        protected void FillException<T>(JsonAPIResponse<T> response, Exception exception)
            where T : BaseDTO
        {
            response.Message =
                $"ControllerException: {exception?.InnerException?.Message ?? exception?.Message} {exception?.StackTrace}";
            response.ApiResponseStatus = Enum.APIResponseStatus.Error;
            response.Result = null;
        }
    }
}
