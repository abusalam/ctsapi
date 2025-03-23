using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class CategoryRepository(PensionDbContext context, IMapper mapper) : ICategoryRepository
    {
        private readonly PensionDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<T>> GetPensionCategoriesAsync<T>()
        {
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
            return await _context.Categories.AnyAsync(entity =>
                entity.ActiveFlag
                && entity.PrimaryCategoryId == categoryEntity.PrimaryCategoryId
                && entity.SubCategoryId == categoryEntity.SubCategoryId
            );
        }

        public async Task<T> CreateCategory<T>(Category categoryEntity)
        {
            T response = _mapper.Map<T>(categoryEntity);
            try
            {
                _context.Categories.Add(categoryEntity);

                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillErrorInDataSource(
                        categoryEntity,
                        "Failed to add Pension Category!"
                    );
                    return response;
                }
                response = _mapper.Map<T>(categoryEntity);
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
    }
}
