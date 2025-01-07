using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class TreasuryRepository(
        PensionDbContext context
    ) : Repository<SubCategory, PensionDbContext>(context), ITreasuryRepository
    {
        private readonly PensionDbContext _context = context;

        public async Task<string> GetTreasuryNameAsync(string treasuryCode)
        {
            return await _context.Treasuries.
            Where(
                entity => entity.ActiveFlag
                && entity.TreasuryCode == treasuryCode
            )
            .Select(entity => entity.TreasuryName)
            .FirstOrDefaultAsync() ?? "--";
        }
    }
}