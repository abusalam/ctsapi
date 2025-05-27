using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class ByTransferHeadController : ApiBaseController
    {
        private readonly IByTransferHeadService _byTransferHeadService;
        private readonly ILogger<ByTransferHeadController> _logger;

        public ByTransferHeadController(
            IClaimService claimService,
            IByTransferHeadService byTransferHeadService,
            ILogger<ByTransferHeadController> logger
        )
            : base(claimService)
        {
            _byTransferHeadService = byTransferHeadService;
            _logger = logger;
        }

        [HttpPost("by-transfer-headmap")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> CreateByTransferHeadMap(
            ByTransferHeadEntryDTO byTransferHeadEntryDTO
        )
        {
            _logger.LogInformation("Received request to fetch By-Transfer Heads.");

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
                _logger.LogError(
                    ex,
                    "Error occurred while creating By-Transfer Head map with data: {Data}",
                    byTransferHeadEntryDTO
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

        [HttpPut("by-transfer-headmap/{byTransferHeadId}")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> UpdateByTransferHeadMap(
            long byTransferHeadId,
            ByTransferHeadUpdateDTO byTransferHeadUpdateDTO
        )
        {
            _logger.LogInformation(
                "Received request to update By-Transfer Head map with ID: {ByTransferHeadId}",
                byTransferHeadId
            );
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
                _logger.LogError(
                    ex,
                    "Error occurred while updating By-Transfer Head map with ID: {ByTransferHeadId} and data: {Data}",
                    byTransferHeadId,
                    byTransferHeadUpdateDTO
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

        [HttpDelete("by-transfer-headmap/{byTransferHeadId}")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<JsonAPIResponse<BaseDTO>> DeleteByTransferHeadMapById(
            long byTransferHeadId
        )
        {
            _logger.LogInformation(
                "Received request to delete By-Transfer Head map with ID: {ByTransferHeadId}",
                byTransferHeadId
            );
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
                _logger.LogError(
                    ex,
                    "Error occurred while deleting By-Transfer Head map with ID: {ByTransferHeadId}",
                    byTransferHeadId
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

        [Authorize("roles:Treasury Officer,Admin|permissions:can-receive-bill,can-bill-check")]
        [HttpGet("by-transfer-headmap/{byTransferHeadId}")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<JsonAPIResponse<ByTransferHeadResponseDTO>> GetByTransferHeadMapById(
            long byTransferHeadId
        )
        {
            _logger.LogInformation(
                "Received request to fetch By-Transfer Head map with ID: {ByTransferHeadId}",
                byTransferHeadId
            );

            JsonAPIResponse<ByTransferHeadResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = "By Transfer Head details received successfully!",
            };

            try
            {
                var result =
                    await _byTransferHeadService.GetByTransferHeadMapById<ByTransferHeadResponseDTO>(
                        byTransferHeadId
                    );

                if (result == null)
                {
                    _logger.LogWarning(
                        "No By-Transfer Head found for ID: {ByTransferHeadId}",
                        byTransferHeadId
                    );
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "No data found for the given ID.";
                }
                else
                {
                    response.Result = result;
                    _logger.LogInformation(
                        "Successfully fetched By-Transfer Head map: {@ByTransferHead}",
                        result
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching By-Transfer Head map with ID: {ByTransferHeadId}",
                    byTransferHeadId
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

        //  [Authorize(Policy = "CanManageUsers")]
        [Authorize("roles:Treasury Officer,Admin|permissions:can-receive-bill,can-bill-check")]
        [HttpGet("by-transfer-headmaps")]
        [Tags("Pension: By-Transfer Head")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ByTransferHeadResponseDTO>>
        > GetByTransferHeadMaps()
        {
            _logger.LogInformation("Received request to fetch all By-Transfer Head maps.");
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
                _logger.LogError(ex, "Error occurred while fetching all By-Transfer Head maps.");
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
