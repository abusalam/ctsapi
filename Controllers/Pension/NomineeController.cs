using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class NomineeController : ApiBaseController
    {
        private readonly INomineeService _nomineeService;

        public NomineeController(INomineeService nomineeService, IClaimService claimService)
            : base(claimService)
        {
            _nomineeService = nomineeService;
        }

        [HttpGet("nominee/{nomineeId}")]
        [Tags("Pension: Nominee Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<NomineeResponseDTO>> GetNomineeDetailsById(int nomineeId)
        {
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
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
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

        [HttpPost("nominee")]
        [Tags("Pension: Nominee Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<NomineeResponseDTO>> RegisterNomineeDetails(
            NomineeEntryDTO nomineeEntryDTO
        )
        {
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

        [HttpPut("nominee/{nomineeId}")]
        [Tags("Pension: Nominee Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<NomineeResponseDTO>> UpdateNomineeDetailsById(
            long nomineeId,
            NomineeEntryDTO nomineeEntryDTO
        )
        {
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

        [HttpDelete("nominee/{nomineeId}")]
        [Tags("Pension: Nominee Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<NomineeResponseDTO>> DeleteNomineeDetailsById(
            long nomineeId
        )
        {
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
