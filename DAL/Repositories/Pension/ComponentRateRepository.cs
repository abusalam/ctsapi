using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ComponentRateRepository(
        PensionDbContext context,
        IMapper mapper,
        ILogger<ComponentRateRepository> logger
    ) : IComponentRateRepository
    {
        private readonly PensionDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ComponentRateRepository> _logger = logger;

        public async Task<List<T>> GetComponentRatesByCategoryId<T>(
            long categoryId,
            Expression<Func<ComponentRate, T>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching component rates for category ID: {CategoryId}",
                categoryId
            );

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
            _logger.LogInformation("Fetching active pension categories with rates.");
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
