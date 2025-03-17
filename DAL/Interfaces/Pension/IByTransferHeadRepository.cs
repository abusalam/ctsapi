using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IByTransferHeadRepository : IRepository<BytransferHead>
    {
        public Task<T> SaveByTransferHead<T>(BytransferHead byTransferHeadEntity);

        public Task<BytransferHead?> GetByTransferHeadMapById(long id);

        public Task<AccountHead?> GetAccountHeadAsync(long id);

        public Task<List<BytransferHead>> GetAllByTransferHeadsAsync();

        public Task<bool> IsUsedInOtherTables(long byTransferHeadId);

        public Task<T> UpdateByTransferHead<T>(BytransferHead byTransferHeadEntity);

        public Task<T> RemoveByTransferHeadAsync<T>(long id);
        public Task<BytransferHead?> GetExistingByTransferHeadAsync(
            BytransferHead byTransferHeadEntity
        );
    }
}
