using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoBillRepository : IRepository<PpoBill>
    {
        public Task<int> GetNextBillNo(
            short financialYear,
            string treasuryCode
        );

        public Task<PpoBill> SavePpoBillBreakups(long ppoBillId, List<PpoBillBreakup> ppoBillBreakups);

        public Task<PpoBill?> GetPpoFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        );
    }
}