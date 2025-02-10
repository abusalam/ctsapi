using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPaymentService : IBaseService
    {
        public Task<PpoBillResponseDTO> GetPaymentFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<RegularBillListResponseDTO> GetPaymentRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? bankId = null,
            long[]? branchIds = null
        );
    }
}
