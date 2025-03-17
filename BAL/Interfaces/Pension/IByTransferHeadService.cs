using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IByTransferHeadService : IBaseService
    {
        public Task<T> SaveByTransferHead<T>(ByTransferHeadEntryDTO byTransferHeadEntryDTO);

        public Task<T> GetByTransferHeadMapById<T>(long Id);

        public Task<List<T>> GetByTransferHeadMaps<T>();

        public Task<T> UpdateByTransferHead<T>(long id, ByTransferHeadUpdateDTO updateDTO);

        public Task<T> DeleteByTransferHeadMapById<T>(long id);
    }
}
