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
    [Route("api/v1/pension")]
    public class PpoComponentController : ApiBaseController
    {
        
        private readonly IPensionBreakupService _pensionBreakupService;
        private readonly IComponentRateService _pensionRateService;
        public PpoComponentController(
                IPensionBreakupService pensionBreakupService,
                IComponentRateService pensionRateService,
                IClaimService claimService
            ) : base(claimService)
        {
            _pensionBreakupService = pensionBreakupService;
            _pensionRateService = pensionRateService;
        }

        
        [HttpPost("bill-component")]
        [Tags("Pension", "Pension: Component")]
        public async Task<JsonAPIResponse<PensionBreakupResponseDTO>> ControlPensionBillComponentCreate(
                PensionBreakupEntryDTO pensionBreakupEntryDTO
            )
        {

            JsonAPIResponse<PensionBreakupResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionBreakupService
                    .CreatePensionBreakup<PensionBreakupEntryDTO, PensionBreakupResponseDTO>(
                        pensionBreakupEntryDTO,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
            }
            catch(InvalidCastException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"C-Error: {ex.Message}";
            }
            finally {
                if(response.Result.Id == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Component not saved!";
                }
            }

            return response;
        }

        [HttpPatch("bill-component")]
        [Tags("Pension", "Pension: Component")]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionBreakupResponseDTO>>>> ControlComponentList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionBreakupResponseDTO>>> response;
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Bill Component ID",
                                    DataType = "text",
                                    FieldName = "id",
                                    FilterField = "id",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "Component Name",
                                    DataType = "text",
                                    FieldName = "componentName",
                                    FilterField = "componentName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Component Type",
                                    DataType = "text",
                                    FieldName = "componentType",
                                    FilterField = "componentType",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Relief Allowed",
                                    DataType = "text",
                                    FieldName = "reliefFlag",
                                    FilterField = "reliefFlag",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionBreakupService.ListBreakup<PensionBreakupResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionBreakupService.DataCount()
                        },
                    Message = $"All Bill Breakups Received Successfully!"

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
 
        [HttpPost("component-rate")]
        [Tags("Pension", "Pension: Component Rate")]
        public async Task<JsonAPIResponse<ComponentRateResponseDTO>> ControlComponentRateCreate(
                ComponentRateEntryDTO pensionRatesEntryDTO
            )
        {

            JsonAPIResponse<ComponentRateResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component Rate saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionRateService
                    .CreateComponentRates<ComponentRateEntryDTO, ComponentRateResponseDTO>(                    
                            pensionRatesEntryDTO,
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        );
            }
            catch(InvalidCastException ex) {
                response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = $"C-Error: {ex.Message}";
            }
            finally {
                if(response.Result.Id == 0) {
                    response.ApiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = $"C-Error: Component Rate not saved!";
                }
            }

            return response;
        }

        [HttpPatch("component-rate")]
        [Tags("Pension", "Pension: Component Rate")]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<ComponentRateResponseDTO>>>> ControlComponentRateList(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<ComponentRateResponseDTO>>> response = new();
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {
                            
                                new() {
                                    Name = "Component Rate ID",
                                    DataType = "text",
                                    FieldName = "id",
                                    FilterField = "id",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "Category ID",
                                    DataType = "text",
                                    FieldName = "categoryId",
                                    FilterField = "categoryId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Bill Breakup ID",
                                    DataType = "text",
                                    FieldName = "breakupId",
                                    FilterField = "breakupId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Effective From Date",
                                    DataType = "text",
                                    FieldName = "effectiveFromDate",
                                    FilterField = "effectiveFromDate",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Rate Amount",
                                    DataType = "text",
                                    FieldName = "rateAmount",
                                    FilterField = "rateAmount",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Rate Type",
                                    DataType = "text",
                                    FieldName = "rateType",
                                    FilterField = "rateType",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionRateService.ListComponentRates<ComponentRateResponseDTO>(
                                    GetCurrentFyYear(),
                                    GetTreasuryCode(),
                                    dynamicListQueryParameters
                                ),
                            DataCount = _pensionRateService.DataCount()
                        },
                    Message = $"All Bill Breakups Received Successfully!"

                };
            }
            finally {

            }
            return response;
        }
 
    }
}