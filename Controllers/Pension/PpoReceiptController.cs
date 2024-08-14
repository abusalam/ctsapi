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
        public async Task<APIResponse<ManualPpoReceiptResponseDTO>> CreatePpoReceipt(
            ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO
        )
        {
            APIResponse<ManualPpoReceiptResponseDTO> response;

            try {
                response = new() {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                result = await _ppoReceiptService
                        .CreatePpoReceipt(
                            manualPpoReceiptEntryDTO,
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                            ),
                Message = $"PPO Received Successfully!"
                };
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new StackFrame(1, true);
                response = new () {
                apiResponseStatus = Enum.APIResponseStatus.Error,
                result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
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
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response;

            try {
                response = new() {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Result = await _ppoReceiptService
                        .GetPpoReceipt(treasuryReceiptNo),
                Message = $"PPO Received Successfully!"

                };
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                    ApiResponseStatus = Enum.APIResponseStatus.Error,
                    Result = null,
                    Message = e.ToString()
                    //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
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
            JsonAPIResponse<DynamicListResult<IEnumerable<ListAllPpoReceiptsResponseDTO>>> response;
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
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
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
            JsonAPIResponse<ManualPpoReceiptResponseDTO> response;

            try {
                response = new() {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = await _ppoReceiptService
                            .UpdatePpoReceipt(treasuryReceiptNo, manualPpoReceiptEntryDTO),
                    Message = $"PPO Receipt Updated Successfully!"
                };
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new StackFrame(1, true);
                response = new () {
                    ApiResponseStatus = Enum.APIResponseStatus.Error,
                    Result = null,
                    Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }

    }
}