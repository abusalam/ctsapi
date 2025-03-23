using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoRegularBillRepository : IPpoBillRepository
    {
        public Task<Bill?> GetExistingBillForRegularBill(
            long hoaId,
            long branchId,
            DateOnly fromDate,
            DateOnly toDate,
            short financialYear,
            string treasuryCode
        );

        public Task<List<Bill>> GetSavedRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? bankId = null,
            long[]? branchIds = null
        );

        public Task<List<Pensioner>> GetAvailablePensionersForRegularBillGeneration(
            short year,
            short month,
            short financialYear,
            string treasuryCode
        );

        public T GenerateRegularPensionBill<T>(
            Pensioner pensioner,
            PpoBillEntryDTO ppoBillEntryDTO,
            short financialYear,
            string treasuryCode
        );
    }
}
