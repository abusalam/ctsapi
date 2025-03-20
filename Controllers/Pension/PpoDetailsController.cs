using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PpoDetailsController(
        IPensionerDetailsService pensionerDetailsService,
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IPensionerDetailsService _pensionerDetailsService =
            pensionerDetailsService;

        [HttpPost("ppo/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> CreatePensioner(
            PensionerEntryDTO pensionerEntryDTO
        )
        {
            JsonAPIResponse<PensionerResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Pensioner saved successfully!",
            };
            try
            {
                response.Result = await _pensionerDetailsService.CreatePensioner(
                    pensionerEntryDTO,
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
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [HttpPut("ppo/{ppoId}/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> UpdatePensionerByPpoId(
            int ppoId,
            PensionerEntryDTO pensionerEntryDTO
        )
        {
            JsonAPIResponse<PensionerResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details saved sucessfully!",
            };
            try
            {
                response.Result = await _pensionerDetailsService.UpdatePensioner(
                    ppoId,
                    pensionerEntryDTO,
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
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [HttpGet("ppo/{ppoId}/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> GetPensionerByPpoId(int ppoId)
        {
            JsonAPIResponse<PensionerResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO details received successfully!",
            };

            try
            {
                response.Result = await _pensionerDetailsService.GetPensioner<PensionerResponseDTO>(
                    ppoId,
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
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [HttpGet("ppo/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>>> GetPensioners()
        {
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All PPO Details Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "PPO ID", FieldName = "ppoId" },
                        new() { Name = "Name of Pensioner", FieldName = "pensionerName" },
                        new() { Name = "Mobile", FieldName = "mobileNumber" },
                        new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                        new() { Name = "Date of Retirement", FieldName = "dateOfRetirement" },
                        new() { Name = "PPO No", FieldName = "ppoNo" },
                    ],
                    Data = await _pensionerDetailsService.GetPensioners<PensionerListItemDTO>(
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    ),
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

        [HttpGet("ppo/details/not-approved")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>>
        > GetAllNotApprovedPensioners()
        {
            JsonAPIResponse<TableResponseDTO<PensionerListItemDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Not Approved PPO Details Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "PPO ID", FieldName = "ppoId" },
                        new() { Name = "Name of Pensioner", FieldName = "pensionerName" },
                        new() { Name = "Mobile", FieldName = "mobileNumber" },
                        new() { Name = "Date of Birth", FieldName = "dateOfBirth" },
                        new() { Name = "Date of Retirement", FieldName = "dateOfRetirement" },
                        new() { Name = "PPO No", FieldName = "ppoNo" },
                    ],
                    Data = await _pensionerDetailsService.GetAllNonApprovedPensioners(
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    ),
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
