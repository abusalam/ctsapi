using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class FinancialYearRepository(PensionDbContext context) : IFinancialYearRepository
    {
        private readonly PensionDbContext _context = context;

        public async Task<short> GetCurrentFinancialYearAsync()
        {
            return (short)
                await _context
                    .FinancialYears.Where(fy => fy.ActiveFlag)
                    .Select(fy => fy.CurrentYear)
                    .FirstOrDefaultAsync();
        }
    }
}
