using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class FileStorageRepository
        : Repository<UploadedFile, PensionDbContext>,
            IFileStorageRepository
    {
        private readonly IMapper _mapper;
        private readonly PensionDbContext _context;

        public FileStorageRepository(IMapper mapper, PensionDbContext context)
            : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<T?> GetFileById<T>(
            long fileId,
            string treasuryCode,
            Expression<Func<UploadedFile, T>> selectExpression
        )
        {
            return await _context
                .UploadedFiles.Where(entity => entity.ActiveFlag && entity.Id == fileId)
                .Select(selectExpression)
                .FirstOrDefaultAsync();
        }

        public async Task<T> SaveUploadedFile<T>(
            short financialYear,
            string treasuryCode,
            UploadedFile fileEntity
        )
        {
            T response = _mapper.Map<T>(fileEntity);
            try
            {
                UploadedFile? uploadedFile = _context
                    .UploadedFiles.Where(entity =>
                        entity.ActiveFlag == true && entity.FileName == fileEntity.FileName
                    )
                    .FirstOrDefault();

                if (uploadedFile != null)
                {
                    response.FillErrorInDataSource(uploadedFile, "File already exists");
                    return response;
                }
                fileEntity.FilePath = treasuryCode + "/" + financialYear + "/";
                FileExtensionContentTypeProvider provider = new();
                var extension = provider.TryGetContentType(
                    fileEntity.FileName,
                    out string? mimeType
                );
                fileEntity.FileMimeType = mimeType ?? "application/octet-stream";
                _context.UploadedFiles.Add(fileEntity);

                if (await _context.SaveChangesAsync() == 0)
                {
                    fileEntity.Contents = null!;
                    response.FillErrorInDataSource(
                        fileEntity,
                        "Failed to upload file. Please try again after sometime."
                    );
                    return response;
                }
                response = _mapper.Map<T>(fileEntity);
            }
            catch (DbUpdateException ex)
            {
                response.FillErrorInDataSource(
                    fileEntity,
                    ex.InnerException?.Message ?? ex.Message
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    fileEntity,
                    ex.InnerException?.Message ?? ex.Message
                );
                return response;
            }
            return response;
        }
    }
}
