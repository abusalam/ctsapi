using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    //  [Authorize("roles:clerk|permissions:can-receive-bill")]
    [Route("api/v1/ppo")]
    public class PpoStatusController : ApiBaseController
    {
        private readonly IPensionStatusService _pensionStatusService;
        private readonly ILogger<PpoStatusController> _logger;

        public PpoStatusController(
            IPensionStatusService pensionStatusService,
            IClaimService claimService,
            ILogger<PpoStatusController> logger
        )
            : base(claimService)
        {
            _pensionStatusService = pensionStatusService;
            _logger = logger;
        }

        [HttpPost("status")]
        [Tags("Pension: PPO Status")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionStatusEntryDTO>> SetPpoStatusFlag(
            PensionStatusEntryDTO pensionStatusEntryDTO
        )
        {
            JsonAPIResponse<PensionStatusEntryDTO> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Message = "PPO Status Flag Set Successfully",
                    Result =
                        await _pensionStatusService.SetPensionStatusFlag<PensionStatusEntryDTO>(
                            pensionStatusEntryDTO,
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                };

                // if(System.Enum.TryParse<PensionStatusFlag>(
                //         $"{pensionStatusEntryDTO.StatusFlag}",
                //         out PensionStatusFlag pensionStatus
                //     )
                // ) {
                //     if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionBillGenerated))
                //     {
                //         response.Message += " First Pension Generated |";
                //     }
                //     if(pensionStatus.HasFlag(PensionStatusFlag.PpoApproved))
                //     {
                //         response.Message += " PPO Approved |";
                //     }
                //     if(pensionStatus.HasFlag(PensionStatusFlag.PpoRunning))
                //     {
                //         response.Message += " PPO Running |";
                //     }
                //     if(pensionStatus.HasFlag(PensionStatusFlag.PpoSuspended))
                //     {
                //         response.Message += " PPO Suspended |";
                //     }
                // }
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

        [HttpDelete("{ppoId}/status/{statusFlag}")]
        [Tags("Pension: PPO Status")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionStatusDTO>> ClearPpoStatusFlagByPpoId(
            int ppoId,
            PensionStatusFlag statusFlag
        )
        {
            _logger.LogInformation(
                "Received request to clear PPO status flag for PPO ID: {PpoId}, Status Flag: {StatusFlag}",
                ppoId,
                statusFlag
            );
            JsonAPIResponse<PensionStatusDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "PPO Status Flag Cleared Successfully",
                Result = new() { StatusFlag = statusFlag },
            };
            try
            {
                response.Result = await _pensionStatusService.ClearPensionStatusFlag(
                    ppoId,
                    statusFlag,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );

                // if(System.Enum.TryParse<PensionStatusFlag>(
                //         $"{response.Result.StatusFlag}",
                //         out PensionStatusFlag pensionStatus
                //     )
                // )
                // {
                //     if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionBillApproved))
                //     {
                //         response.Message += " First Pension Generated Flag |";
                //     }
                //     if(pensionStatus.HasFlag(PensionStatusFlag.PpoApproved))
                //     {
                //         response.Message += " PPO Approved Flag |";
                //     }
                //     if(pensionStatus.HasFlag(PensionStatusFlag.PpoRunning))
                //     {
                //         response.Message += " PPO Running Flag |";
                //     }
                //     if(pensionStatus.HasFlag(PensionStatusFlag.PpoSuspended))
                //     {
                //         response.Message += " PPO Suspended Flag |";
                //     }
                // }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while clearing PPO status flag for PPO ID: {PpoId}, Status Flag: {StatusFlag}",
                    ppoId,
                    statusFlag
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

        [HttpGet("{ppoId}/status/{statusFlag}")]
        [Tags("Pension: PPO Status")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionStatusDTO>> GetPpoStatusFlagByPpoId(
            int ppoId,
            PensionStatusFlag statusFlag
        )
        {
            _logger.LogInformation(
                "Received request to get PPO status flag for PPO ID: {PpoId}, Status Flag: {StatusFlag}",
                ppoId,
                statusFlag
            );
            JsonAPIResponse<PensionStatusDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "PPO Status Flag Retrieved Successfully",
                Result = new() { StatusFlag = statusFlag },
            };
            try
            {
                response.Result = await _pensionStatusService.CheckPensionStatusFlag(
                    ppoId,
                    statusFlag,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );

                // if(System.Enum.TryParse<PensionStatusFlag>(
                //         $"{response.Result.StatusFlag}",
                //         out PensionStatusFlag pensionStatus
                //     )
                // ) {
                //     if(pensionStatus.HasFlag(PensionStatusFlag.FirstPensionBillGenerated))
                //     {
                //         response.Message += " First Pension Generated |";
                //     }
                //     if(pensionStatus.HasFlag(PensionStatusFlag.PpoApproved))
                //     {
                //         response.Message += " PPO Approved |";
                //     }
                //     if(pensionStatus.HasFlag(PensionStatusFlag.PpoRunning))
                //     {
                //         response.Message += " PPO Running |";
                //     }
                //     if(pensionStatus.HasFlag(PensionStatusFlag.PpoSuspended))
                //     {
                //         response.Message += " PPO Suspended |";
                //     }
                // }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while retrieving PPO status flag for PPO ID: {PpoId}, Status Flag: {StatusFlag}",
                    ppoId,
                    statusFlag
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
