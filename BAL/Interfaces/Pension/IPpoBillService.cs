using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoBillService : IBaseService
    {
        public Task<T> GetAllPposForFirstBillGeneration<T>(
            short financialYear,
            string treasuryCode
        ) where T : BaseDTO;

        public Task<T> GetAllPposForBillGeneration<T>(
            short year,
            short month,
            char billType,
            short financialYear,
            string treasuryCode
        ) where T : BaseDTO;
        public Task<RegularBillListResponseDTO> GetRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? bankId = null
        );
        
        public Task<T> SavePpoBill<T>(
            PensionerFirstBillResponseDTO firstBill,
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