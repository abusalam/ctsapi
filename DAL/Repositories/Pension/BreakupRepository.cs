using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class BreakupRepository : Repository<Breakup, PensionDbContext>, IBreakupRepository
    {
        private readonly PensionDbContext _context;
        private readonly IMapper _mapper;
        public BreakupRepository(
            IMapper mapper,
            PensionDbContext context
        ) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<T>> GetBreakupsAsync<T>(
            Expression<Func<Breakup, T>> selectExpression
        )
        {
            return await _context.Breakups
                .Select(selectExpression)
                .ToListAsync();
        }
    }
}