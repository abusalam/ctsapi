using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CTS_BE.Controllers.Pension
{
    public class LifeCertificateController : ApiBaseController
    {
        private readonly ILifeCertificateService _lifeCertificateService;
        public LifeCertificateController(
            ILifeCertificateService lifeCertificateService,
            IClaimService claimService
        ) : base(claimService)
        {
            _lifeCertificateService = lifeCertificateService;
        }

        [HttpGet("lifecertificate/{ppoId}")]
        [Tags("Pension: Life Certificate")]
        [OpenApi]
        public async Task<JsonAPIResponse<LifeCertificateResponseDTO>> GetLifeCertificateByPpoId(
            int ppoId
        )
        {

            JsonAPIResponse<LifeCertificateResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Life Certificate received sucessfully!"
            };
            try {
                response.Result = await _lifeCertificateService.GetLifeCertificateByPpoId<LifeCertificateResponseDTO>(
                    ppoId,
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

        [Route("api/v1/ppo/lifecertificate")]
        [HttpPost]
        [Tags("Pension: Life Certificate")]
        [OpenApi]
        public async Task<JsonAPIResponse<LifeCertificateResponseDTO>> AddLifeCertificate(
            LifeCertificateEntryDTO lifeCertificateEntryDTO
        )
        {

            JsonAPIResponse<LifeCertificateResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Life Certificate saved sucessfully!"
            };
            try {
                response.Result = await _lifeCertificateService.CreateLifeCertificate<LifeCertificateResponseDTO>(
                    lifeCertificateEntryDTO,
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

        [HttpPut("lifecertificate/{ppoId}")]
        [Tags("Pension: Life Certificate")]
        [OpenApi]
        public async Task<JsonAPIResponse<LifeCertificateResponseDTO>> UpdateLifeCertificateByPpoId(
            long ppoId,
            LifeCertificateEntryDTO lifeCertificateEntryDTO
        )
        {

            JsonAPIResponse<LifeCertificateResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Life Certificate updated sucessfully!"
            };
            try {
                response.Result = await _lifeCertificateService.UpdateLifeCertificateByPpoId<LifeCertificateResponseDTO>(
                    ppoId,
                    lifeCertificateEntryDTO,
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

    }
}