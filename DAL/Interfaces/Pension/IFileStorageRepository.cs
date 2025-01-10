using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IFileStorageRepository : IRepository<UploadedFile>
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
