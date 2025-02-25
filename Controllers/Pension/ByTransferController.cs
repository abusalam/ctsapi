using System.Data.Common;
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
    public class ByTransferController : ApiBaseController
    {
        private readonly IClaimService _claimService;
        private readonly IMqService _mqService;
        private readonly IByTransferService _byTransferHeadService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ByTransferController(
            IClaimService claimService,
            IMqService mqService,
            IByTransferService byTransferHeadService
        )
            : base(claimService)
        {
            _claimService = claimService;
            _mqService = mqService;
            _byTransferHeadService = byTransferHeadService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [HttpPost("by-transfer-headmap")]
        [Tags("Pension: By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> CreateByTransferHeadMap(
            ByTransferHeadEntryDTO byTransferHeadEntryDTO
        )
        {
            JsonAPIResponse<ByTransferHeadResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "ByTransfer Head saved successfully!",
            };

            try
            {
                response.Result =
                    await _byTransferHeadService.SaveByTransferHead<ByTransferHeadResponseDTO>(
                        byTransferHeadEntryDTO
                    );
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

        [HttpPut("update-by-transfer-headmap")]
        [Tags("Pension: By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> UpdateByTransferHeadMap(
            ByTransferHeadUpdateDTO byTransferHeadUpdateDTO
        )
        {
            JsonAPIResponse<ByTransferHeadResponseDTO> response = new();

            try
            {
                var result =
                    await _byTransferHeadService.UpdateByTransferHead<ByTransferHeadResponseDTO>(
                        byTransferHeadUpdateDTO
                    );

                if (!string.IsNullOrEmpty((string?)result.Message))
                {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = (string?)result.Message;
                    return response;
                }

                response.Result = result;
                response.ApiResponseStatus = Enum.APIResponseStatus.Success;
                response.Message = "ByTransfer Head updated successfully!";
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

        [HttpDelete("delete-by-transfer-headmap/{bytransferheadid}")]
        [Tags("Pension: By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> DeleteByTransferHead(
            long bytransferheadid
        )
        {
            JsonAPIResponse<ByTransferHeadResponseDTO> response = new();

            try
            {
                var result =
                    await _byTransferHeadService.DeleteByTransferHead<ByTransferHeadResponseDTO>(
                        bytransferheadid
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
                response.Message = "By Transfer Head deleted successfully!";
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

        [HttpGet("by-transfer-headmap/{bytransferheadid}")]
        [Tags("Pension: By Transfer")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> GetByTransferHeadById(
            long bytransferheadid
        )
        {
            JsonAPIResponse<ByTransferHeadResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "By Transfer Head details received successfully!",
            };

            try
            {
                response.Result =
                    await _byTransferHeadService.GetByTransferHeadById<ByTransferHeadResponseDTO>(
                        bytransferheadid
                    );
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

        [HttpGet("by-transfer-headmaps")]
        [Tags("Pension: By Transfer")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ByTransferHeadResponseDTO>>
        > GetAllByTransferHeads()
        {
            JsonAPIResponse<TableResponseDTO<ByTransferHeadResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "All By Transfer Head details retrieved successfully!",
            };

            try
            {
                var byTransferHeads =
                    await _byTransferHeadService.GetAllByTransferHeads<ByTransferHeadResponseDTO>();

                response.Result = new()
                {
                    Headers = new()
                    {
                        new() { Name = "ID", FieldName = "id" },
                        new() { Name = "By Transfer Type", FieldName = "byTransferType" },
                        new() { Name = "Account Head ID", FieldName = "accountHeadId" },
                        new() { Name = "Description", FieldName = "byTransferDescription" },
                        new() { Name = "AG By Transfer", FieldName = "agBytransfer" },
                        new()
                        {
                            Name = "By Transfer Head Count",
                            FieldName = "ByTransferHeadCount",
                        },
                    },
                    Data = byTransferHeads,
                };
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
