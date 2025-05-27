using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    // [Authorize("roles:clerk|permissions:can-receive-bill")]
    public class PpoReceiptController(
        IPpoReceiptService ppoReceiptService,
        IClaimService claimService,
        ILogger<PpoReceiptController> logger
    ) : ApiBaseController(claimService)
    {
        private readonly IPpoReceiptService _ppoReceiptService = ppoReceiptService;
        private readonly ILogger<PpoReceiptController> _logger = logger;

        [Authorize(
            "roles:Treasury Officer,Admin|permissions:can-bill-check,can-return-memo-generate"
        )]
        [HttpPost("manual-ppo-receipt")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<ManualPpoReceiptResponseDTO>> CreatePpoReceipt(
            ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to create PPO Receipt with data: {Data}",
                manualPpoReceiptEntryDTO
            );
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new();

            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _ppoReceiptService.CreatePpoReceipt(
                        manualPpoReceiptEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    ),
                    Message = $"PPO Receipt Created Successfully!",
                };
                _logger.LogInformation(
                    "PPO Receipt created successfully with Treasury Receipt No: {TreasuryReceiptNo}",
                    response.Result.TreasuryReceiptNo
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating PPO Receipt with data: {Data}",
                    manualPpoReceiptEntryDTO
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

        [HttpGet("manual-ppo-receipt/trid/{treasuryReceiptNo}")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<ManualPpoReceiptResponseDTO>
        > GetPpoReceiptByTreasuryReceiptNo(string treasuryReceiptNo)
        {
            _logger.LogInformation(
                "Received request to get PPO Receipt by Treasury Receipt No: {TreasuryReceiptNo}",
                treasuryReceiptNo
            );
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new();

            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _ppoReceiptService.GetPpoReceipt(treasuryReceiptNo),
                    Message = $"PPO Receipt Received Successfully!",
                };
                _logger.LogInformation(
                    "PPO Receipt received successfully for Treasury Receipt No: {TreasuryReceiptNo}",
                    treasuryReceiptNo
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPO Receipt for Treasury Receipt No: {TreasuryReceiptNo}",
                    treasuryReceiptNo
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

        [HttpGet("manual-ppo-receipt/{receiptId}")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<ManualPpoReceiptResponseDTO>> GetPpoReceiptById(
            long receiptId
        )
        {
            _logger.LogInformation(
                "Received request to get PPO Receipt by ID: {ReceiptId}",
                receiptId
            );
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new();

            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _ppoReceiptService.GetPpoReceipt(receiptId),
                    Message = $"PPO Receipt Received Successfully!",
                };
                _logger.LogInformation(
                    "PPO Receipt received successfully for ID: {ReceiptId}",
                    receiptId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPO Receipt for ID: {ReceiptId}",
                    receiptId
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

        [HttpGet("manual-ppo-receipts")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ListAllPpoReceiptsResponseDTO>>
        > GetPpoReceipts()
        {
            _logger.LogInformation("Received request to get all PPO Receipts.");
            JsonAPIResponse<TableResponseDTO<ListAllPpoReceiptsResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers =
                        [
                            new() { Name = "Treasury Receipt No", FieldName = "treasuryReceiptNo" },
                            new() { Name = "PPO No", FieldName = "ppoNo" },
                            new() { Name = "Name of Pensioner", FieldName = "pensionerName" },
                            new() { Name = "Date of Receipt", FieldName = "receiptDate" },
                        ],
                        Data =
                            await _ppoReceiptService.GetPpoReceipts<ListAllPpoReceiptsResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode()
                            ),
                    },
                    Message = $"All PPO Receipts Received Successfully!",
                };
                _logger.LogInformation("All PPO Receipts received successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all PPO Receipts.");
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
        [HttpPut("manual-ppo-receipt/trid/{treasuryReceiptNo}")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<ManualPpoReceiptResponseDTO>
        > UpdatePpoReceiptByTreasuryReceiptNo(
            string treasuryReceiptNo,
            ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to update PPO Receipt with Treasury Receipt No: {TreasuryReceiptNo} and data: {Data}",
                treasuryReceiptNo,
                manualPpoReceiptEntryDTO
            );
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Receipt Updated Successfully!",
            };

            try
            {
                response.Result = await _ppoReceiptService.UpdatePpoReceipt(
                    treasuryReceiptNo,
                    manualPpoReceiptEntryDTO
                );
                _logger.LogInformation(
                    "PPO Receipt updated successfully for Treasury Receipt No: {TreasuryReceiptNo}",
                    treasuryReceiptNo
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating PPO Receipt for Treasury Receipt No: {TreasuryReceiptNo} with data: {Data}",
                    treasuryReceiptNo,
                    manualPpoReceiptEntryDTO
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

        [HttpPut("manual-ppo-receipt/{receiptId}")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<ManualPpoReceiptResponseDTO>> UpdatePpoReceipt(
            long receiptId,
            ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to update PPO Receipt with ID: {ReceiptId} and data: {Data}",
                receiptId,
                manualPpoReceiptEntryDTO
            );
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new();

            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _ppoReceiptService.UpdatePpoReceipt(
                        receiptId,
                        manualPpoReceiptEntryDTO
                    ),
                    Message = $"PPO Receipt Updated Successfully!",
                };
                _logger.LogInformation(
                    "PPO Receipt updated successfully for ID: {ReceiptId}",
                    receiptId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating PPO Receipt for ID: {ReceiptId} with data: {Data}",
                    receiptId,
                    manualPpoReceiptEntryDTO
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

        [HttpGet("manual-ppo-receipts/unused")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ManualPpoReceiptResponseDTO>>
        > GetAllUnusedPpoReceipts()
        {
            _logger.LogInformation("Received request to get all unused PPO Receipts.");
            JsonAPIResponse<TableResponseDTO<ManualPpoReceiptResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers =
                        [
                            new() { Name = "Treasury Receipt No", FieldName = "treasuryReceiptNo" },
                            new() { Name = "PPO No", FieldName = "ppoNo" },
                            new() { Name = "Name of Pensioner", FieldName = "pensionerName" },
                            new() { Name = "Mobile Number", FieldName = "mobileNumber" },
                            new() { Name = "Date of Receipt", FieldName = "receiptDate" },
                            new()
                            {
                                Name = "Date of Commencement",
                                FieldName = "dateOfCommencement",
                            },
                        ],
                        Data =
                            await _ppoReceiptService.GetAllUnusedPpoReceipts<ManualPpoReceiptResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode()
                            ),
                    },
                    Message = $"All Unused PPO Receipts Received Successfully!",
                };
                _logger.LogInformation("All unused PPO Receipts received successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all unused PPO Receipts.");
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
