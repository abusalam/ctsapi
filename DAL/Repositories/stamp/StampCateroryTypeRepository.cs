using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampCateroryTypeRepository : Repository<CategoryType, CTSDBContext>, IStampCategoryTypeRepository
    {
        protected readonly CTSDBContext _context;
        public StampCateroryTypeRepository(CTSDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
