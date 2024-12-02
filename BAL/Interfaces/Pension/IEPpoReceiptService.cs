using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IEPpoReceiptService : IBaseService
    {
        public Task<T> CreateEPpoReceipt<T>(
            EPpoReceiptEntryDTO ePpoReceiptEntryDTO,
            string treasuryCode,
            short financialYear
        );
        public Task<T> CreateEPpoReceiptRevision<T>(
            EPpoReceiptRevisionEntryDTO ePpoReceiptRevisionEntryDTO,
            string treasuryCode,
            short financialYear
        );
        public Task<T> RegisterEPpoReceiptWithdrawal<T>(
            string pension_appln_no,
            EPpoReceiptWithdrawlEntryDTO ePpoReceiptWithdrawlEntryDTO,
            string treasuryCode,
            short financialYear
        );
        public Task<T> GetEPpoReceiptByPension_Appln_No<T>(
            string pension_appln_no,
            string treasuryCode,
            short financialYear
        );
        public Task<T> GetPpoIdByPpoNo<T>(
            string ppoNo,
            string treasuryCode,
            short financialYear
        );
    }
}