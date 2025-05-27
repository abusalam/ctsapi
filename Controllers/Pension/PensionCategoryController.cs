using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Filters;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PensionCategoryController(
        IClaimService claimService,
        IPensionCategoryService pensionCategoryService,
        ILogger<PensionCategoryController> logger
    ) : ApiBaseController(claimService)
    {
        private readonly IPensionCategoryService _pensionCategoryService = pensionCategoryService;
        private readonly ILogger<PensionCategoryController> _logger = logger;

        [HttpPost("pension/primary-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionPrimaryCategoryResponseDTO>> CreatePrimaryCategory(
            PensionPrimaryCategoryEntryDTO pensionPrimaryCategoryEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to create primary category with details: {PensionPrimaryCategoryEntryDTO}",
                pensionPrimaryCategoryEntryDTO
            );
            JsonAPIResponse<PensionPrimaryCategoryResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PrimaryCategory saved sucessfully!",
            };
            try
            {
                response.Result = await _pensionCategoryService.CreatePensionPrimaryCategory<
                    PensionPrimaryCategoryEntryDTO,
                    PensionPrimaryCategoryResponseDTO
                >(pensionPrimaryCategoryEntryDTO, GetCurrentFyYear(), GetTreasuryCode());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating primary category with data: {Data}",
                    pensionPrimaryCategoryEntryDTO
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

        [HttpGet("pension/primary-categories")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionPrimaryCategoryResponseDTO>>
        > GetPrimaryCategories()
        {
            _logger.LogInformation("Received request to get all primary categories.");
            JsonAPIResponse<TableResponseDTO<PensionPrimaryCategoryResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All PrimaryCategories Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "Primary Category ID", FieldName = "id" },
                        new() { Name = "Head of Account", FieldName = "headDetails" },
                        new() { Name = "Primary Category Name", FieldName = "primaryCategoryName" },
                    ],
                    Data =
                        await _pensionCategoryService.GetPrimaryCategories<PensionPrimaryCategoryResponseDTO>(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                };
                _logger.LogInformation("Primary categories retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching primary categories.");
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }
            return response;
        }

        [HttpPost("pension/sub-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionSubCategoryResponseDTO>> CreateSubCategory(
            PensionSubCategoryEntryDTO pensionSubCategoryEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to create sub-category with details: {PensionSubCategoryEntryDTO}",
                pensionSubCategoryEntryDTO
            );
            JsonAPIResponse<PensionSubCategoryResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"SubCategory saved sucessfully!",
            };
            try
            {
                response.Result = await _pensionCategoryService.CreatePensionSubCategory<
                    PensionSubCategoryEntryDTO,
                    PensionSubCategoryResponseDTO
                >(pensionSubCategoryEntryDTO, GetCurrentFyYear(), GetTreasuryCode());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating sub-category with data: {Data}",
                    pensionSubCategoryEntryDTO
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

        [HttpGet("pension/sub-categories")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionSubCategoryResponseDTO>>
        > GetSubCategories()
        {
            _logger.LogInformation("Received request to get all sub-categories.");
            JsonAPIResponse<TableResponseDTO<PensionSubCategoryResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All SubCategories Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "Sub Category ID", FieldName = "id" },
                        new() { Name = "Sub Category Name", FieldName = "subCategoryName" },
                    ],
                    Data =
                        await _pensionCategoryService.GetSubCategories<PensionSubCategoryResponseDTO>(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                };
                _logger.LogInformation("Sub-categories retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching sub-categories.");
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }
            return response;
        }

        [HttpPost("pension/category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionCategoryResponseDTO>> CreateCategory(
            PensionCategoryEntryDTO pensionCategoryEntryDTO
        )
        {
            _logger.LogInformation(
                "Received request to create category with details: {PensionCategoryEntryDTO}",
                pensionCategoryEntryDTO
            );
            JsonAPIResponse<PensionCategoryResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Category saved sucessfully!",
            };
            try
            {
                response.Result = await _pensionCategoryService.CreatePensionCategory<
                    PensionCategoryEntryDTO,
                    PensionCategoryResponseDTO
                >(pensionCategoryEntryDTO, GetCurrentFyYear(), GetTreasuryCode());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating category with data: {Data}",
                    pensionCategoryEntryDTO
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

        [HttpGet("pension/category/{categoryId}")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionCategoryResponseDTO>> GetCategoryById(
            long categoryId
        )
        {
            _logger.LogInformation(
                "Received request to get category by ID: {CategoryId}",
                categoryId
            );
            JsonAPIResponse<PensionCategoryResponseDTO> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Category received successfully!",
            };
            try
            {
                response.Result =
                    await _pensionCategoryService.GetPensionCategoryById<PensionCategoryResponseDTO>(
                        categoryId,
                        GetCurrentFyYear(),
                        GetTreasuryCode()
                    );
                _logger.LogInformation(
                    "Category with ID {CategoryId} retrieved successfully.",
                    categoryId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching category with ID: {CategoryId}",
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

        [HttpGet("pension/categories")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>>> GetCategories()
        {
            _logger.LogInformation("Received request to get all pension categories.");
            JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Pension Categories Received Successfully!",
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
                        await _pensionCategoryService.GetPensionCategories<PensionCategoryListDTO>(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                };
                _logger.LogInformation("Pension categories retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching pension categories.");
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMessageFromDataSource(response);
            }
            return response;
        }

        [HttpGet("pension/account-heads")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<AccountHeadListItemResponseDTO>>
        > GetAccountHeads()
        {
            _logger.LogInformation("Received request to get all account heads.");
            JsonAPIResponse<TableResponseDTO<AccountHeadListItemResponseDTO>> response = new()
            {
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"All Account Heads Received Successfully!",
            };
            try
            {
                response.Result = new()
                {
                    Headers =
                    [
                        new() { Name = "ID", FieldName = "id" },
                        new() { Name = "Head of Account", FieldName = "headDetails" },
                        new() { Name = "Head Description", FieldName = "headDescription" },
                    ],
                    Data =
                        await _pensionCategoryService.GetListOfAccountHeads<AccountHeadListItemResponseDTO>(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                };
                _logger.LogInformation("Account heads retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching account heads.");
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
