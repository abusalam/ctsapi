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
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IPpoReceiptService _ppoReceiptService = ppoReceiptService;

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

        [HttpGet("manual-ppo-receipt/trid/{treasuryReceiptNo}")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<ManualPpoReceiptResponseDTO>
        > GetPpoReceiptByTreasuryReceiptNo(string treasuryReceiptNo)
        {
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new();

            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _ppoReceiptService.GetPpoReceipt(treasuryReceiptNo),
                    Message = $"PPO Receipt Received Successfully!",
                };
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

        [HttpGet("manual-ppo-receipt/{receiptId}")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<ManualPpoReceiptResponseDTO>> GetPpoReceiptById(
            long receiptId
        )
        {
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new();

            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _ppoReceiptService.GetPpoReceipt(receiptId),
                    Message = $"PPO Receipt Received Successfully!",
                };
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

        [HttpGet("manual-ppo-receipts")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ListAllPpoReceiptsResponseDTO>>
        > GetPpoReceipts()
        {
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

        [HttpPut("manual-ppo-receipt/{receiptId}")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<ManualPpoReceiptResponseDTO>> UpdatePpoReceipt(
            long receiptId,
            ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO
        )
        {
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

        [HttpGet("manual-ppo-receipts/unused")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ManualPpoReceiptResponseDTO>>
        > GetAllUnusedPpoReceipts()
        {
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
