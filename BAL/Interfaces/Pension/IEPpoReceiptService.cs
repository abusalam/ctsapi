using CTS_BE.DTOs;
using CTS_BE.DAL.Entities.Pension;

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
            string pensionApplnNo,
            EPpoReceiptWithdrawlEntryDTO ePpoReceiptWithdrawlEntryDTO,
            string treasuryCode,
            short financialYear
        );
        public Task<T> GetEPpoReceiptByPensionApplnNo<T>(
            string pensionApplnNo,
            string treasuryCode,
            short financialYear
        );
        public Task<T> GetEPpoReceiptById<T>(
            long receiptId,
            string treasuryCode,
            short financialYear
        );
        public Task<List<T>> GetUnusedEPpoReceipts<T>(
            string treasuryCode,
            short financialYear
        );
    }
}