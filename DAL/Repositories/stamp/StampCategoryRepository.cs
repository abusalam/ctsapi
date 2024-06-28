using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampCategoryRepository : Repository<StampCategory, CTSDBContext>, IStampCategoryRepository
    {
        protected readonly CTSDBContext _context;
        public StampCategoryRepository(CTSDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
