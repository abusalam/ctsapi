using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionCategoryService : BaseService, IPensionCategoryService
    {
        private readonly IPrimaryCategoryRepository _primaryCategoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public PensionCategoryService(
                IPrimaryCategoryRepository primaryCategoryRepository,
                ISubCategoryRepository subCategoryRepository,
                ICategoryRepository categoryRepository,
                IClaimService claimService,
                IMapper mapper
            ) : base(claimService)
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

                var primaryCategory = await _primaryCategoryRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag
                        && entity.PrimaryCategoryName == primaryCategoryEntity.PrimaryCategoryName
                    );
                if (primaryCategory != null) {
                    response.FillDataSource(
                        primaryCategoryEntity,
                        $"Primary Category already exists!"
                    );
                    return response;
                }

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

        public async Task<IEnumerable<TResponse>> ListPrimaryCategory<TResponse>(
                short financialYear,
                string treasuryCode,
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            _dataCount = _primaryCategoryRepository.Count();
            return await _primaryCategoryRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag,
                    entity => _mapper.Map<TResponse>(entity),
                    dynamicListQueryParameters
                );
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

                var subCategory = await _subCategoryRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag
                        && entity.SubCategoryName == subCategoryEntity.SubCategoryName
                    );
                if (subCategory != null) {
                    response.FillDataSource(
                        subCategoryEntity,
                        $"Sub Category already exists!"
                    );
                    return response;
                }

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

       public async Task<IEnumerable<TResponse>> ListSubCategory<TResponse>(
                short financialYear,
                string treasuryCode,
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            _dataCount = _subCategoryRepository.Count();
            return await _subCategoryRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag,
                    entity => _mapper.Map<TResponse>(entity),
                    dynamicListQueryParameters
                );
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
                SetCreatedBy(categoryEntity);
                response = await _categoryRepository.CreateCategory<TResponse>(
                    financialYear,
                    treasuryCode,
                    categoryEntity
                );
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        categoryEntity,
                        $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                    );
            }
            return response;
        }


        public async Task<T> GetPensionCategoryById<T>(
            long categoryId,
            short financialYear,
            string treasuryCode
        )
        {
            Category? pensionCategoryEntity = new ();
            T PensionCategoryDTO = _mapper.Map<T>(pensionCategoryEntity);
            try {

                pensionCategoryEntity = await _categoryRepository.GetCategoryById(
                    categoryId,
                    financialYear,
                    treasuryCode
                );
                if(pensionCategoryEntity == null) {
                    PensionCategoryDTO.FillDataSource(
                        pensionCategoryEntity,
                        $"Category not found! Please check category id: {categoryId}"
                    );
                    return PensionCategoryDTO;
                }
                PensionCategoryDTO = _mapper.Map<T>(pensionCategoryEntity);
                return PensionCategoryDTO;
            }
            catch (DbUpdateException ex) {
                PensionCategoryDTO.FillDataSource(
                    _mapper.Map<T>(pensionCategoryEntity),
                    $"DbUpdateException: {ex.InnerException?.Message} {this.ToString()}"
                );
                return PensionCategoryDTO;
            }
            catch (Exception ex) {
                PensionCategoryDTO.FillDataSource(
                    _mapper.Map<T>(pensionCategoryEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message} {this.ToString()}"
                );
                return PensionCategoryDTO;
            }
        }

       public async Task<IEnumerable<TResponse>> ListPensionCategory<TResponse>(
                short financialYear,
                string treasuryCode,
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            _dataCount = _categoryRepository.Count();
            return await _categoryRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag,
                    entity => _mapper.Map<TResponse>(entity),
                    dynamicListQueryParameters
                );
        }

    }
}