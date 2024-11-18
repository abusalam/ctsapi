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

        [Route("api/v1/ppo/{ppoId}/lifecertificate")]
        [HttpGet]
        [Tags("Pension: Life Certificate")]
        [OpenApi]
        public async Task<JsonAPIResponse<TableResponseDTO<LifeCertificateResponseDTO>>> GetLifeCertificateByPpoId(
            int ppoId
        )
        {
            JsonAPIResponse<TableResponseDTO<LifeCertificateResponseDTO>> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Life Certificate received sucessfully!"
            };
            try {
                LifeCertificateListResponseDTO? lifeCertificateList = await _lifeCertificateService.GetLifeCertificateByPpoId(
                    ppoId,
                    GetTreasuryCode()
                );

                response.Result = new () {
                    Headers = new (){
                        new (){
                            Name = "PPO ID",
                            FieldName = "ppoId",
                        },
                        new (){
                            Name = "PPO No",
                            FieldName = "ppoNo",
                        },
                        new (){
                            Name = "Name of Pensioner",
                            FieldName = "pensionerName",
                        },
                        new (){
                            Name = "Account Number",
                            FieldName = "bankAcNo",
                        },
                        new (){
                            Name = "Mobile Number",
                            FieldName = "mobileNumber",
                        }
                    },
                    Data = lifeCertificateList.LifeCertificates ?? new (),
                    DataSource = lifeCertificateList.DataSource
                };
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

    }
}