using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Controllers.Pension
{
    [Route("api/v1/ppo")]
    public class PpoDetailsController : ApiBaseController
    {
        private readonly IPensionerDetailsService _pensionerDetailsService;

        public PpoDetailsController(
                IPensionerDetailsService pensionerDetailsService,
                IClaimService claimService
            ) : base(claimService)
        {
            _pensionerDetailsService = pensionerDetailsService;

        }

        [HttpPost("details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> CreatePensioner(
                PensionerEntryDTO pensionerEntryDTO
            )
        {

            JsonAPIResponse<PensionerResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details saved sucessfully!",
                Result = new(){
                    PpoId = 0
                }
            };
            try {
                response.Result = await _pensionerDetailsService.CreatePensioner(
                    pensionerEntryDTO,
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

        [HttpPut("{ppoId}/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> UpdatePensionerByPpoId(
                int ppoId,
                PensionerEntryDTO pensionerEntryDTO
            )
        {

            JsonAPIResponse<PensionerResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details saved sucessfully!",
                Result = new(){
                    PpoId = 0
                }
            };
            try {
                response.Result = await _pensionerDetailsService.UpdatePensioner(
                    ppoId,
                    pensionerEntryDTO,
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

        [HttpGet("{ppoId}/details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> GetPensionerByPpoId(
            int ppoId
        )
        {

            JsonAPIResponse<PensionerResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details received sucessfully!",
                Result = new(){
                    PpoId = 0
                }
            };
            try {
                response.Result = await _pensionerDetailsService.GetPensioner<PensionerResponseDTO>(
                    ppoId,
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

        [HttpPatch("details")]
        [Tags("Pension: PPO Details")]
        [OpenApi]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionerListItemDTO>>>> GetAllPensioners(
            DynamicListQueryParameters dynamicListQueryParameters
        )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionerListItemDTO>>> response = new();
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "PPO ID",
                                    DataType = "text",
                                    FieldName = "ppoId",
                                    FilterField = "ppoId",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "Name of Pensioner",
                                    DataType = "text",
                                    FieldName = "pensionerName",
                                    FilterField = "pensionerName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Mobile",
                                    DataType = "text",
                                    FieldName = "mobileNumber",
                                    FilterField = "mobileNumber",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Date of Birth",
                                    DataType = "text",
                                    FieldName = "dateOfBirth",
                                    FilterField = "dateOfBirth",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Date of Retirement",
                                    DataType = "text",
                                    FieldName = "dateOfRetirement",
                                    FilterField = "dateOfRetirement",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "PPO No",
                                    DataType = "text",
                                    FieldName = "ppoNo",
                                    FilterField = "ppoNo",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionerDetailsService.GetAllPensioners(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionerDetailsService.DataCount()
                        },
                    Message = $"All PPO Details Received Successfully!"

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
    
    }
}