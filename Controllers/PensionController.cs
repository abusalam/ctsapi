using System.Data.Common;
using System.Diagnostics;
using System.Text.Json.Nodes;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.DTOs.PensionDTO;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace CTS_BE.Controllers
{
    [ApiController]
    [Tags("Pension")]
    [Route("api/v1")]
    public class PensionController : ApiBaseController
    {
        private readonly IPensionService _pensionService;

        public PensionController(IClaimService claimService, IPensionService pensionService) : 
        base(claimService)
        {
            _pensionService = pensionService;
        }

        [HttpPost("echo")]
        [Produces("application/json")]
        [Tags("Pension", "Echo Request in Response")]
        public async Task<APIResponse<Object>> ControlEchoRequestInResponse(Object req)
        {
            APIResponse<Object> response = new()
            {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Echoing Request",
                result = req
            };
            return await Task.FromResult(response);
        }

        [HttpPost("date-only")]
        [Produces("application/json")]
        [Tags("Pension", "DateOnly Echo")]
        public async Task<APIResponse<DateOnlyDTO>> ControlSetDateOnly(DateOnlyDTO dateOnly)
        {
            APIResponse<DateOnlyDTO> response = new()
            {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Writing DateOnly",
                result = dateOnly
            };
            return await Task.FromResult(response);
        }

        [HttpGet("date-only")]
        [Produces("application/json")]
        [Tags("Pension", "DateOnly Echo")]
        public async Task<APIResponse<DateOnly>> ControlGetDateOnly()
        {
            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
            APIResponse<DateOnly> response = new()
            {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Reading DateOnly",
                result = dateOnly
            };
            return await Task.FromResult(response);
        }
    

        [HttpPost("manual-ppo/receipts")]
        [Produces("application/json")]
        [Tags("Pension", "Manual PPO Receipt")]
        public async Task<APIResponse<ManualPpoReceiptResponseDTO>> ControlCreateManualPpoReceipt(ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO)
        {
            APIResponse<ManualPpoReceiptResponseDTO> response;

            try {
                response = new() {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                result = await _pensionService
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

        [HttpGet("manual-ppo/receipts/{treasuryReceiptNo}")]
        [Produces("application/json")]
        [Tags("Pension", "Manual PPO Receipt")]
        public async Task<APIResponse<ManualPpoReceiptResponseDTO>> ControlGetPpoReceipt(string treasuryReceiptNo)
        {
            APIResponse<ManualPpoReceiptResponseDTO> response;

            try {
                response = new() {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                result = await _pensionService
                        .GetPpoReceipt(treasuryReceiptNo),
                Message = $"PPO Received Successfully!"

                };
            } catch(DbException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                    apiResponseStatus = Enum.APIResponseStatus.Error,
                    result = null,
                    Message = e.ToString()
                    //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }


        [HttpPatch("manual-ppo/receipts")]
        [Produces("application/json")]
        [Tags("Pension", "Manual PPO Receipt")]
        public async Task<APIResponse<DynamicListResult<IEnumerable<ListAllPpoReceiptsResponseDTO>>>> ControlGetAllManualPpoReceipts(DynamicListQueryParameters dynamicListQueryParameters)
        {

            APIResponse<DynamicListResult<IEnumerable<ListAllPpoReceiptsResponseDTO>>> response;
            try {

                response = new() {

                    apiResponseStatus = Enum.APIResponseStatus.Success,
                    result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Treasury Receipt No",
                                    DataType = "text",
                                    FieldName = "TreasuryReceiptNo",
                                    FilterField = "treasury_receipt_no",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "PPO No",
                                    DataType = "text",
                                    FieldName = "PpoNo",
                                    FilterField = "ppo_no",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Date of Receipt",
                                    DataType = "text",
                                    FieldName = "ReceiptDate",
                                    FilterField = "receipt_date",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Name of Pensioner",
                                    DataType = "text",
                                    FieldName = "PensionerName",
                                    FilterField = "pensioner_name",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionService
                                    .GetPpoReceipts(dynamicListQueryParameters),
                            DataCount = 1
                        },
                    Message = $"All PPO Receipts Received Successfully!"

                };
            } catch(DbException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                apiResponseStatus = Enum.APIResponseStatus.Error,
                result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }


        [HttpPut("manual-ppo/receipts/{treasuryReceiptNo}")]
        [Produces("application/json")]
        [Tags("Pension", "Manual PPO Receipt")]
        public async Task<APIResponse<ManualPpoReceiptResponseDTO>> ControlUpdateManualPpoReceipt(string treasuryReceiptNo, ManualPpoReceiptEntryDTO manualPpoReceiptEntryDTO)
        {
            APIResponse<ManualPpoReceiptResponseDTO> response;

            try {
                response = new() {
                apiResponseStatus = Enum.APIResponseStatus.Success,
                result = await _pensionService
                        .UpdatePpoReceipt(treasuryReceiptNo, manualPpoReceiptEntryDTO),
                Message = $"PPO Receipt Updated Successfully!"
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

    } 
}