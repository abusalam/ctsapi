using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IFileStorageService
    {
        public Task<T> CreateFileUpload<T>(
            FileEntryDTO fileEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<T> GetFileById<T>(long fileId, string treasuryCode);
    }
}
