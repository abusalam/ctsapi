using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoRegularBillService
    {
        public Task<T> GetPposForRegularBillGeneration<T>(
            short year,
            short month,
            short financialYear,
            string treasuryCode
        );

        public Task<RegularBillListResponseDTO> GetRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? bankId = null,
            long[]? branchIds = null
        );

        public Task<T> SaveRegularPensionBill<T>(
            PpoBillEntryDTO ppoBillEntryDTO,
            short financialYear,
            string treasuryCode
        );
    }
}
