using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class PpoSanctionDetailsController : ApiBaseController
    {

        private readonly IPpoSanctionDetailsService _ppoSanctionDetailsService;
        public PpoSanctionDetailsController(
            IPpoSanctionDetailsService ppoSanctionDetailsService,
            IClaimService claimService
        ) : base(claimService)
        {
            _ppoSanctionDetailsService = ppoSanctionDetailsService;
        }

        [HttpPost("sanction")]
        [Tags("Pension: Sanction Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoSanctionDetailsResponseDTO>> CreateSanctionDetails(
            PpoSanctionDetailsEntryDTO ppoSanctionDetailsEntryDTO
        )
        {

            JsonAPIResponse<PpoSanctionDetailsResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Sanction Details saved sucessfully!"
            };
            try {
                response.Result = await _ppoSanctionDetailsService.CreateSanctionDetails<PpoSanctionDetailsResponseDTO>(
                    ppoSanctionDetailsEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch(Exception ex) {
                FillException(response, ex);
                return response;
            }
            finally {
                FillErrorMesageFromDataSource(response);
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

            JsonAPIResponse<PpoSanctionDetailsResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Sanction Details updated sucessfully!"
            };
            try {
                response.Result = await _ppoSanctionDetailsService.UpdateSanctionDetailsById<PpoSanctionDetailsResponseDTO>(
                    sanctionDetailsId,
                    ppoSanctionDetailsEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch(Exception ex) {
                FillException(response, ex);
                return response;
            }
            finally {
                FillErrorMesageFromDataSource(response);
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

            JsonAPIResponse<PpoSanctionDetailsResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Sanction Details received sucessfully!"
            };
            try {
                response.Result = await _ppoSanctionDetailsService.GetSanctionDetailsById<PpoSanctionDetailsResponseDTO>(
                    sanctionDetailsId,
                    GetTreasuryCode()
                );
            }
            catch(Exception ex) {
                FillException(response, ex);
                return response;
            }
            finally {
                FillErrorMesageFromDataSource(response);
            }

            return response;
        }
    }
}