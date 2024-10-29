using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IEPpoReceiptRepository :
        IRepository<EppoReceipt>
    {
        public Task<T> SaveEPpoReceipt<T>(
            EppoReceipt entity,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        );
        public Task<T> SaveRevisedEPpoReceipt<T>(
            EppoRevision entity,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        );
        public Task<T> WithdrawEPpoReceipt<T>(
            string applicationNo,
            EppoReceipt entity,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        );
        public Task<T> GetEPpoReceiptByApplicationNo<T>(
            string applicationNo,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        );
        public Task<T> GetPpoIdByPpoNo<T>(
            string ppoNo,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        );
    }
}