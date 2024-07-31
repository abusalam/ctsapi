using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionCategoryService : IPensionCategoryService
    {
        private readonly IPrimaryCategoryRepository _primaryCategoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public PensionCategoryService(
                IPrimaryCategoryRepository primaryCategoryRepository,
                ISubCategoryRepository subCategoryRepository,
                ICategoryRepository categoryRepository,
                IMapper mapper
            )
        {
            _primaryCategoryRepository = primaryCategoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<TResponse> CreatePensionPrimaryCategory<TEntry, TResponse>(
                TEntry pensionPrimaryCategoryEntryDTO,
                short financialYear,
                string treasuryCode
            )
        {
            PrimaryCategory primaryCategoryEntity = new() {
                Id = 0
            };
            TResponse? response = _mapper.Map<TResponse>(primaryCategoryEntity);

            try {
                primaryCategoryEntity.FillFrom(pensionPrimaryCategoryEntryDTO);
                primaryCategoryEntity.ActiveFlag = true;
                primaryCategoryEntity.CreatedAt = DateTime.Now;
                
                _primaryCategoryRepository.Add(primaryCategoryEntity);

                if(await _primaryCategoryRepository.SaveChangesManagedAsync() == 0) {
                    response.FillDataSource(
                        primaryCategoryEntity,
                        $"Primary Category not saved!"
                    );
                    return response;
                }
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        primaryCategoryEntity,
                        $"ServiceException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                response.FillFrom(primaryCategoryEntity);
            }
            return response;
        }
    
        public async Task<TResponse> CreatePensionSubCategory<TEntry, TResponse>(
                TEntry pensionSubCategoryEntryDTO,
                short financialYear,
                string treasuryCode
            )
        {
            SubCategory subCategoryEntity = new() {
                Id = 0
            };
            TResponse? response = _mapper.Map<TResponse>(subCategoryEntity);

            try {
                subCategoryEntity.FillFrom(pensionSubCategoryEntryDTO);
                subCategoryEntity.ActiveFlag = true;
                subCategoryEntity.CreatedAt = DateTime.Now;
                
                _subCategoryRepository.Add(subCategoryEntity);

                if(await _subCategoryRepository.SaveChangesManagedAsync() == 0) {
                    response.FillDataSource(
                        subCategoryEntity,
                        $"Sub Category not saved!"
                    );
                    return response;
                }
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        subCategoryEntity,
                        $"ServiceException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                response.FillFrom(subCategoryEntity);
            }
            return response;
        }
    
        public async Task<TResponse> CreatePensionCategory<TEntry, TResponse>(
                TEntry pensionCategoryEntryDTO,
                short financialYear,
                string treasuryCode
            )
        {
            Category categoryEntity = new() {
                Id = 0
            };
            TResponse? response = _mapper.Map<TResponse>(categoryEntity);

            try {
                categoryEntity.FillFrom(pensionCategoryEntryDTO);
                categoryEntity.ActiveFlag = true;
                categoryEntity.CreatedAt = DateTime.Now;

                PrimaryCategory primaryCategoryEntity = await _primaryCategoryRepository.GetSingleAysnc(entity => entity.Id == categoryEntity.PrimaryCategoryId);
                SubCategory subCategoryEntity = await _subCategoryRepository.GetSingleAysnc(entity => entity.Id == categoryEntity.SubCategoryId);

                categoryEntity.CategoryName = primaryCategoryEntity.PrimaryCategoryName + " : " + subCategoryEntity.SubCategoryName;
                
                _categoryRepository.Add(categoryEntity);

                if(await _categoryRepository.SaveChangesManagedAsync() == 0) {
                    response.FillDataSource(
                        categoryEntity,
                        $"Category not saved!"
                    );
                    return response;
                }
 
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        categoryEntity,
                        $"ServiceException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                response.FillFrom(categoryEntity);
            }
            return response;
        }

       public async Task<IEnumerable<TResponse>> ListPensionCategory<TResponse>(
                short financialYear,
                string treasuryCode,
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            return await _categoryRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag,
                    entity => _mapper.Map<TResponse>(entity),
                    dynamicListQueryParameters
                );
        }
    }
}