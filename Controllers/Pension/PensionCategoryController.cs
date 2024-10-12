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
            ) : base(claimService)
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

            JsonAPIResponse<PensionPrimaryCategoryResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"PrimaryCategory saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionCategoryService
                    .CreatePensionPrimaryCategory<PensionPrimaryCategoryEntryDTO, PensionPrimaryCategoryResponseDTO>(
                        pensionPrimaryCategoryEntryDTO,
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

        [HttpPatch("primary-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionPrimaryCategoryResponseDTO>>>> GetAllPrimaryCategories(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionPrimaryCategoryResponseDTO>>> response = new();
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
                            Data = await _pensionCategoryService.ListPrimaryCategory<PensionPrimaryCategoryResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionCategoryService.DataCount()
                        },
                    Message = $"All Primary Category Details Received Successfully!"

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

        [HttpPost("sub-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionSubCategoryResponseDTO>> CreateSubCategory(
            PensionSubCategoryEntryDTO pensionSubCategoryEntryDTO
        )
        {

            JsonAPIResponse<PensionSubCategoryResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"SubCategory saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionCategoryService
                    .CreatePensionSubCategory<PensionSubCategoryEntryDTO, PensionSubCategoryResponseDTO>(
                        pensionSubCategoryEntryDTO,
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

        [HttpPatch("sub-category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionSubCategoryResponseDTO>>>> GetAllSubCategories(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionSubCategoryResponseDTO>>> response = new();
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {

                                new() {
                                    Name = "Sub Category ID",
                                    DataType = "text",
                                    FieldName = "id",
                                    FilterField = "id",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Sub Category Name",
                                    DataType = "text",
                                    FieldName = "subCategoryName",
                                    FilterField = "subCategoryName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionCategoryService.ListSubCategory<PensionSubCategoryResponseDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionCategoryService.DataCount()
                        },
                    Message = $"All Sub Category Details Received Successfully!"

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

        [HttpPost("category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionCategoryResponseDTO>> CreateCategory(
                PensionCategoryEntryDTO pensionCategoryEntryDTO
            )
        {

            JsonAPIResponse<PensionCategoryResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Category saved sucessfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionCategoryService
                    .CreatePensionCategory<PensionCategoryEntryDTO, PensionCategoryResponseDTO>(
                        pensionCategoryEntryDTO,
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

        [HttpGet("category/{categoryId}")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<PensionCategoryResponseDTO>> GetCategoryById(
            long categoryId
        )
        {

            JsonAPIResponse<PensionCategoryResponseDTO> response = new(){
                ApiResponseStatus = Enum.APIResponseStatus.Success,
                Message = $"Category received successfully!",
                Result = new(){
                    Id = 0
                }
            };
            try {
                response.Result = await _pensionCategoryService
                    .GetPensionCategoryById<PensionCategoryResponseDTO>(
                        categoryId,
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

        [HttpPatch("category")]
        [Tags("Pension: Category Master")]
        [OpenApi]
        public async Task<JsonAPIResponse<DynamicListResult<IEnumerable<PensionCategoryListDTO>>>> GetAllCategories(
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            JsonAPIResponse<DynamicListResult<IEnumerable<PensionCategoryListDTO>>> response = new();
            try {

                response = new() {

                    ApiResponseStatus = Enum.APIResponseStatus.Success,
                    Result = new()
                        {
                            Headers = new () {

                                new() {
                                    Name = "Category ID",
                                    DataType = "text",
                                    FieldName = "id",
                                    FilterField = "id",
                                    IsFilterable = true,
                                    IsSortable = true,

                                },
                                new() {
                                    Name = "Primary Category ID",
                                    DataType = "text",
                                    FieldName = "primaryCategoryId",
                                    FilterField = "primaryCategoryId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Sub Category ID",
                                    DataType = "text",
                                    FieldName = "subCategoryId",
                                    FilterField = "subCategoryId",
                                    IsFilterable = true,
                                    IsSortable = true,
                                },
                                new() {
                                    Name = "Category Name",
                                    DataType = "text",
                                    FieldName = "categoryName",
                                    FilterField = "categoryName",
                                    IsFilterable = true,
                                    IsSortable = true,
                                }

                            },
                            Data = await _pensionCategoryService.ListPensionCategory<PensionCategoryListDTO>(
                                GetCurrentFyYear(),
                                GetTreasuryCode(),
                                dynamicListQueryParameters),
                            DataCount = _pensionCategoryService.DataCount()
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