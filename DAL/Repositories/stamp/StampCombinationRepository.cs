using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampCombinationRepository : Repository<StampCombination, CTSDBContext>, IStampCombinationRepository
    {
        protected readonly CTSDBContext _context;
        public StampCombinationRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<StampCombination>()
                .Include(t => t.StampCategory)
                .Include(t => t.StampDenomination)
                .Include(t => t.StampType)
                .Include(t => t.StampLabel);
        }

    }
}
