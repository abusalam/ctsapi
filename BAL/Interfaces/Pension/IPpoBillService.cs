using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoBillService : IBaseService
    {
        public Task<T> SavePpoBill<T>(
            PensionerFirstBillResponseDTO firstBill,
            short financialYear,
            string treasuryCode
        ) where T : PpoBillResponseDTO;

        public Task<PpoBillResponseDTO> GetFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        );
    }
}