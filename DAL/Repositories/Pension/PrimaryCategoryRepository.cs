using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PrimaryCategoryRepository :
        Repository<PrimaryCategory, PensionDbContext>,
        IPrimaryCategoryRepository
    {
        private readonly PensionDbContext _context;

        private readonly IMapper _mapper;

        public PrimaryCategoryRepository(
            IMapper mapper,
            PensionDbContext context
        ) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<T>> GetPrimaryCategoriesAsync<T>()
        {
            return await _context.PrimaryCategories
                .Where(
                    entity => entity.ActiveFlag
                )
                .Select(entity => _mapper.Map<T>(entity))
                .ToListAsync();
        }
    }
}