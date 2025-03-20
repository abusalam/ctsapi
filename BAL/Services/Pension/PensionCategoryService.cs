using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
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
        private readonly PensionDbContext _pensionDbContext;
        private readonly IPrimaryCategoryRepository _primaryCategoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public PensionCategoryService(
            PensionDbContext pensionDbContext,
            IPrimaryCategoryRepository primaryCategoryRepository,
            ISubCategoryRepository subCategoryRepository,
            ICategoryRepository categoryRepository,
            IClaimService claimService,
            IMapper mapper
        )
            : base(claimService)
        {
            _pensionDbContext = pensionDbContext;
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
            PrimaryCategory primaryCategoryEntity = new() { Id = 0 };
            TResponse? response = _mapper.Map<TResponse>(primaryCategoryEntity);

            try
            {
                primaryCategoryEntity.FillFrom(pensionPrimaryCategoryEntryDTO);
                var PrimaryCategoryExists = await _primaryCategoryRepository.PrimaryCategoryExists(
                    primaryCategoryEntity.PrimaryCategoryName
                );

                if (PrimaryCategoryExists)
                {
                    response.FillErrorInDataSource(
                        primaryCategoryEntity,
                        $"Primary Category '{primaryCategoryEntity.PrimaryCategoryName}' already exists!"
                    );
                    return response;
                }

                SetCreatedBy(primaryCategoryEntity);
                response = await _primaryCategoryRepository.SavePrimaryCategoryAsync<TResponse>(
                    primaryCategoryEntity
                );
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    primaryCategoryEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            finally
            {
                PrimaryCategory? primaryCategory =
                    await _primaryCategoryRepository.GetPrimaryCategoryWithAccountHeadAsync(
                        primaryCategoryEntity.Id
                    );
                response = _mapper.Map<TResponse>(primaryCategory);
            }
            return response;
        }

        public async Task<List<T>> GetPrimaryCategories<T>(short financialYear, string treasuryCode)
        {
            var primaryCategories = new List<PrimaryCategory>();
            var response = new List<T>();
            try
            {
                primaryCategories = await _primaryCategoryRepository.GetPrimaryCategoriesAsync();
                response = _mapper.Map<List<T>>(primaryCategories);
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    primaryCategories,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return response;
        }

        public async Task<TResponse> CreatePensionSubCategory<TEntry, TResponse>(
            TEntry pensionSubCategoryEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            SubCategory subCategoryEntity = new() { Id = 0 };
            TResponse? response = _mapper.Map<TResponse>(subCategoryEntity);

            try
            {
                subCategoryEntity.FillFrom(pensionSubCategoryEntryDTO);

                var subCategory = await _pensionDbContext.SubCategories.FirstOrDefaultAsync(
                    entity =>
                        entity.ActiveFlag
                        && entity.SubCategoryName == subCategoryEntity.SubCategoryName
                );

                if (subCategory != null)
                {
                    response.FillErrorInDataSource(
                        subCategoryEntity,
                        $"Sub Category already exists!"
                    );
                    return response;
                }

                subCategoryEntity.ActiveFlag = true;
                subCategoryEntity.CreatedAt = DateTime.Now;

                await _pensionDbContext.SubCategories.AddAsync(subCategoryEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    response.FillErrorInDataSource(subCategoryEntity, $"Sub Category not saved!");
                    return response;
                }
            }
            catch (DbUpdateException ex)
            {
                response.FillErrorInDataSource(
                    subCategoryEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            finally
            {
                response.FillFrom(subCategoryEntity);
            }
            return response;
        }

        public async Task<List<PensionSubCategoryResponseDTO>> ListSubCategory(
            short financialYear,
            string treasuryCode
        )
        {
            // _dataCount = await _pensionDbContext.SubCategories.CountAsync();
            return await _pensionDbContext
                .SubCategories.Where(entity => entity.ActiveFlag)
                .Select(entity => _mapper.Map<PensionSubCategoryResponseDTO>(entity))
                .ToListAsync();
        }

        public async Task<List<TResponse>> GetSubCategories<TResponse>(
            short financialYear,
            string treasuryCode
        )
        {
            return await _subCategoryRepository.GetSubCategoriesAsync<TResponse>();
        }

        public async Task<TResponse> CreatePensionCategory<TEntry, TResponse>(
            TEntry pensionCategoryEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            Category categoryEntity = new();
            TResponse? response = _mapper.Map<TResponse>(categoryEntity);
            try
            {
                categoryEntity.FillFrom(pensionCategoryEntryDTO);

                var categoryExists = await _categoryRepository.CategoryExists(categoryEntity);

                if (categoryExists)
                {
                    response.FillErrorInDataSource(categoryEntity, $"Category already exists!");
                    return response;
                }

                PrimaryCategory? primaryCategoryEntity =
                    await _primaryCategoryRepository.GetPrimaryCategoryById(
                        categoryEntity.PrimaryCategoryId
                    );

                if (primaryCategoryEntity == null)
                {
                    response.FillErrorInDataSource(
                        categoryEntity,
                        $"Primary Category does not exists!"
                    );
                    return response;
                }

                SubCategory? subCategoryEntity = await _subCategoryRepository.GetSubCategoryById(
                    categoryEntity.SubCategoryId
                );

                if (subCategoryEntity == null)
                {
                    response.FillErrorInDataSource(
                        categoryEntity,
                        $"Sub Category does not exists!"
                    );
                    return response;
                }

                categoryEntity.CategoryName =
                    primaryCategoryEntity?.PrimaryCategoryName
                    + " : "
                    + subCategoryEntity?.SubCategoryName;

                SetCreatedBy(categoryEntity);

                response = await _categoryRepository.CreateCategory<TResponse>(categoryEntity);
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    categoryEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            return response;
        }

        public async Task<T> GetPensionCategoryById<T>(
            long categoryId,
            short financialYear,
            string treasuryCode
        )
        {
            Category? pensionCategoryEntity = new();
            T PensionCategoryDTO = _mapper.Map<T>(pensionCategoryEntity);
            try
            {
                pensionCategoryEntity = await _categoryRepository.GetCategoryById(
                    categoryId,
                    financialYear,
                    treasuryCode
                );
                if (pensionCategoryEntity == null)
                {
                    PensionCategoryDTO.FillErrorInDataSource(
                        new
                        {
                            categoryId,
                            financialYear,
                            treasuryCode,
                        },
                        $"Category not found! Please check category id: {categoryId}"
                    );
                    return PensionCategoryDTO;
                }
                PensionCategoryDTO = _mapper.Map<T>(pensionCategoryEntity);
                return PensionCategoryDTO;
            }
            catch (DbUpdateException ex)
            {
                PensionCategoryDTO.FillErrorInDataSource(
                    _mapper.Map<T>(pensionCategoryEntity),
                    $"DbException: {ex.InnerException?.Message} {this.ToString()}"
                );
                return PensionCategoryDTO;
            }
            catch (Exception ex)
            {
                PensionCategoryDTO.FillErrorInDataSource(
                    _mapper.Map<T>(pensionCategoryEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message} {this.ToString()}"
                );
                return PensionCategoryDTO;
            }
        }

        public async Task<List<PensionCategoryListDTO>> ListPensionCategory(
            short financialYear,
            string treasuryCode
        )
        {
            // _dataCount = await _pensionDbContext.Categories.CountAsync();
            return await _pensionDbContext
                .Categories.Where(entity => entity.ActiveFlag)
                .Select(entity => _mapper.Map<PensionCategoryListDTO>(entity))
                .ToListAsync();
        }

        public async Task<List<TResponse>> GetPensionCategories<TResponse>(
            short financialYear,
            string treasuryCode
        )
        {
            return await _categoryRepository.GetPensionCategoriesAsync<TResponse>();
        }

        public async Task<List<AccountHeadListItemResponseDTO>> GetListOfAccountHeads<T>(
            short financialYear,
            string treasuryCode
        )
        {
            return await _primaryCategoryRepository.GetAccountHeadsAsync(
                financialYear,
                treasuryCode
            );
        }
    }
}
