using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Tags("Pension")]
    [Route("api/v1")]
    public class ApiBase : ControllerBase
    {
        private readonly IClaimService _claimService;
        private const short CURRENT_FINANCIAL_YEAR = 2024;

        public ApiBase(
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