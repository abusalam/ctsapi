using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ComponentRateRepository(PensionDbContext context, IMapper mapper)
        : IComponentRateRepository
    {
        private readonly PensionDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<T>> GetComponentRatesByCategoryId<T>(
            long categoryId,
            Expression<Func<ComponentRate, T>> selectExpression
        )
        {
            return await _context
                .ComponentRates.Where(entity =>
                    entity.ActiveFlag && entity.CategoryId == categoryId
                )
                .Include(entity => entity.Breakup)
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<List<T>> GetPensionCategoriesWithRatesAsync<T>()
        {
            return await _context
                .Categories.Where(entity =>
                    entity.ActiveFlag
                    && _context.ComponentRates.Any(cr => cr.CategoryId == entity.Id)
                )
                .Select(entity => _mapper.Map<T>(entity))
                .ToListAsync();
        }
    }
}
