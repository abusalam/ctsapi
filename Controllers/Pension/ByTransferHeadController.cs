using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class ByTransferHeadController : ApiBaseController
    {
        private readonly IByTransferHeadService _byTransferHeadService;

        public ByTransferHeadController(
            IClaimService claimService,
            IByTransferHeadService byTransferHeadService
        )
            : base(claimService)
        {
            _byTransferHeadService = byTransferHeadService;
        }

        [HttpPost("by-transfer-headmap")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> CreateByTransferHeadMap(
            ByTransferHeadEntryDTO byTransferHeadEntryDTO
        )
        {
            JsonAPIResponse<ByTransferHeadResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "By Transfer Head saved successfully!",
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
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [HttpPut("by-transfer-headmap/{byTransferHeadId}")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> UpdateByTransferHeadMap(
            long byTransferHeadId,
            ByTransferHeadUpdateDTO byTransferHeadUpdateDTO
        )
        {
            JsonAPIResponse<ByTransferHeadResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "By Transfer Head updated successfully!",
            };

            try
            {
                response.Result =
                    await _byTransferHeadService.UpdateByTransferHead<ByTransferHeadResponseDTO>(
                        byTransferHeadId,
                        byTransferHeadUpdateDTO
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

        [HttpDelete("by-transfer-headmap/{byTransferHeadId}")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<JsonAPIResponse<BaseDTO>> DeleteByTransferHeadMapById(
            long byTransferHeadId
        )
        {
            JsonAPIResponse<BaseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "By Transfer Head deleted successfully!",
            };

            try
            {
                response.Result = await _byTransferHeadService.DeleteByTransferHeadMapById<BaseDTO>(
                    byTransferHeadId
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

        [HttpGet("by-transfer-headmap/{byTransferHeadId}")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> GetByTransferHeadMapById(
            long byTransferHeadId
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
                    await _byTransferHeadService.GetByTransferHeadMapById<ByTransferHeadResponseDTO>(
                        byTransferHeadId
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

        [HttpGet("by-transfer-headmaps")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ByTransferHeadResponseDTO>>
        > GetByTransferHeadMaps()
        {
            JsonAPIResponse<TableResponseDTO<ByTransferHeadResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "All By Transfer Head details retrieved successfully!",
            };

            try
            {
                TableResponseDTO<ByTransferHeadResponseDTO>? tableResponse =
                    await _byTransferHeadService.GetByTransferHeadMaps<
                        TableResponseDTO<ByTransferHeadResponseDTO>
                    >();

                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "BT No", FieldName = "id" },
                        new() { Name = "By Transfer Type", FieldName = "byTransferType" },
                        new() { Name = "Account Head ID", FieldName = "accountHeadId" },
                        new() { Name = "Description", FieldName = "byTransferDescription" },
                    ],
                    Data = tableResponse.Data,
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
