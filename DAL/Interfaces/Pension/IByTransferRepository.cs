using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IByTransferRepository : IRepository<BytransferHead>
    {
        Task<T> SaveByTransferHead<T>(
            BytransferHead byTransferHeadEntity,
            short financialYear,
            string treasuryCode
        );

        Task<BytransferHead?> GetByTransferHeadByIdAsync(
            long byTransferHeadId,
            string treasuryCode
        );
    }
}
