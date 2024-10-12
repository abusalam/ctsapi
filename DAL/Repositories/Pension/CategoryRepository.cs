using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class CategoryRepository : Repository<Category, PensionDbContext>, ICategoryRepository
    {
        private readonly PensionDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(
            PensionDbContext context,
            IMapper mapper
        ) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Category?> GetCategoryById(
            long categoryId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _context.Categories
                .Where(
                    entity => entity.ActiveFlag
                    && entity.Id == categoryId
                )
                .Include(entity => entity.PrimaryCategory)
                .Include(entity => entity.SubCategory)
                .FirstOrDefaultAsync();
        }

        public async Task<T> CreateCategory<T>(
            short finYear,
            string treasuryCode,
            Category categoryEntity
        )
        {
            T response = _mapper.Map<T>(categoryEntity);
            try {
                Category? categoryExists = await _context.Categories.Where(
                        entity => entity.ActiveFlag
                        && entity.PrimaryCategoryId == categoryEntity.PrimaryCategoryId
                        && entity.SubCategoryId == categoryEntity.SubCategoryId
                    )
                    .FirstOrDefaultAsync();

                if (categoryExists != null) {
                    response.FillDataSource(
                        categoryExists,
                        $"Category already exists!"
                    );
                    return response;
                }

                categoryEntity.ActiveFlag = true;
                categoryEntity.CreatedAt = DateTime.Now;

                PrimaryCategory? primaryCategoryEntity = await _context.PrimaryCategories
                .Where(entity => entity.Id == categoryEntity.PrimaryCategoryId)
                .FirstOrDefaultAsync();

                if (primaryCategoryEntity == null) {
                    response.FillDataSource(
                        categoryExists,
                        $"Primary Category does not exists!"
                    );
                    return response;
                }

                SubCategory? subCategoryEntity = await _context.SubCategories
                .Where(entity => entity.Id == categoryEntity.SubCategoryId)
                .FirstOrDefaultAsync();

                if (subCategoryEntity == null) {
                    response.FillDataSource(
                        categoryExists,
                        $"Sub Category does not exists!"
                    );
                    return response;
                }

                categoryEntity.CategoryName = primaryCategoryEntity?.PrimaryCategoryName + " : " + subCategoryEntity?.SubCategoryName;

                _context.Categories.Add(categoryEntity);


                if(await _context.SaveChangesAsync() == 0) {
                    response.FillDataSource(
                        categoryEntity,
                        "Failed to add Pension Category!"
                    );
                    return response;
                }
                response = _mapper.Map<T>(categoryEntity);
            }
            catch (Exception ex) {
                response.FillDataSource(
                    categoryEntity,
                    ex.InnerException?.Message ?? ex.Message
                );
                return response;
            }
            return response;
        }
    }

}