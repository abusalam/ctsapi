using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class FileStorageController(
        IFileStorageService ppoReceiptService,
        IFileStorageService fileStorageService,
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IFileStorageService _ppoReceiptService = ppoReceiptService;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        [HttpPost("storage/file")]
        [Tags("Pension: File Storage")]
        [OpenApi]
        public async Task<JsonAPIResponse<FileResponseDTO>> StoreFile(FileEntryDTO fileEntryDTO)
        {
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
