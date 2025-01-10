using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [ApiController]
    [Route("api/v1/storage")]
    public class FileStorageController : ApiBaseController
    {
        private readonly IFileStorageService _ppoReceiptService;
        private readonly IFileStorageService _fileStorageService;

        public FileStorageController(
            IFileStorageService ppoReceiptService,
            IFileStorageService fileStorageService,
            IClaimService claimService
        )
            : base(claimService)
        {
            _ppoReceiptService = ppoReceiptService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost("file")]
        [Tags("Pension: File Storage")]
        [OpenApi]
        public async Task<JsonAPIResponse<FileResponseDTO>> StoreFile(FileEntryDTO fileEntryDTO)
        {
            JsonAPIResponse<FileResponseDTO> response = new();

            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _fileStorageService.CreateFileUpload<FileResponseDTO>(
                        fileEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    ),
                    Message = $"File Stored Successfully!",
                };
            }
            catch (Exception ex)
            {
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
            }
            return response;
        }

        [HttpGet("file/{fileId}")]
        [Tags("Pension: File Storage")]
        [OpenApi]
        public async Task<JsonAPIResponse<FileResponseDTO>> GetFileById(int fileId)
        {
            JsonAPIResponse<FileResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"File received sucessfully!",
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
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }
    }
}
