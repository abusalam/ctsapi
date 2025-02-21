using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPaymentService : IBaseService
    {
        public Task<RegularBillListResponseDTO> GetPayment(
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
