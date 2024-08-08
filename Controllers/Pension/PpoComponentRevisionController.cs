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
    public class PpoComponentRevisionController : ApiBaseController
    {
        private readonly IPpoComponentRateService _ppoComponentRateService;
        public PpoComponentRevisionController(
                IPpoComponentRateService ppoComponentRateService,
                IClaimService claimService
            ) : base(claimService)
        {
            _ppoComponentRateService = ppoComponentRateService;
        }

        [HttpPost("component-revision-rate")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<JsonAPIResponse<PpoComponentRateResponseDTO>> PpoComponentRevisionRatesCreate(
                PpoComponentRateEntryDTO ppoComponentRateEntryDTO
            )
        {

            JsonAPIResponse<PpoComponentRateResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component Rate saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _ppoComponentRateService
                    .CreatePpoComponentRate<PpoComponentRateEntryDTO, PpoComponentRateResponseDTO>(
                        ppoComponentRateEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            finally {
                if(response.Result.Id == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Component Rate not saved!";
                }
            }

            return response;
        }

        [HttpPatch("component-revision-rate")]
        [Tags("Pension: Component Revision")]
        [OpenApi]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PpoComponentRateResponseDTO>>>> PpoComponentRevisionRatesList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PpoComponentRateResponseDTO>>> response;
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Primary Category ID",
                                    DataType = "text",
                                    FieldName = "id",
                                    FilterField = "id",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Head of Account",
                                    DataType = "text",
                                    FieldName = "hoaId",
                                    FilterField = "hoaId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Primary Category Name",
                                    DataType = "text",
                                    FieldName = "primaryCategoryName",
                                    FilterField = "primaryCategoryName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _ppoComponentRateService.ListPpoComponentRates<PpoComponentRateResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _ppoComponentRateService.DataCount()
                        },
                    Message = $"All Component Rate Details Received Successfully!"

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