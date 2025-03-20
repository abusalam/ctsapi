using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PensionComponentController(
        IPensionBreakupService pensionBreakupService,
        IComponentRateService pensionRateService,
        IClaimService claimService
    ) : ApiBaseController(claimService)
    {
        private readonly IPensionBreakupService _pensionBreakupService = pensionBreakupService;
        private readonly IComponentRateService _pensionRateService = pensionRateService;

        [HttpPost("pension-component")]
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
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [HttpGet("pension-components")]
        [Tags("Pension: Component")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionBreakupResponseDTO>>
        > GetComponents()
        {
            JsonAPIResponse<TableResponseDTO<PensionBreakupResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Components/Breakups Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "Component ID", FieldName = "id" },
                        new() { Name = "Component Name", FieldName = "componentName" },
                        new() { Name = "Component Type", FieldName = "componentType" },
                        new() { Name = "Relief Allowed", FieldName = "reliefFlag" },
                    ],
                    Data = await _pensionBreakupService.GetBreakups<PensionBreakupResponseDTO>(
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

        [HttpPost("pension-component/rate")]
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
                FillErrorMessageFromDataSource(response);
            }

            return response;
        }

        [HttpGet("pension-component/{categoryId}/rates")]
        [Tags("Pension: Component Rate")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<ComponentRateResponseDTO>>
        > GetComponentRatesByCategoryId(long categoryId)
        {
            JsonAPIResponse<TableResponseDTO<ComponentRateResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Component Rates Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "Rate ID", FieldName = "id" },
                        new() { Name = "Breakup", FieldName = "componentName" },
                        new() { Name = "Effective From Date", FieldName = "withEffectFrom" },
                        new() { Name = "Rate", FieldName = "componentRate" },
                        new() { Name = "Type", FieldName = "componentType" },
                    ],
                    Data =
                        await _pensionRateService.ListComponentRatesByCategoryId<ComponentRateResponseDTO>(
                            categoryId
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

        [HttpGet("pension-component/categories")]
        [Tags("Pension: Component Rate")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>>
        > GetCategoriesWithRates()
        {
            JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Pension Categories With Component Rates Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "Category ID", FieldName = "id" },
                        new() { Name = "Primary Category ID", FieldName = "primaryCategoryId" },
                        new() { Name = "Sub Category ID", FieldName = "subCategoryId" },
                        new() { Name = "Category Name", FieldName = "categoryName" },
                    ],
                    Data =
                        await _pensionRateService.GetPensionCategoriesWithRates<PensionCategoryListDTO>(
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
