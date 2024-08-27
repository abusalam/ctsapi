using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/manual-ppo")]
    public class PpoReceiptController : ApiBaseController
    {
        private readonly IPpoReceiptService _ppoReceiptService;
        public PpoReceiptController(
                IPpoReceiptService ppoReceiptService,
                IClaimService claimService
            ) : base(claimService)
        {
            _ppoReceiptService = ppoReceiptService;
        }

        [HttpPost("receipts")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<ManualPpoReceiptResponseDTO>> CreatePpoReceipt(
            ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO
        )
        {
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new();

            try {
                response = new() {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Result = await _ppoReceiptService
                        .CreatePpoReceipt(
                            manualPpoReceiptEntryDTO,
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                            ),
                Message = $"PPO Received Successfully!"
                };
            }
            catch(Exception ex) {
                FillException(response, ex);
                return response;
            }
            finally {
                FillErrorMesageFromDataSource(response);
            }
            return response;
        }

        [HttpGet("receipts/{treasuryReceiptNo}")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<ManualPpoReceiptResponseDTO>> GetPpoReceiptByTreasuryReceiptNo(
            string treasuryReceiptNo
        )
        {
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new();

            try {
                response = new() {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Result = await _ppoReceiptService
                        .GetPpoReceipt(treasuryReceiptNo),
                Message = $"PPO Received Successfully!"

                };
            }
            catch(Exception ex) {
                FillException(response, ex);
                return response;
            }
            finally {
                FillErrorMesageFromDataSource(response);
            }
            return response;
        }

        [HttpPatch("receipts")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<ListAllPpoReceiptsResponseDTO>>>> GetAllPpoReceipts(
            DynamicListQueryParameters dynamicListQueryParameters
        ) 
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<ListAllPpoReceiptsResponseDTO>>> response = new();
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Treasury Receipt No",
                                    DataType = "text",
                                    FieldName = "treasuryReceiptNo",
                                    FilterField = "treasuryReceiptNo",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "PPO No",
                                    DataType = "text",
                                    FieldName = "ppoNo",
                                    FilterField = "ppoNo",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Name of Pensioner",
                                    DataType = "text",
                                    FieldName = "pensionerName",
                                    FilterField = "pensionerName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Date of Receipt",
                                    DataType = "text",
                                    FieldName = "receiptDate",
                                    FilterField = "receiptDate",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _ppoReceiptService.GetAllPpoReceipts(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _ppoReceiptService.DataCount()
                        },
                    Message = $"All PPO Receipts Received Successfully!"

                };
            }
            catch(Exception ex) {
                FillException(response, ex);
                return response;
            }
            finally {
                FillErrorMesageFromDataSource(response);
            }
            return response;
        }

        [HttpPut("receipts/{treasuryReceiptNo}")]
        [Tags("Pension: Manual PPO Receipt")]
        [OpenApi]
        public async Task<JsonAPIResponse<ManualPpoReceiptResponseDTO>> UpdatePpoReceiptByTreasuryReceiptNo(
            string treasuryReceiptNo,
            ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO
        )
        {
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response = new();

            try {
                response = new() {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _ppoReceiptService
                            .UpdatePpoReceipt(treasuryReceiptNo, manualPpoReceiptEntryDTO),
                    Message = $"PPO Receipt Updated Successfully!"
                };
            }
            catch(Exception ex) {
                FillException(response, ex);
                return response;
            }
            finally {
                FillErrorMesageFromDataSource(response);
            }
            return response;
        }

    }
}