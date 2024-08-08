using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class PpoStatusController : ApiBaseController
    {
        private readonly IPensionStatusService _pensionStatusService;
        public PpoStatusController(
                IPensionStatusService pensionStatusService,
                IClaimService claimService
            ) : base(claimService)
        {
            _pensionStatusService = pensionStatusService;
        }

        [HttpPost("status")]
        [Tags("Pension: PPO Status")]
        [OpenApi]
        public async Task<APIResponse<PensionStatusEntryDTO>> PpoStatusFlagCreate(PensionStatusEntryDTO pensionStatusEntryDTO) {

            APIResponse<PensionStatusEntryDTO> response = new(){
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
                if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionBillGenerated))
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

            return response;
        }

        [HttpDelete("{ppoId}/status/{statusFlag}")]
        [Tags("Pension: PPO Status")]
        [OpenApi]
        public async Task<APIResponse<PensionStatusDTO>> PpoStatusFlagDelete(int ppoId, int statusFlag) {

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
                    if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionBillGenerated))
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

        [HttpGet("{ppoId}/status/{statusFlag}")]
        [Tags("Pension: PPO Status")]
        [OpenApi]
        public async Task<APIResponse<PensionStatusDTO>> PpoStatusFlagRead(int ppoId, int statusFlag) {
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
                        statusFlag,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );

                if(System.Enum.TryParse<PensionStatusFlag>(
                        $"{response.result.StatusFlag}",
                        out PensionStatusFlag pensionStatus
                    )
                ) {
                    if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionBillGenerated))
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