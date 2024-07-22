using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}