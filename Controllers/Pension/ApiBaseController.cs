using System.Net.Mime;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Tags("Pension")]
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
    
    }
}