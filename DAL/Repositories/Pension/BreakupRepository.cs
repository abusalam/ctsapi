using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class BreakupRepository(IMapper mapper, PensionDbContext context) : IBreakupRepository
    {
        private readonly PensionDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<T>> GetBreakupsAsync<T>(
            Expression<Func<Breakup, T>> selectExpression
        )
        {
            return await _context.Breakups.Select(selectExpression).ToListAsync();
        }
    }
}
