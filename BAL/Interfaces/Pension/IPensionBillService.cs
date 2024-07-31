using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionBillService
    {
        public Task<T> GetPensionerFirstBill<T>(
            int ppoId,
            DateOnly toDate,
            short financialYear,
            string treasuryCode
        );
    }
}