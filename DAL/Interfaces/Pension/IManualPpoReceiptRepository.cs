using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IManualPpoReceiptRepository : IRepository<PpoReceipt>
    {
        public Task<List<T>> GetAllUnusedPpoReceipts<T>(
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoReceipt, T>> selectExpression
        );
        public Task<List<T>> GetPpoReceiptsAsync<T>(
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoReceipt, T>> selectExpression
        );
        public Task<T> CreatePpoReceiptWithTreasuryReceiptNo<T>(
            short finYear,
            string treasuryCode,
            PpoReceipt ppoReceipt
        ) where T : BaseDTO;
    }
}