using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class FileStorageRepository(
        IMapper mapper,
        PensionDbContext context,
        ILogger<FileStorageRepository> logger
    ) : IFileStorageRepository
    {
        private readonly IMapper _mapper = mapper;
        private readonly PensionDbContext _context = context;
        private readonly ILogger<FileStorageRepository> _logger = logger;

        public async Task<T?> GetFileById<T>(
            long fileId,
            string treasuryCode,
            Expression<Func<UploadedFile, T>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching file with ID: {FileId} for treasury code: {TreasuryCode}",
                fileId,
                treasuryCode
            );
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
            _logger.LogInformation(
                "Saving uploaded file with name: {FileName} for treasury code: {TreasuryCode} and financial year: {FinancialYear}",
                fileEntity.FileName,
                treasuryCode,
                financialYear
            );
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
                    _logger.LogWarning(
                        "File with name: {FileName} already exists for treasury code: {TreasuryCode}",
                        fileEntity.FileName,
                        treasuryCode
                    );
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
                    _logger.LogError(
                        "Failed to save file with name: {FileName} for treasury code: {TreasuryCode}",
                        fileEntity.FileName,
                        treasuryCode
                    );
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
                _logger.LogError(
                    ex,
                    "Database update error while saving file with name: {FileName} for treasury code: {TreasuryCode}",
                    fileEntity.FileName,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    fileEntity,
                    ex.InnerException?.Message ?? ex.Message
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while saving file with name: {FileName} for treasury code: {TreasuryCode}",
                    fileEntity.FileName,
                    treasuryCode
                );
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
