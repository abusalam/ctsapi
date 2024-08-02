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
    public class PpoDetails : ApiBase
    {
        private readonly IPensionerDetailsService _pensionerDetailsService;

        public PpoDetails(
                IPensionerDetailsService pensionerDetailsService,
                IClaimService claimService
            ) : base(claimService)
        {
            _pensionerDetailsService = pensionerDetailsService;

        }

        [HttpPost("details")]
        [Tags("Pension", "Pension: PPO Details")]
        public async Task<JsonAPIResponse<PensionerResponseDTO>> ControlPensionerDetailsCreate(
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
            catch (DbUpdateException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"PPO Details not saved! Error: {ex.Message}";
            }
            finally {
                if(response.Result.PpoId == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"Error: PPO Details not saved!";
                }
            }

            return response;
        }

        [HttpPut("{ppoId}/details")]
        [Tags("Pension", "Pension: PPO Details")]
        public async Task<APIResponse<PensionerResponseDTO>> ControlPensionerDetailsUpdate(
                int ppoId,
                PensionerEntryDTO pensionerEntryDTO
            )
        {

            APIResponse<PensionerResponseDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details saved sucessfully!",
                result = new(){
                    PpoId = 0
                }
            };
            try {
                response.result = await _pensionerDetailsService.UpdatePensioner(
                    ppoId,
                    pensionerEntryDTO,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"PPO Details not saved! Error: {ex.Message}";
            }
            finally {
                if(response.result.PpoId == 0) {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"Error: PPO Details not saved!";
                }
            }

            return response;
        }

        [HttpGet("{ppoId}/details")]
        [Tags("Pension", "Pension: PPO Details")]
        public async Task<APIResponse<PensionerResponseDTO>> ControlPensionerDetailsRead(
                int ppoId
            )
        {

            APIResponse<PensionerResponseDTO> response = new(){
                apiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PPO Details received sucessfully!",
                result = new(){
                    PpoId = 0
                }
            };
            try {
                response.result = await _pensionerDetailsService.GetPensioner<PensionerResponseDTO>(
                    ppoId,
                    GetCurrentFyYear(),
                    GetTreasuryCode()
                );
            }
            catch (DbUpdateException ex) {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"PPO Details not received! Error: {ex.Message}";
            }
            finally {
                if(response.result.PpoId == 0) {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"PPO Details not received!";
                }
            }

            return response;
        }

        [HttpPatch("details")]
        [Tags("Pension", "Pension: PPO Details")]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionerListItemDTO>>>> ControlPensionerDetailsList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionerListItemDTO>>> response;
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
            } catch(DbUpdateException e) {
                // StackFrame CallStack = new(1, true);
                response = new () {
                ApiResponseStatus = Enum.APIResponseStatus.Error,
                Result = null,
                Message = e.ToString()
                //   $"{e.GetType()}=>File:{CallStack.GetFileName()}({CallStack.GetFileLineNumber()}): {e.Message}"
                };
            }
            return response;
        }
    
    }
}