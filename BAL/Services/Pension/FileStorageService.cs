using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class FileStorageService : BaseService, IFileStorageService
    {
        private readonly IFileStorageRepository _fileStorageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(
            IFileStorageRepository fileStorageRepository,
            IMapper mapper,
            IClaimService claimService,
            ILogger<FileStorageService> logger
        )
            : base(claimService)
        {
            _fileStorageRepository = fileStorageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<T> CreateFileUpload<T>(
            FileEntryDTO fileEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            UploadedFile fileEntity = _mapper.Map<UploadedFile>(fileEntryDTO);
            T response = _mapper.Map<T>(fileEntity);
            try
            {
                fileEntity = _mapper.Map<UploadedFile>(fileEntryDTO);
                SetCreatedBy(fileEntity);
                response = await _fileStorageRepository.SaveUploadedFile<T>(
                    financialYear,
                    treasuryCode,
                    fileEntity
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while saving file with details: {FileEntryDTO}",
                    fileEntryDTO
                );
                response.FillErrorInDataSource(
                    fileEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Service exception occurred while saving file with details: {FileEntryDTO}",
                    fileEntryDTO
                );
                response.FillErrorInDataSource(
                    fileEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            return response;
        }

        public async Task<T> GetFileById<T>(long fileId, string treasuryCode)
        {
            UploadedFile? uploadedFile = new();
            T? response = _mapper.Map<T>(new UploadedFile());
            try
            {
                uploadedFile = await _fileStorageRepository.GetFileById(
                    fileId,
                    treasuryCode,
                    entity => _mapper.Map<UploadedFile>(entity)
                );
                response = _mapper.Map<T>(uploadedFile);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while retrieving file with ID: {FileId}",
                    fileId
                );
                response.FillErrorInDataSource(
                    uploadedFile,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Service exception occurred while retrieving file with ID: {FileId}",
                    fileId
                );
                response.FillErrorInDataSource(
                    uploadedFile,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return response;
        }
    }
}
