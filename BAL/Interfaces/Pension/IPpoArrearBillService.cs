using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoArrearBillService
    {
        public Task<T> GetPposForArrearBillGeneration<T>(short financialYear, string treasuryCode);

        public Task<PpoArrearBillResponseDTO> GetArrearBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<T> GenerateArrearPensionBill<T>(
            PpoArrearBillEntryDTO ppoArrearBillEntryDTO,
            short financialYear,
            string treasuryCode
        );
    }
}
