using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoBillRepository : IRepository<PpoBill>
    {
        public Task<int> GetNextBillNo(short financialYear, string treasuryCode);

        public Task<Bill?> GetBillByPpoBillId(
            long ppoBillId,
            short financialYear,
            string treasuryCode
        );

        public Task<T> GetPpoFirstBillByPpoId<T>(
            int ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<T> SavePpoBill<T>(PpoBill firstBill, short financialYear, string treasuryCode);

        public T GeneratePensionBill<T>(
            Pensioner pensioner,
            PpoBillEntryDTO ppoBillEntryDTO,
            char billType,
            short financialYear,
            string treasuryCode
        );

        public Task<Bill?> GetExistingBillForRegularBill(
            long hoaId,
            long branchId,
            DateOnly fromDate,
            DateOnly toDate,
            short financialYear,
            string treasuryCode
        );

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
        public Task<DateOnly> LastBillGeneratedUpTo(long ppoId, string treasuryCode);

        public Task<List<PpoComponentRevision>> GetPpoComponentRevisionsByPensionerId(
            long pensionerId
        );

        public Task<Pensioner?> GetPensionerByPpoId(int ppoId, string treasuryCode);

        public Task<List<Bill>> GetSavedRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? bankId = null,
            long[]? branchIds = null
        );

        public Task<List<Pensioner>> GetAvailablePensionersForBillGeneration(
            short year,
            short month,
            char billType,
            short financialYear,
            string treasuryCode
        );

        public Task<List<Pensioner>> GetPensionersForFirstBillGeneration(
            short financialYear,
            string treasuryCode
        );
        public Task<List<Pensioner>> GetPensionersForFirstBillPrint(
            short financialYear,
            string treasuryCode
        );
    }
}
