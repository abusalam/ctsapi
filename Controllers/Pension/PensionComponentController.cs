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
    public class PensionComponentController : ApiBaseController
    {
        private readonly IPensionBreakupService _pensionBreakupService;
        private readonly IComponentRateService _pensionRateService;

        public PensionComponentController(
            IPensionBreakupService pensionBreakupService,
            IComponentRateService pensionRateService,
            IClaimService claimService
        )
            : base(claimService)
        {
            _pensionBreakupService = pensionBreakupService;
            _pensionRateService = pensionRateService;
        }

        [HttpPost("component")]
        [Tags("Pension: Component")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionBreakupResponseDTO>> CreateComponent(
            PensionBreakupEntryDTO pensionBreakupEntryDTO
        )
        {
            JsonAPIResponse<PensionBreakupResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component saved sucessfully!",
                Result = new() { Id = 0 },
            };
            try
            {
                response.Result = await _pensionBreakupService.CreatePensionBreakup<
                    PensionBreakupEntryDTO,
                    PensionBreakupResponseDTO
                >(pensionBreakupEntryDTO, GetCurrentFyYear(), GetTreasuryCode());
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

        [HttpPatch("component")]
        [Tags("Pension: Component")]
        [OpenApi]
        [Obsolete("Use GetComponents instead")]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionBreakupResponseDTO>>
        > GetAllComponents()
        {
            JsonAPIResponse<TableResponseDTO<PensionBreakupResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Bill Component ID", FieldName = "id" },
                            new() { Name = "Component Name", FieldName = "componentName" },
                            new() { Name = "Component Type", FieldName = "componentType" },
                            new() { Name = "Relief Allowed", FieldName = "reliefFlag" },
                        },
                        Data = await _pensionBreakupService.ListBreakup(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                    },
                    Message = $"All Bill Breakups Received Successfully!",
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

        [HttpGet("component")]
        [Tags("Pension: Component")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionBreakupResponseDTO>>
        > GetComponents()
        {
            JsonAPIResponse<TableResponseDTO<PensionBreakupResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Bill Component ID", FieldName = "id" },
                            new() { Name = "Component Name", FieldName = "componentName" },
                            new() { Name = "Component Type", FieldName = "componentType" },
                            new() { Name = "Relief Allowed", FieldName = "reliefFlag" },
                        },
                        Data = await _pensionBreakupService.GetBreakups<PensionBreakupResponseDTO>(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                    },
                    Message = $"All Bill Breakups Received Successfully!",
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

        [HttpPost("component-rate")]
        [Tags("Pension: Component Rate")]
        [OpenApi]
        public async Task<JsonAPIResponse<ComponentRateResponseDTO>> CreateComponentRate(
            ComponentRateEntryDTO pensionRatesEntryDTO
        )
        {
            JsonAPIResponse<ComponentRateResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Component Rate saved sucessfully!",
                Result = new() { Id = 0 },
            };
            try
            {
                response.Result = await _pensionRateService.CreateComponentRates<
                    ComponentRateEntryDTO,
                    ComponentRateResponseDTO
                >(pensionRatesEntryDTO, GetCurrentFyYear(), GetTreasuryCode());
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

        [HttpGet("{categoryId}/component-rate")]
        [Tags("Pension: Component Rate")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ComponentRateResponseDTO>>
        > GetComponentRatesByCategoryId(long categoryId)
        {
            JsonAPIResponse<TableResponseDTO<ComponentRateResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Rate ID", FieldName = "id" },
                            new() { Name = "Breakup", FieldName = "componentName" },
                            new() { Name = "Effective From Date", FieldName = "withEffectFrom" },
                            new() { Name = "Rate", FieldName = "componentRate" },
                            new() { Name = "Type", FieldName = "componentType" },
                        },
                        Data =
                            await _pensionRateService.ListComponentRatesByCategoryId<ComponentRateResponseDTO>(
                                categoryId
                            ),
                    },
                    Message = $"All Component Rates Received Successfully!",
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

        [HttpPatch("component-rate")]
        [Tags("Pension: Component Rate")]
        [OpenApi]
        [Obsolete("Use GetComponentRatesByCategoryId instead")]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ComponentRateResponseDTO>>
        > GetAllComponentRates()
        {
            JsonAPIResponse<TableResponseDTO<ComponentRateResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Component Rate ID", FieldName = "id" },
                            new() { Name = "Category ID", FieldName = "categoryId" },
                            new() { Name = "Bill Breakup ID", FieldName = "breakupId" },
                            new() { Name = "Effective From Date", FieldName = "effectiveFromDate" },
                            new() { Name = "Rate Amount", FieldName = "rateAmount" },
                            new() { Name = "Rate Type", FieldName = "rateType" },
                        },
                        Data = await _pensionRateService.ListComponentRates(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                    },
                    Message = $"All Bill Breakups Received Successfully!",
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
    }
}
