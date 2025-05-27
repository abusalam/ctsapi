using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class BreakupRepository(PensionDbContext context, ILogger<BreakupRepository> logger)
        : IBreakupRepository
    {
        private readonly PensionDbContext _context = context;

        private readonly ILogger<BreakupRepository> _logger = logger;

        public async Task<List<T>> GetBreakupsAsync<T>(
            Expression<Func<Breakup, T>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching breakups with select expression: {SelectExpression}",
                selectExpression
            );
            return await _context.Breakups.Select(selectExpression).ToListAsync();
        }
    }
}
