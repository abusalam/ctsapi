using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class NomineeController : ApiBaseController
    {
        private readonly INomineeService _nomineeService;
        private readonly ILogger<NomineeController> _logger;

        public NomineeController(
            INomineeService nomineeService,
            IClaimService claimService,
            ILogger<NomineeController> logger
        )
            : base(claimService)
        {
            _nomineeService = nomineeService;
            _logger = logger;
        }

        [HttpGet("nominee/{nomineeId}")]
        [Tags("Pension: Nominee Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<NomineeResponseDTO>> GetNomineeDetailsById(int nomineeId)
        {
            _logger.LogInformation(
                "Received request to get nominee details by ID: {NomineeId}",
                nomineeId
            );
            JsonAPIResponse<NomineeResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Nominee Details received sucessfully!",
            };
            try
            {
                response.Result =
                    await _nomineeService.GetNomineeDetailsByNomineeId<NomineeResponseDTO>(
                        nomineeId,
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching nominee details for ID: {NomineeId}",
                    nomineeId
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

        [HttpGet("{ppoId}/nominees")]
        [Tags("Pension: Nominee Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<TableResponseDTO<NomineeResponseDTO>>> GetNomineesByPpoId(
            int ppoId
        )
        {
            _logger.LogInformation("Received request to get nominees for PPO ID: {PpoId}", ppoId);
            JsonAPIResponse<TableResponseDTO<NomineeResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Nominee Details received sucessfully!",
            };
            try
            {
                NomineeListResponseDTO? nomineeList = await _nomineeService.GetNomineeByPpoId(
                    ppoId,
                    GetTreasuryCode()
                );

                response.Result = new()
                {
                    Headers = new()
                    {
                        new() { Name = "Serial No", FieldName = "serialNo" },
                        new() { Name = "Nominee Name", FieldName = "nomineeName" },
                        new() { Name = "Relation", FieldName = "relation" },
                        new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                        new() { Name = "Active", FieldName = "nomineeActive" },
                    },
                    Data = nomineeList.Nominees ?? new(),
                    DataSource = nomineeList.DataSource,
                };
                _logger.LogInformation(
                    "Nominee details for PPO ID {PpoId} retrieved successfully.",
                    ppoId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching nominees for PPO ID: {PpoId}",
                    ppoId
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

        [HttpPost("nominee")]
        [Tags("Pension: Nominee Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<NomineeResponseDTO>> RegisterNomineeDetails(
            NomineeEntryDTO nomineeEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to register nominee details: {NomineeEntryDTO}",
                nomineeEntryDTO
            );
            JsonAPIResponse<NomineeResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Nominee Details saved sucessfully!",
            };
            try
            {
                response.Result = await _nomineeService.CreateNomineeDetails<NomineeResponseDTO>(
                    nomineeEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
                _logger.LogInformation(
                    "Nominee details registered successfully for PPO ID: {PpoId}",
                    nomineeEntryDTO.PpoId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while registering nominee details with data: {Data}",
                    nomineeEntryDTO
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

        [HttpPut("nominee/{nomineeId}")]
        [Tags("Pension: Nominee Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<NomineeResponseDTO>> UpdateNomineeDetailsById(
            long nomineeId,
            NomineeEntryDTO nomineeEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to update nominee details for ID: {NomineeId} with data: {Data}",
                nomineeId,
                nomineeEntryDTO
            );
            JsonAPIResponse<NomineeResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Nominee Details updated sucessfully!",
            };
            try
            {
                response.Result =
                    await _nomineeService.UpdateNomineeDetailsById<NomineeResponseDTO>(
                        nomineeId,
                        nomineeEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
                _logger.LogInformation(
                    "Nominee details for ID {NomineeId} updated successfully.",
                    nomineeId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating nominee details for ID: {NomineeId} with data: {Data}",
                    nomineeId,
                    nomineeEntryDTO
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

        [HttpDelete("nominee/{nomineeId}")]
        [Tags("Pension: Nominee Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<NomineeResponseDTO>> DeleteNomineeDetailsById(
            long nomineeId
        )
        {
            _logger.LogInformation(
                "Received request to delete nominee details for ID: {NomineeId}",
                nomineeId
            );
            JsonAPIResponse<NomineeResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Nominee Details deleted sucessfully!",
            };
            try
            {
                response.Result =
                    await _nomineeService.DeleteNomineeDetailsById<NomineeResponseDTO>(
                        nomineeId,
                        GetTreasuryCode()
                    );
                _logger.LogInformation(
                    "Nominee details for ID {NomineeId} deleted successfully.",
                    nomineeId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while deleting nominee details for ID: {NomineeId}",
                    nomineeId
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
