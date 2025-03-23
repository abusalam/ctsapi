using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoFirstBillService
    {
        public Task<T> GetPposForFirstBillGeneration<T>(short financialYear, string treasuryCode);

        public Task<T> GetPposForFirstBillPrint<T>(short financialYear, string treasuryCode);

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
    }
}
