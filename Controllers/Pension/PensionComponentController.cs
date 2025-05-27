using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PensionComponentController(
        IPensionBreakupService pensionBreakupService,
        IComponentRateService pensionRateService,
        IClaimService claimService,
        ILogger<PensionComponentController> logger
    ) : ApiBaseController(claimService)
    {
        private readonly IPensionBreakupService _pensionBreakupService = pensionBreakupService;
        private readonly IComponentRateService _pensionRateService = pensionRateService;
        private readonly ILogger<PensionComponentController> _logger = logger;

        [HttpPost("pension-component")]
        [Tags("Pension: Component")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionBreakupResponseDTO>> CreateComponent(
            PensionBreakupEntryDTO pensionBreakupEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to create pension component with data: {PensionBreakupEntryDTO}",
                pensionBreakupEntryDTO
            );
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
                _logger.LogInformation(
                    "Pension component created successfully with ID: {ComponentId}",
                    response.Result.Id
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating pension component with data: {Data}",
                    pensionBreakupEntryDTO
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

        [HttpGet("pension-components")]
        [Tags("Pension: Component")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionBreakupResponseDTO>>
        > GetComponents()
        {
            _logger.LogInformation("Received request to get all pension components.");
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
                _logger.LogInformation("Pension components retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching pension components.");
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
            _logger.LogInformation(
                "Received request to create component rate with data: {PensionRatesEntryDTO}",
                pensionRatesEntryDTO
            );
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
                _logger.LogError(
                    ex,
                    "Error occurred while creating component rate with data: {Data}",
                    pensionRatesEntryDTO
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
                _logger.LogError(
                    ex,
                    "Error occurred while fetching component rates for category ID: {CategoryId}",
                    categoryId
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

        [HttpGet("pension-component/categories")]
        [Tags("Pension: Component Rate")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>>
        > GetCategoriesWithRates()
        {
            _logger.LogInformation(
                "Received request to get all pension categories with component rates."
            );
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
                _logger.LogInformation(
                    "Pension categories with component rates retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching pension categories with component rates."
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
