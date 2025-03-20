using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class EPpoReceiptController : ApiBaseController
    {
        private readonly IEPpoReceiptService _ppoReceiptService;

        public EPpoReceiptController(
            IEPpoReceiptService ppoReceiptService,
            IClaimService claimService
        )
            : base(claimService)
        {
            _ppoReceiptService = ppoReceiptService;
        }

        [HttpPost("e-ppo/receipt")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptResponseDTO>> CreateEPpoReceipt(
            EPpoReceiptEntryDTO ePpoReceiptEntryDTO
        )
        {
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

        [HttpGet("e-ppo/receipt/{receiptId}")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<EPpoReceiptDetailDTO>> GetEPpoReceiptById(long receiptId)
        {
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

        [HttpGet("e-ppo/receipt/unused")]
        [Tags("Pension: e-PPO Receipt")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<EPpoReceiptDetailDTO>>
        > GetUnusedEPpoReceipts()
        {
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
