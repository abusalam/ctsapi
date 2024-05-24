using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampIndentRepository : Repository<StampIndent, CTSDBContext>, IStampIndentRepository
    {
        protected readonly CTSDBContext _context;
        public StampIndentRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<StampIndent>()
                .Include(t => t.StampCombination)
                .Include(t => t.RaisedToTreasuryNavigation)
                .Include(t => t.RaisedByTreasuryNavigation);
        }

    }
}
