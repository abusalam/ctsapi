using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoByTransferAmountRepository
    {
        public Task<T> CreatePpoByTransfer<T>(PpoBytransfer ppobyTransferHeadEntity);

        public Task<List<T>> GetAllPpoByTransferByPpoIdAsync<T>(
            Expression<Func<PpoBytransfer, T>> selectExpression,
            int ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<PpoBytransfer?> GetPpoByTransferByIdAsync(long id);

        public Task<T> UpdatePpoByTransfer<T>(PpoBytransfer ppoByTransferEntity);

        public Task<bool> IsUsedInOtherTables(long byTransferHeadId);

        public Task<T> RemovePpoByTransferAsync<T>(long id);

        public Task<PpoBytransfer?> ValidatePpoByTransferOverlapAsync(
            PpoBytransfer ppoByTransferEntity,
            int ppoId
        );

        public Task<PpoBytransfer?> GetExistingPpoByTransferAsync(
            PpoBytransfer ppoByTransferEntity
        );
    }
}
