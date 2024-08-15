using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoBillBreakupService : BaseService, IPpoBillBreakupService
    {
        public PpoBillBreakupService(IClaimService claimService) : base(claimService)
        {
        }
    }
}