using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoBillRepository
    {
        public Task<int> GetNextBillNo(short financialYear, string treasuryCode);

        public Task<Bill?> GetBillByPpoBillId(
            long ppoBillId,
            short financialYear,
            string treasuryCode
        );

        public Task<T> SavePpoBill<T>(PpoBill firstBill, short financialYear, string treasuryCode);

        public Task<long> GetHoaIdByPpoId(long ppoId, short financialYear, string treasuryCode);

        public Task<bool> IsPpoApproved(long ppoId, short financialYear, string treasuryCode);

        public Task<bool> IsFirstBillAlreadyGenerated(
            long ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<bool> IsRegularBillAlreadyGenerated(
            long ppoId,
            int month,
            int year,
            short financialYear,
            string treasuryCode
        );

        public Task<bool> IsArrearBillAlreadyGenerated(
            long ppoId,
            short financialYear,
            string treasuryCode
        );
        public Task<DateOnly> LastBillGeneratedUpTo(long ppoId, string treasuryCode);

        public Task<List<PpoComponentRevision>> GetPpoComponentRevisionsByPensionerId(
            long pensionerId
        );

        public Task<Pensioner?> GetPensionerByPpoId(int ppoId, string treasuryCode);
    }
}
