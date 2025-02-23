using System.Data.Common;
using System.Net.Mime;
using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.BAL.Services;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v1")]
    public class PpoByTransferController : ApiBaseController
    {
        private readonly IClaimService _claimService;
        private readonly IMqService _mqService;
        private readonly IPpoByTransferService _ppobyTransferHeadService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public PpoByTransferController(
            IClaimService claimService,
            IMqService mqService,
            IPpoByTransferService ppobyTransferHeadService
        )
            : base(claimService)
        {
            _claimService = claimService;
            _mqService = mqService;
            _ppobyTransferHeadService = ppobyTransferHeadService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [HttpPost("save-ppo-by-transfer-headmap")]
        [Tags("Pension: PPO By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoByTransferHeadResponseDTO>> CreatePPoByTransferHeadMap(
            PpoByTransferEntryDTO ppobyTransferHeadEntryDTO
        )
        {
            JsonAPIResponse<PpoByTransferHeadResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "Ppo ByTransfer Head saved successfully!",
                Result = new() { DataSource = null },
            };

            try
            {
                response.Result =
                    await _ppobyTransferHeadService.SavePpoByTransferHead<PpoByTransferHeadResponseDTO>(
                        ppobyTransferHeadEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );

                if (response.Result.DataSource != null)
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }

        [HttpPut("update-ppo-by-transfer-headmap")]
        [Tags("Pension: PPO By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoByTransferHeadResponseDTO>> UpdatePPOByTransferHeadMap(
            PpoByTransferUpdateDTO ppoByTransferUpdateDTO
        )
        {
            JsonAPIResponse<PpoByTransferHeadResponseDTO> response = new();

            try
            {
                var result =
                    await _ppobyTransferHeadService.UpdatePPOByTransfer<PpoByTransferHeadResponseDTO>(
                        ppoByTransferUpdateDTO
                    );

                if (!string.IsNullOrEmpty((string?)result.Message))
                {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = (string?)result.Message;
                    return response;
                }

                response.Result = result;
                response.ApiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = "PPO By Transfer updated successfully!";
            }
            catch (Exception ex)
            {
                FillException(response, ex);
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }

        [HttpDelete("delete-ppo-by-transfer/{ppobytransferid}")]
        [Tags("Pension: PPO By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoByTransferHeadResponseDTO>> DeletePPOByTransfer(
            long ppobytransferid
        )
        {
            JsonAPIResponse<PpoByTransferHeadResponseDTO> response = new();

            try
            {
                var result =
                    await _ppobyTransferHeadService.DeletePPOByTransfer<PpoByTransferHeadResponseDTO>(
                        ppobytransferid
                    );

                // Check if an error message exists in the result
                if (!string.IsNullOrEmpty((string?)result.Message))
                {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = (string?)result.Message;
                    return response;
                }

                response.Result = result;
                response.ApiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = "PPO By Transfer deleted successfully!";
            }
            catch (Exception ex)
            {
                FillException(response, ex);
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }

        [HttpGet("{ppoid}/by-transfer")]
        [Tags("Pension: PPO By Transfer")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PpoByTransferHeadResponseList>>
        > GetPpoByTransferByPpoId(int ppoid)
        {
            JsonAPIResponse<TableResponseDTO<PpoByTransferHeadResponseList>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "PPO By Transfer records received successfully!",
            };

            try
            {
                var result =
                    await _ppobyTransferHeadService.GetAllPpoByTransferById<PpoByTransferHeadResponseDTO>(
                        ppoid,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );

                if (result?.Result == null || !result.Result.Any())
                {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "No PPO By Transfer records found for the given PPO Id.";
                }
                else
                {
                    response.Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "ID", FieldName = "id" },
                            new() { Name = "PPO No", FieldName = "ppoNo" },
                            new() { Name = "Pensioner Name", FieldName = "pensionerName" },
                            new() { Name = "From Date", FieldName = "fromDate" },
                            new() { Name = "To Date", FieldName = "toDate" },
                            new() { Name = "By Transfer Head ID", FieldName = "bytransferHeadId" },
                            new() { Name = "By Transfer Amount", FieldName = "bytransferAmount" },
                            new() { Name = "Remarks", FieldName = "remarks" },
                            new()
                            {
                                Name = "PPO By Transfer Head Count",
                                FieldName = "PPOByTransferHeadCount",
                            },
                        },
                        Data = result.Result,
                    };
                }
            }
            catch (Exception ex)
            {
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }
    }
}
