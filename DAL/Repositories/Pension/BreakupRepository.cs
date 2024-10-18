using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class BreakupRepository : Repository<Breakup, PensionDbContext>, IBreakupRepository
    {
        public BreakupRepository(PensionDbContext context) : base(context)
        {
        }
    }
}