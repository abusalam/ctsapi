using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class EPpoReceiptController : ApiBaseController
    {
        private readonly IEPpoReceiptService _ppoReceiptService;
        private readonly ILogger<EPpoReceiptController> _logger;

        public EPpoReceiptController(
            IEPpoReceiptService ppoReceiptService,
            IClaimService claimService,
            ILogger<EPpoReceiptController> logger
        )
            : base(claimService)
        {
            _ppoReceiptService = ppoReceiptService;
            _logger = logger;
        }

        [HttpPost("e-ppo/receipt")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptResponseDTO>> CreateEPpoReceipt(
            EPpoReceiptEntryDTO ePpoReceiptEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to create ePPO Receipt with data: {Data}",
                ePpoReceiptEntryDTO
            );
            JsonAPIResponse<EPpoReceiptResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Receipt saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoReceiptService.CreateEPpoReceipt<EPpoReceiptResponseDTO>(
                        ePpoReceiptEntryDTO,
                        GetTreasuryCode(),
                        GetCurrentFyYear()
                    );
                _logger.LogInformation(
                    "ePPO Receipt created successfully with PPO ID: {PpoId}",
                    response.Result
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating ePPO Receipt with data: {Data}",
                    ePpoReceiptEntryDTO
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

        [HttpGet("e-ppo/receipt/{receiptId}")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptDetailDTO>> GetEPpoReceiptById(long receiptId)
        {
            _logger.LogInformation(
                "Received request to get ePPO Receipt by ID: {ReceiptId}",
                receiptId
            );
            JsonAPIResponse<EPpoReceiptDetailDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Receipt received sucessfully!",
            };
            try
            {
                response.Result = await _ppoReceiptService.GetEPpoReceiptById<EPpoReceiptDetailDTO>(
                    receiptId,
                    GetTreasuryCode(),
                    GetCurrentFyYear()
                );
                _logger.LogInformation(
                    "ePPO Receipt details retrieved successfully for ID: {ReceiptId}",
                    receiptId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching ePPO Receipt details for ID: {ReceiptId}",
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

        [HttpGet("e-ppo/receipt/unused")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<EPpoReceiptDetailDTO>>
        > GetUnusedEPpoReceipts()
        {
            _logger.LogInformation("Received request to get unused ePPO Receipts.");
            JsonAPIResponse<TableResponseDTO<EPpoReceiptDetailDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "PPO No", FieldName = "ppoNo" },
                            new() { Name = "Pension Application No", FieldName = "pensionApplnNo" },
                            new() { Name = "Name of Pensioner", FieldName = "pensionerName" },
                            new() { Name = "Mobile Number", FieldName = "mobileNumber" },
                        },
                        Data = await _ppoReceiptService.GetUnusedEPpoReceipts<EPpoReceiptDetailDTO>(
                            GetTreasuryCode(),
                            GetCurrentFyYear()
                        ),
                    },
                    Message = $"ePPO Receipt received sucessfully!",
                };
                _logger.LogInformation("Unused ePPO Receipts retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching unused ePPO Receipts.");
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }
            return response;
        }

        [HttpPost("e-ppo/revision")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptRevisionResponseDTO>> CreateEPpoRevision(
            EPpoReceiptRevisionEntryDTO ePpoReceiptRevisionEntryDTO
        )
        {
            JsonAPIResponse<EPpoReceiptRevisionResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Revision saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoReceiptService.CreateEPpoReceiptRevision<EPpoReceiptRevisionResponseDTO>(
                        ePpoReceiptRevisionEntryDTO,
                        GetTreasuryCode(),
                        GetCurrentFyYear()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating ePPO Receipt revision with data: {Data}",
                    ePpoReceiptRevisionEntryDTO
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

        [HttpGet("e-ppo/{applicationNo}")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptPpoIdResponseDTO>> GetPpoIdByPensionApplnNo(
            string applicationNo
        )
        {
            _logger.LogInformation(
                "Received request to get ePPO Receipt by Pension Application No: {ApplicationNo}",
                applicationNo
            );
            JsonAPIResponse<EPpoReceiptPpoIdResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Receipt Details received sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoReceiptService.GetEPpoReceiptByPensionApplnNo<EPpoReceiptPpoIdResponseDTO>(
                        applicationNo,
                        GetTreasuryCode(),
                        GetCurrentFyYear()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching ePPO Receipt details for Pension Application No: {ApplicationNo}",
                    applicationNo
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

        [HttpPut("e-ppo/withdraw")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptWithdrawlResponseDTO>> WithdrawEPpoReceipt(
            EPpoReceiptWithdrawlEntryDTO ePpoReceiptWithdrawlEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to withdraw ePPO Receipt for Pension Application No: {PensionApplnNo}",
                ePpoReceiptWithdrawlEntryDTO.PensionApplnNo
            );
            JsonAPIResponse<EPpoReceiptWithdrawlResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"ePPO Receipt withdrawn sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoReceiptService.RegisterEPpoReceiptWithdrawal<EPpoReceiptWithdrawlResponseDTO>(
                        ePpoReceiptWithdrawlEntryDTO.PensionApplnNo,
                        ePpoReceiptWithdrawlEntryDTO,
                        GetTreasuryCode(),
                        GetCurrentFyYear()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while withdrawing ePPO Receipt for Pension Application No: {PensionApplnNo}",
                    ePpoReceiptWithdrawlEntryDTO.PensionApplnNo
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
