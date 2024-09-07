using CTS_BE.DTOs;
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

        public Task<PpoBill?> GetPpoBillByPpoId(
            int ppoId,
            string treasuryCode
        );

        public Task<PpoBill?> GetPpoBillByPpoId(
            int ppoId,
            string treasuryCode,
            short financialYear
        );

        public Task<T> SavePpoBill<T>(
            PpoBill firstBill,
            short financialYear,
            string treasuryCode
        );
    }
}