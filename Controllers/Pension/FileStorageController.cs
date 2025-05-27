using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class FileStorageController(
        IFileStorageService ppoReceiptService,
        IFileStorageService fileStorageService,
        IClaimService claimService,
        ILogger<FileStorageController> logger
    ) : ApiBaseController(claimService)
    {
        private readonly IFileStorageService _ppoReceiptService = ppoReceiptService;
        private readonly IFileStorageService _fileStorageService = fileStorageService;
        private readonly ILogger<FileStorageController> _logger = logger;

        [HttpPost("storage/file")]
        [Tags("Pension: File Storage")]
        [OpenApi]
        public async Task<JsonAPIResponse<FileResponseDTO>> StoreFile(FileEntryDTO fileEntryDTO)
        {
            _logger.LogInformation(
                "Received request to store file with details: {FileEntryDTO}",
                fileEntryDTO
            );
            JsonAPIResponse<FileResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"File Stored Successfully!",
            };

            try
            {
                response.Result = await _fileStorageService.CreateFileUpload<FileResponseDTO>(
                    fileEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (Exception ex)
            {
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }
            return response;
        }

        [HttpGet("storage/file/{fileId}")]
        [Tags("Pension: File Storage")]
        [OpenApi]
        public async Task<JsonAPIResponse<FileResponseDTO>> GetFileById(int fileId)
        {
            _logger.LogInformation("Received request to get file by ID: {FileId}", fileId);
            JsonAPIResponse<FileResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"File Received Successfully!",
            };
            try
            {
                response.Result = await _fileStorageService.GetFileById<FileResponseDTO>(
                    fileId,
                    GetTreasuryCode()
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching file with ID: {FileId}",
                    fileId
                );
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }
    }
}
