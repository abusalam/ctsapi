using System.Runtime;
using System.Data.Common;
using System.Diagnostics;
using System.Text.Json.Nodes;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.PensionEnum;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
        private readonly IPensionStatusService _pensionStatusService;

        public PensionController(
                IClaimService claimService,
                IPensionService pensionService,
                IPensionStatusService pensionStatusService) : 
        base(claimService)
        {
            _pensionService = pensionService;
            _pensionStatusService = pensionStatusService;
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
                                    FilterField = "TreasuryReceiptNo",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "PPO No",
                                    DataType = "text",
                                    FieldName = "PpoNo",
                                    FilterField = "PpoNo",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Date of Receipt",
                                    DataType = "text",
                                    FieldName = "ReceiptDate",
                                    FilterField = "ReceiptDate",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Name of Pensioner",
                                    DataType = "text",
                                    FieldName = "PensionerName",
                                    FilterField = "PensionerName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionService.GetAllPpoReceipts(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = 10
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


        [HttpPost("ppo/status")]
        [Produces("application/json")]
        [Tags("Pension", "PPO Status")]
        public async Task<APIResponse<PensionStatusEntryDTO>> ControlPensionStatus(PensionStatusEntryDTO pensionStatusEntryDTO) {

            APIResponse<PensionStatusEntryDTO> result = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "|",
                result = await _pensionStatusService.SetPensionStatusFlag(
                    pensionStatusEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                )
            };

            if(System.Enum.TryParse<PensionStatusFlag>(
                    $"{pensionStatusEntryDTO.StatusFlag}",
                    out PensionStatusFlag pensionStatus
                )
            ) {
                if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionGenerated))
                {
                    result.Message += " First Pension Generated |";
                }
                if(pensionStatus.HasFlag(PensionStatusFlag.PpoApproved))
                {
                    result.Message += " PPO Approved |";
                }
                if(pensionStatus.HasFlag(PensionStatusFlag.PpoRunning))
                {
                    result.Message += " PPO Running |";
                }
                if(pensionStatus.HasFlag(PensionStatusFlag.PpoSuspended))
                {
                    result.Message += " PPO Suspended |";
                }
            }

            return await Task.FromResult(result);
        }

        [HttpDelete("ppo/{ppoId}/status/{statusFlag}")]
        [Produces("application/json")]
        [Tags("Pension", "PPO Status")]
        public async Task<APIResponse<PensionStatusDTO>> ControlPensionStatus(int ppoId, int statusFlag) {

            APIResponse<PensionStatusDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Reset: |",
                result = new(){
                    StatusFlag = 0
                }
            };
            try {
                response.result = await _pensionStatusService.ClearPensionStatusFlag(
                    ppoId,
                    statusFlag,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                if(System.Enum.TryParse<PensionStatusFlag>(
                        $"{response.result.StatusFlag}",
                        out PensionStatusFlag pensionStatus
                    )
                ) {
                    if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionGenerated))
                    {
                        response.Message += " First Pension Generated Flag |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoApproved))
                    {
                        response.Message += " PPO Approved Flag |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoRunning))
                    {
                        response.Message += " PPO Running Flag |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoSuspended))
                    {
                        response.Message += " PPO Suspended Flag |";
                    }
                }
            }
            finally {
                if(response.result.StatusFlag==0){
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"PPO status flag not found!";
                }
            }

            return response;
        }

        [HttpGet("ppo/{ppoId}/status")]
        [Produces("application/json")]
        [Tags("Pension", "PPO Status")]
        public async Task<APIResponse<PensionStatusDTO>> ControlPensionStatus(int ppoId) {
            APIResponse<PensionStatusDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "|",
                result = new(){
                    StatusFlag = 0
                }
            };
            try {
                response.result = await _pensionStatusService.CheckPensionStatusFlag(
                        ppoId,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );

                if(System.Enum.TryParse<PensionStatusFlag>(
                        $"{response.result.StatusFlag}",
                        out PensionStatusFlag pensionStatus
                    )
                ) {
                    if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionGenerated))
                    {
                        response.Message += " First Pension Generated |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoApproved))
                    {
                        response.Message += " PPO Approved |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoRunning))
                    {
                        response.Message += " PPO Running |";
                    }
                    if(pensionStatus.HasFlag(PensionStatusFlag.PpoSuspended))
                    {
                        response.Message += " PPO Suspended |";
                    }
                }
            }
            finally {
                if(response.result.StatusFlag==0){
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"PPO status flag not found!";
                }
            }
            return response;
        }
    } 
}