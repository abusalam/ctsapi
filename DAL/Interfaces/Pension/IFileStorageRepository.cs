using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IFileStorageRepository
    {
        public Task<T> SaveUploadedFile<T>(
            short financialYear,
            string treasuryCode,
            UploadedFile fileEntity
        );
        public Task<T?> GetFileById<T>(
            long fileId,
            string treasuryCode,
            Expression<Func<UploadedFile, T>> selectExpression
        );
    }
}
