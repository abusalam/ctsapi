using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoByTransferRepository : IRepository<PpoBytransfer>
    {
        public Task<T> SavePpoByTransferHead<T>(PpoBytransfer ppobyTransferHeadEntity);

        public Task<List<T>?> GetAllPpoByTransferByPpoIdAsync<T>(
            int ppoId,
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoBytransfer, T>> selectExpression
        );

        public Task<PpoBytransfer?> GetPPOByTransferByIdAsync(long id);
        public Task<T> UpdatePPOByTransfer<T>(PpoBytransfer ppoByTransferEntity);
        public Task<bool> IsUsedInOtherTables(long byTransferHeadId);
    }
}
