using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CTS_BE.Controllers.Pension
{
    public class PensionCategoryController(
        IClaimService claimService,
        IPensionCategoryService pensionCategoryService
    ) : ApiBaseController(claimService)
    {
        private readonly IPensionCategoryService _pensionCategoryService = pensionCategoryService;

        [HttpPost("pension/primary-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionPrimaryCategoryResponseDTO>> CreatePrimaryCategory(
            PensionPrimaryCategoryEntryDTO pensionPrimaryCategoryEntryDTO
        )
        {
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
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
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

        [HttpPost("pension/sub-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionSubCategoryResponseDTO>> CreateSubCategory(
            PensionSubCategoryEntryDTO pensionSubCategoryEntryDTO
        )
        {
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
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
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

        [HttpPost("pension/category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionCategoryResponseDTO>> CreateCategory(
            PensionCategoryEntryDTO pensionCategoryEntryDTO
        )
        {
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
                FillException(response, ex);
                return response;
            }
            finally
            {
                FillErrorMesageFromDataSource(response);
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

        [HttpGet("pension/categories")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>>> GetCategories()
        {
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

        [HttpGet("pension/account-heads")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<AccountHeadListItemResponseDTO>>
        > GetAccountHeads()
        {
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
