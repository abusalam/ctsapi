using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    // [Authorize("roles:clerk|permissions:can-receive-bill")]
    [Route("api/v1/ppo")]
    public class PpoSanctionDetailsController : ApiBaseController
    {
        private readonly IPpoSanctionDetailsService _ppoSanctionDetailsService;
        private readonly ILogger<PpoSanctionDetailsController> _logger;

        public PpoSanctionDetailsController(
            IPpoSanctionDetailsService ppoSanctionDetailsService,
            IClaimService claimService,
            ILogger<PpoSanctionDetailsController> logger
        )
            : base(claimService)
        {
            _ppoSanctionDetailsService = ppoSanctionDetailsService;
            _logger = logger;
        }

        [HttpPost("sanction")]
        [Tags("Pension: Sanction Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoSanctionDetailsResponseDTO>> CreateSanctionDetails(
            PpoSanctionDetailsEntryDTO ppoSanctionDetailsEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to create PPO Sanction Details with data: {Data}",
                ppoSanctionDetailsEntryDTO
            );
            JsonAPIResponse<PpoSanctionDetailsResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Sanction Details saved sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoSanctionDetailsService.CreateSanctionDetails<PpoSanctionDetailsResponseDTO>(
                        ppoSanctionDetailsEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating PPO Sanction Details with data: {Data}",
                    ppoSanctionDetailsEntryDTO
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

        [HttpPut("sanction/{sanctionDetailsId}")]
        [Tags("Pension: Sanction Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoSanctionDetailsResponseDTO>> UpdateSanctionDetailsById(
            long sanctionDetailsId,
            PpoSanctionDetailsEntryDTO ppoSanctionDetailsEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to update PPO Sanction Details with ID: {SanctionDetailsId} and data: {Data}",
                sanctionDetailsId,
                ppoSanctionDetailsEntryDTO
            );
            JsonAPIResponse<PpoSanctionDetailsResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Sanction Details updated sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoSanctionDetailsService.UpdateSanctionDetailsById<PpoSanctionDetailsResponseDTO>(
                        sanctionDetailsId,
                        ppoSanctionDetailsEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating PPO Sanction Details with ID: {SanctionDetailsId} and data: {Data}",
                    sanctionDetailsId,
                    ppoSanctionDetailsEntryDTO
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

        [HttpGet("sanction/{sanctionDetailsId}")]
        [Tags("Pension: Sanction Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoSanctionDetailsResponseDTO>> GetSanctionDetailsById(
            long sanctionDetailsId
        )
        {
            _logger.LogInformation(
                "Received request to get PPO Sanction Details by ID: {SanctionDetailsId}",
                sanctionDetailsId
            );
            JsonAPIResponse<PpoSanctionDetailsResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Sanction Details received sucessfully!",
            };
            try
            {
                response.Result =
                    await _ppoSanctionDetailsService.GetSanctionDetailsById<PpoSanctionDetailsResponseDTO>(
                        sanctionDetailsId,
                        GetTreasuryCode()
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPO Sanction Details for ID: {SanctionDetailsId}",
                    sanctionDetailsId
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
