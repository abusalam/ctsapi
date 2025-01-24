using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IByTransferService : IBaseService
    {
        Task<T> SaveByTransferHead<T>(
            ByTransferHeadEntryDTO byTransferHeadEntryDTO,
            short financialYear,
            string treasuryCode
        );
        Task<T> GetByTransferHeadById<T>(long byTransferHeadId, string treasuryCode);
    }
}
