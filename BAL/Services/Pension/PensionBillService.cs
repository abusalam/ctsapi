using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionBillService : BaseService, IPensionBillService
    {
        public Task<T> GetPensionerFirstBill<T>(
                int ppoId,
                DateOnly toDate,
                short financialYear,
                string treasuryCode
            )
        {
            throw new NotImplementedException();
        }
    }
}