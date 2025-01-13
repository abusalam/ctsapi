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
    public class PensionCategoryController : ApiBaseController
    {
        private readonly IPensionCategoryService _pensionCategoryService;

        public PensionCategoryController(
            IClaimService claimService,
            IPensionCategoryService pensionCategoryService
        )
            : base(claimService)
        {
            _pensionCategoryService = pensionCategoryService;
        }

        [HttpPost("primary-category")]
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
                Result = new() { Id = 0 },
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

        [HttpGet("primary-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionPrimaryCategoryResponseDTO>>
        > GetPrimaryCategories()
        {
            JsonAPIResponse<TableResponseDTO<PensionPrimaryCategoryResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Primary Category ID", FieldName = "id" },
                            new() { Name = "Head of Account", FieldName = "headDetails" },
                            new()
                            {
                                Name = "Primary Category Name",
                                FieldName = "primaryCategoryName",
                            },
                        },
                        Data = await _pensionCategoryService.GetPrimaryCategories(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                    },
                    Message = $"All Primary Category Details Received Successfully!",
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

        [HttpPatch("primary-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        [Obsolete("Use GetPrimaryCategories instead")]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionPrimaryCategoryResponseDTO>>
        > GetAllPrimaryCategories()
        {
            JsonAPIResponse<TableResponseDTO<PensionPrimaryCategoryResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Primary Category ID", FieldName = "id" },
                            new() { Name = "Head of Account", FieldName = "hoaId" },
                            new()
                            {
                                Name = "Primary Category Name",
                                FieldName = "primaryCategoryName",
                            },
                        },
                        Data = await _pensionCategoryService.ListPrimaryCategory(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                    },
                    Message = $"All Primary Category Details Received Successfully!",
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

        [HttpPost("sub-category")]
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
                Result = new() { Id = 0 },
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

        [HttpPatch("sub-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        [Obsolete("Use GetSubCategories instead")]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionSubCategoryResponseDTO>>
        > GetAllSubCategories()
        {
            JsonAPIResponse<TableResponseDTO<PensionSubCategoryResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Sub Category ID", FieldName = "id" },
                            new() { Name = "Sub Category Name", FieldName = "subCategoryName" },
                        },
                        Data = await _pensionCategoryService.ListSubCategory(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                    },
                    Message = $"All Sub Category Details Received Successfully!",
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

        [HttpGet("sub-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionSubCategoryResponseDTO>>
        > GetSubCategories()
        {
            JsonAPIResponse<TableResponseDTO<PensionSubCategoryResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Sub Category ID", FieldName = "id" },
                            new() { Name = "Sub Category Name", FieldName = "subCategoryName" },
                        },
                        Data =
                            await _pensionCategoryService.GetSubCategories<PensionSubCategoryResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode()
                            ),
                    },
                    Message = $"All Sub Category Details Received Successfully!",
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

        [HttpPost("category")]
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
                Result = new() { Id = 0 },
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

        [HttpGet("category/{categoryId}")]
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
                Result = new() { Id = 0 },
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

        [HttpPatch("category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        [Obsolete("Use GetCategories instead")]
        public async Task<
            JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>>
        > GetAllCategories()
        {
            JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Category ID", FieldName = "id" },
                            new() { Name = "Primary Category ID", FieldName = "primaryCategoryId" },
                            new() { Name = "Sub Category ID", FieldName = "subCategoryId" },
                            new() { Name = "Category Name", FieldName = "categoryName" },
                        },
                        Data = await _pensionCategoryService.ListPensionCategory(
                            GetCurrentFyYear(),
                            GetTreasuryCode()
                        ),
                    },
                    Message = $"All PPO Details Received Successfully!",
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

        [HttpGet("category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>>> GetCategories()
        {
            JsonAPIResponse<TableResponseDTO<PensionCategoryListDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "Category ID", FieldName = "id" },
                            new() { Name = "Primary Category ID", FieldName = "primaryCategoryId" },
                            new() { Name = "Sub Category ID", FieldName = "subCategoryId" },
                            new() { Name = "Category Name", FieldName = "categoryName" },
                        },
                        Data =
                            await _pensionCategoryService.GetPensionCategories<PensionCategoryListDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode()
                            ),
                    },
                    Message = $"All PPO Details Received Successfully!",
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

        [HttpGet("account-heads")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<
            JsonAPIResponse<TableResponseDTO<AccountHeadListItemResponseDTO>>
        > GetAccountHeads()
        {
            JsonAPIResponse<TableResponseDTO<AccountHeadListItemResponseDTO>> response = new();
            try
            {
                response = new()
                {
                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                    {
                        Headers = new()
                        {
                            new() { Name = "ID", FieldName = "id" },
                            new() { Name = "Head of Account", FieldName = "headDetails" },
                            new() { Name = "Head Description", FieldName = "headDescription" },
                        },
                        Data =
                            await _pensionCategoryService.GetListOfAccountHeads<AccountHeadListItemResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode()
                            ),
                    },
                    Message = $"All Head Details Received Successfully!",
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
