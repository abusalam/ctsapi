using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoBillService : IBaseService
    {
        public Task<T> GetPposForFirstBillGeneration<T>(short financialYear, string treasuryCode);

        public Task<T> GetPposForFirstBillPrint<T>(short financialYear, string treasuryCode);

        public Task<T> GetPposForBillGeneration<T>(
            short year,
            short month,
            char billType,
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

        public Task<PpoBillResponseDTO> GetFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<T> GenerateFirstPensionBill<T>(
            InitiateFirstPensionBillEntryDTO initiateFirstPensionBillDTO,
            short financialYear,
            string treasuryCode
        );

        public Task<T> SaveFirstPensionBill<T>(
            InitiateFirstPensionBillEntryDTO initiateFirstPensionBillDTO,
            short financialYear,
            string treasuryCode
        );

        public Task<T> SaveRegularPensionBill<T>(
            PpoBillEntryDTO ppoBillEntryDTO,
            short financialYear,
            string treasuryCode
        );
    }
}
