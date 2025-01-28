using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IByTransferService : IBaseService
    {
        Task<T> SaveByTransferHead<T>(ByTransferHeadEntryDTO byTransferHeadEntryDTO);

        Task<T> GetByTransferHeadById<T>(long byTransferHeadId);
        public Task<List<T>> GetAllByTransferHeads<T>();
    }
}
