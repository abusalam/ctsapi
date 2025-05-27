using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class CategoryRepository(
        PensionDbContext context,
        IMapper mapper,
        ILogger<CategoryRepository> logger
    ) : ICategoryRepository
    {
        private readonly PensionDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CategoryRepository> _logger = logger;

        public async Task<List<T>> GetPensionCategoriesAsync<T>()
        {
            _logger.LogInformation("Fetching active pension categories from the database.");
            return await _context
                .Categories.Where(entity => entity.ActiveFlag)
                .Select(entity => _mapper.Map<T>(entity))
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryById(
            long categoryId,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Fetching category with ID {CategoryId} for financial year {FinancialYear} and treasury code {TreasuryCode}.",
                categoryId,
                financialYear,
                treasuryCode
            );
            return await _context
                .Categories.Where(entity => entity.ActiveFlag && entity.Id == categoryId)
                .Include(entity => entity.PrimaryCategory)
                .ThenInclude(entity => entity.AccountHead)
                .Include(entity => entity.SubCategory)
                // .AsSplitQuery()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CategoryExists(Category categoryEntity)
        {
            _logger.LogInformation(
                "Checking if category exists with PrimaryCategoryId {PrimaryCategoryId} and SubCategoryId {SubCategoryId}.",
                categoryEntity.PrimaryCategoryId,
                categoryEntity.SubCategoryId
            );
            return await _context.Categories.AnyAsync(entity =>
                entity.ActiveFlag
                && entity.PrimaryCategoryId == categoryEntity.PrimaryCategoryId
                && entity.SubCategoryId == categoryEntity.SubCategoryId
            );
        }

        public async Task<T> CreateCategory<T>(Category categoryEntity)
        {
            _logger.LogInformation(
                "Creating new category with PrimaryCategoryId {PrimaryCategoryId} and SubCategoryId {SubCategoryId}.",
                categoryEntity.PrimaryCategoryId,
                categoryEntity.SubCategoryId
            );
            T response = _mapper.Map<T>(categoryEntity);
            try
            {
                _context.Categories.Add(categoryEntity);

                if (await _context.SaveChangesAsync() == 0)
                {
                    _logger.LogError(
                        "Failed to save new category with PrimaryCategoryId {PrimaryCategoryId} and SubCategoryId {SubCategoryId}.",
                        categoryEntity.PrimaryCategoryId,
                        categoryEntity.SubCategoryId
                    );
                    response.FillErrorInDataSource(
                        categoryEntity,
                        "Failed to add Pension Category!"
                    );
                    return response;
                }
                _logger.LogInformation(
                    "Successfully created new category with PrimaryCategoryId {PrimaryCategoryId} and SubCategoryId {SubCategoryId}.",
                    categoryEntity.PrimaryCategoryId,
                    categoryEntity.SubCategoryId
                );
                response = _mapper.Map<T>(categoryEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating category with PrimaryCategoryId {PrimaryCategoryId} and SubCategoryId {SubCategoryId}.",
                    categoryEntity.PrimaryCategoryId,
                    categoryEntity.SubCategoryId
                );
                response.FillErrorInDataSource(
                    categoryEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            return response;
        }
    }
}
