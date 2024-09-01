using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoBillService : IBaseService
    {
        public Task<T> SaveFirstBill<T>(
            InitiateFirstPensionBillResponseDTO firstBill,
            short financialYear,
            string treasuryCode
        );

        public Task<PpoBillResponseDTO> GetFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        );
    }
}