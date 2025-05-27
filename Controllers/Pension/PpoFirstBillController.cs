using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    // [Authorize("roles:clerk|permissions:can-receive-bill")]
    public class PpoFirstBillController(
        IPpoFirstBillService ppoFirstBillService,
        IClaimService claimService,
        ILogger<PpoFirstBillController> logger
    ) : ApiBaseController(claimService)
    {
        private readonly IPpoFirstBillService _ppoFirstBillService = ppoFirstBillService;
        private readonly ILogger<PpoFirstBillController> _logger = logger;

        [HttpGet("first-bill/ppos")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>>
        > GetAllPposForFirstBill()
        {
            _logger.LogInformation("Received request to get all PPOs for first bill generation.");
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO List for first bill received sucessfully!",
            };
            try
            {
                var ppoList =
                    await _ppoFirstBillService.GetPposForFirstBillGeneration<PpoListResponseDTO>(
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "PPO ID", FieldName = "ppoId" },
                        new() { Name = "PPO Number", FieldName = "ppoNo" },
                        new() { Name = "Pensioner Name", FieldName = "pensionerName" },
                        new() { Name = "Mobile", FieldName = "mobileNumber" },
                        new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                        new() { Name = "Date of Commencement", FieldName = "dateOfCommencement" },
                        new() { Name = "Date of Retirement", FieldName = "dateOfRetirement" },
                        new() { Name = "Date of Commencement", FieldName = "dateOfCommencement" },
                    ],
                    Data = ppoList.PpoList,
                };
                _logger.LogInformation(
                    "PPO List for first bill received successfully with {Count} records.",
                    ppoList.PpoList.Count
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPOs for first bill generation."
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

        [Authorize("roles:Accountant,Admin|permissions:can-bill-check")]
        [HttpGet("first-bill-print/ppos")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>>
        > GetPposForFirstBillPrint()
        {
            _logger.LogInformation("Received request to get PPOs for first bill print.");
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO List for first bill received sucessfully!",
            };
            try
            {
                var ppoList =
                    await _ppoFirstBillService.GetPposForFirstBillPrint<PpoListResponseDTO>(
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "PPO ID", FieldName = "ppoId" },
                        new() { Name = "PPO Number", FieldName = "ppoNo" },
                        new() { Name = "Pensioner Name", FieldName = "pensionerName" },
                        new() { Name = "Mobile", FieldName = "mobileNumber" },
                        new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                        new() { Name = "Date of Commencement", FieldName = "dateOfCommencement" },
                        new() { Name = "Date of Retirement", FieldName = "dateOfRetirement" },
                        new() { Name = "Date of Commencement", FieldName = "dateOfCommencement" },
                    ],
                    Data = ppoList.PpoList,
                };
                _logger.LogInformation(
                    "PPO List for first bill print received successfully with {Count} records.",
                    ppoList.PpoList.Count
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching PPOs for first bill print.");
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [Authorize(
            "roles:Treasury Officer,Admin|permissions:can-bill-check,can-return-memo-generate"
        )]
        [HttpPost("first-bill-generate")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<InitiateFirstPensionBillResponseDTO>
        > GenerateFirstPensionBill(InitiateFirstPensionBillEntryDTO initiateFirstPensionBillDTO)
        {
            _logger.LogInformation(
                "Received request to generate first pension bill with data: {InitiateFirstPensionBillDTO}",
                initiateFirstPensionBillDTO
            );
            JsonAPIResponse<InitiateFirstPensionBillResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill generated sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoFirstBillService.GenerateFirstPensionBill<InitiateFirstPensionBillResponseDTO>(
                        initiateFirstPensionBillDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while generating first pension bill with data: {Data}",
                    initiateFirstPensionBillDTO
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

        [Authorize(
            "roles:Treasury Officer,Admin|permissions:can-bill-check,can-return-memo-generate"
        )]
        [HttpPost("first-bill")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoBillSaveResponseDTO>> SaveFirstPensionBill(
            InitiateFirstPensionBillEntryDTO initiateFirstPensionBillEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to save first pension bill with data: {InitiateFirstPensionBillEntryDTO}",
                initiateFirstPensionBillEntryDTO
            );
            JsonAPIResponse<PpoBillSaveResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoFirstBillService.SaveFirstPensionBill<PpoBillSaveResponseDTO>(
                        initiateFirstPensionBillEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while saving first pension bill with data: {Data}",
                    initiateFirstPensionBillEntryDTO
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

        [Authorize("roles:Accountant,Admin|permissions:can-bill-check")]
        [HttpGet("first-bill/{ppoId}")]
        [Tags("Pension: First Bill")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoBillResponseDTO>> GetFirstPensionBillByPpoId(int ppoId)
        {
            _logger.LogInformation(
                "Received request to get first pension bill by PPO ID: {PpoId}",
                ppoId
            );
            JsonAPIResponse<PpoBillResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"First Pension Bill retrieved sucessfully!",
            };
            try
            {
                response.Result = await _ppoFirstBillService.GetFirstBillByPpoId(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                _logger.LogInformation(
                    "First pension bill for PPO ID {PpoId} retrieved successfully.",
                    ppoId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching first pension bill for PPO ID: {PpoId}",
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
    }
}
