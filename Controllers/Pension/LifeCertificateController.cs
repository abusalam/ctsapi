using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class LifeCertificateController : ApiBaseController
    {
        private readonly ILifeCertificateService _lifeCertificateService;
        private readonly ILogger<LifeCertificateController> _logger;

        public LifeCertificateController(
            ILifeCertificateService lifeCertificateService,
            IClaimService claimService,
            ILogger<LifeCertificateController> logger
        )
            : base(claimService)
        {
            _lifeCertificateService = lifeCertificateService;
            _logger = logger;
        }

        [HttpPost("lifecertificate")]
        [Tags("Pension: Life Certificate")]
        [OpenApi]
        public async Task<JsonAPIResponse<LifeCertificateResponseDTO>> SubmitLifeCertificate(
            LifeCertificateEntryDTO lifeCertificateEntryDTO
        )
        {
            JsonAPIResponse<LifeCertificateResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Life Certificate saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _lifeCertificateService.CreateLifeCertificate<LifeCertificateResponseDTO>(
                        lifeCertificateEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while submitting life certificate with data: {Data}",
                    lifeCertificateEntryDTO
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

        [HttpGet("lifecertificate/ppo/{ppoId}")]
        [Tags("Pension: Life Certificate")]
        [OpenApi]
        public async Task<JsonAPIResponse<LifeCertificateResponseDTO>> GetLifeCertificateByPpoId(
            int ppoId
        )
        {
            _logger.LogInformation(
                "Received request to get life certificate for PPO ID: {PpoId}",
                ppoId
            );
            JsonAPIResponse<LifeCertificateResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Life Certificate received sucessfully!",
            };
            try
            {
                response.Result =
                    await _lifeCertificateService.GetLifeCertificateByPpoId<LifeCertificateResponseDTO>(
                        ppoId,
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching life certificate for PPO ID: {PpoId}",
                    ppoId
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

        [HttpPut("lifecertificate/ppo/{ppoId}")]
        [Tags("Pension: Life Certificate")]
        [OpenApi]
        public async Task<JsonAPIResponse<LifeCertificateResponseDTO>> UpdateLifeCertificateByPpoId(
            int ppoId,
            LifeCertificateEntryDTO lifeCertificateEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to update life certificate for PPO ID: {PpoId} with data: {Data}",
                ppoId,
                lifeCertificateEntryDTO
            );
            JsonAPIResponse<LifeCertificateResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Life Certificate updated sucessfully!",
            };
            try
            {
                response.Result =
                    await _lifeCertificateService.UpdateLifeCertificateByPpoId<LifeCertificateResponseDTO>(
                        ppoId,
                        lifeCertificateEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating life certificate for PPO ID: {PpoId} with data: {Data}",
                    ppoId,
                    lifeCertificateEntryDTO
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

        [HttpGet("lifecertificate/branch/{branchId}")]
        [Tags("Pension: Life Certificate")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<LifeCertificateListResponseDTO>
        > GetLifeCertificatesByBranchId(long branchId)
        {
            _logger.LogInformation(
                "Received request to get life certificates for branch ID: {BranchId}",
                branchId
            );
            JsonAPIResponse<LifeCertificateListResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Life Certificates retrieved successfully!",
            };

            try
            {
                response.Result = await _lifeCertificateService.GetLifeCertificatesByBranchId(
                    branchId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching life certificates for branch ID: {BranchId}",
                    branchId
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
