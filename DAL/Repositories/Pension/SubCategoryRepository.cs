using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class SubCategoryRepository
        : Repository<SubCategory, PensionDbContext>,
            ISubCategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly PensionDbContext _context;

        public SubCategoryRepository(IMapper mapper, PensionDbContext context)
            : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SubCategory?> GetSubCategoryById(long subCategoryId)
        {
            return await _context
                .SubCategories.Where(entity => entity.ActiveFlag && entity.Id == subCategoryId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetSubCategoriesAsync<T>()
        {
            return await _context
                .SubCategories.Where(entity => entity.ActiveFlag)
                .Select(entity => _mapper.Map<T>(entity))
                .ToListAsync();
        }
    }
}
