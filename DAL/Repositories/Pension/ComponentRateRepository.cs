using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ComponentRateRepository : Repository<ComponentRate, PensionDbContext>, IComponentRateRepository
    {
        private readonly PensionDbContext _context;
        public ComponentRateRepository(
            PensionDbContext context
        ) : base(context)
        {
            _context = context;
        }

        public async Task<List<T>> GetComponentRatesByCategoryId<T>(
            long categoryId,
            Expression<Func<ComponentRate, T>> selectExpression

        )
        {
            return await _context.ComponentRates
                .Where(
                    entity => entity.ActiveFlag
                    && entity.CategoryId == categoryId
                )
                .Include(entity => entity.Breakup)
                .Select(selectExpression)
                .ToListAsync();
        }
    }
}