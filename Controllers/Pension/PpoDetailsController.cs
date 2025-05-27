using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    //[Authorize("roles:clerk|permissions:can-receive-bill")]
    public class PpoDetailsController(
        IPensionerDetailsService pensionerDetailsService,
        IClaimService claimService,
        ILogger<PpoDetailsController> logger
    ) : ApiBaseController(claimService)
    {
        private readonly IPensionerDetailsService _pensionerDetailsService =
            pensionerDetailsService;
        private readonly ILogger<PpoDetailsController> _logger = logger;

        [HttpPost("ppo/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> CreatePensioner(
            PensionerEntryDTO pensionerEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to create pensioner with data: {PensionerEntryDTO}",
                pensionerEntryDTO
            );
            JsonAPIResponse<PensionerResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Pensioner saved successfully!",
            };
            try
            {
                response.Result = await _pensionerDetailsService.CreatePensioner(
                    pensionerEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                _logger.LogInformation(
                    "Pensioner created successfully with PPO ID: {PpoId}",
                    response.Result.PpoId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating pensioner with data: {Data}",
                    pensionerEntryDTO
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

        [HttpPut("ppo/{ppoId}/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> UpdatePensionerByPpoId(
            int ppoId,
            PensionerEntryDTO pensionerEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to update pensioner with PPO ID: {PpoId} and data: {Data}",
                ppoId,
                pensionerEntryDTO
            );
            JsonAPIResponse<PensionerResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details saved sucessfully!",
            };
            try
            {
                response.Result = await _pensionerDetailsService.UpdatePensioner(
                    ppoId,
                    pensionerEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                _logger.LogInformation(
                    "Pensioner with PPO ID: {PpoId} updated successfully",
                    ppoId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating pensioner with PPO ID: {PpoId} and data: {Data}",
                    ppoId,
                    pensionerEntryDTO
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

        [HttpGet("ppo/{ppoId}/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> GetPensionerByPpoId(int ppoId)
        {
            _logger.LogInformation(
                "Received request to get pensioner details by PPO ID: {PpoId}",
                ppoId
            );
            JsonAPIResponse<PensionerResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO details received successfully!",
            };

            try
            {
                response.Result = await _pensionerDetailsService.GetPensioner<PensionerResponseDTO>(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                _logger.LogInformation(
                    "Pensioner details for PPO ID: {PpoId} retrieved successfully",
                    ppoId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching pensioner details for PPO ID: {PpoId}",
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

        [HttpGet("ppo/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>>> GetPensioners()
        {
            _logger.LogInformation("Received request to get all pensioners.");
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All PPO Details Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "PPO ID", FieldName = "ppoId" },
                        new() { Name = "Name of Pensioner", FieldName = "pensionerName" },
                        new() { Name = "Mobile", FieldName = "mobileNumber" },
                        new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                        new() { Name = "Date of Retirement", FieldName = "dateOfRetirement" },
                        new() { Name = "PPO No", FieldName = "ppoNo" },
                    ],
                    Data = await _pensionerDetailsService.GetPensioners<PensionerListItemDTO>(
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    ),
                };
                _logger.LogInformation("All pensioners retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all pensioners.");
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }
            return response;
        }

        [HttpGet("ppo/details/not-approved")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>>
        > GetAllNotApprovedPensioners()
        {
            _logger.LogInformation("Received request to get all not approved pensioners.");
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Not Approved PPO Details Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "PPO ID", FieldName = "ppoId" },
                        new() { Name = "Name of Pensioner", FieldName = "pensionerName" },
                        new() { Name = "Mobile", FieldName = "mobileNumber" },
                        new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                        new() { Name = "Date of Retirement", FieldName = "dateOfRetirement" },
                        new() { Name = "PPO No", FieldName = "ppoNo" },
                    ],
                    Data = await _pensionerDetailsService.GetAllNonApprovedPensioners(
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    ),
                };
                _logger.LogInformation("All not approved pensioners retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all not approved pensioners.");
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
        [HttpGet("ppo/{PpoId}/payment-history")]
        [Tags("Pension: Payment History")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PpoPaymentHistoryResponseDTO>>
        > GetPensionerPaymentHistoryByPpoId(int PpoId)
        {
            _logger.LogInformation(
                "Received request to get payment history for PPO ID: {PpoId}",
                PpoId
            );
            JsonAPIResponse<TableResponseDTO<PpoPaymentHistoryResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Payment History Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "Component ID", FieldName = "componentId" },
                        new() { Name = "Component Name", FieldName = "componentName" },
                        new() { Name = "Breakup Amount", FieldName = "breakupAmount" },
                        new() { Name = "From Date", FieldName = "fromDate" },
                        new() { Name = "To Date", FieldName = "toDate" },
                    ],
                    Data = await _pensionerDetailsService.GetPensionerPaymentHistoryByPpoId(
                        PpoId,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    ),
                };
                _logger.LogInformation(
                    "Payment history for PPO ID: {PpoId} retrieved successfully",
                    PpoId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching payment history for PPO ID: {PpoId}",
                    PpoId
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
