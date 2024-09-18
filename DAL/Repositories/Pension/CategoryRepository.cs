using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class CategoryRepository : Repository<Category, PensionDbContext>, ICategoryRepository
    {
        private readonly PensionDbContext _context;
        public CategoryRepository(
            PensionDbContext context
        ) : base(context)
        {
            _context = context;
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
    }

}