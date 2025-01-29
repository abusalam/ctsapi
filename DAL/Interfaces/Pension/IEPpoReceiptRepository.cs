using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IEPpoReceiptRepository : IRepository<EppoReceipt>
    {
        public Task<T> SaveEPpoReceipt<T>(
            EppoReceipt entity,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        );
        public Task<T> SaveRevisedEPpoReceipt<T>(
            EppoRevision entity,
            Expression<Func<EppoReceipt, T>> selectExpression
        );
        public Task<T> WithdrawEPpoReceipt<T>(
            string pensionApplnNo,
            EppoReceipt entity,
            string treasuryCode,
            short financialYear,
            string? reason,
            char flag,
            Expression<Func<EppoReceipt, T>> selectExpression
        );
        public Task<EppoReceipt?> GetEPpoReceiptByPensionApplnNo(
            string pensionApplnNo,
            string treasuryCode,
            short financialYear
        );
        public Task<EppoReceipt?> GetPpoIdByPpoNo(
            string ppoNo,
            string treasuryCode,
            short financialYear
        );
        public Task<EppoReceipt?> GetEPpoReceiptById(
            long receiptId,
            string treasuryCode,
            short financialYear
        );

        public Task<List<T>> GetUnusedEPpoReceipts<T>(
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        );
    }
}
