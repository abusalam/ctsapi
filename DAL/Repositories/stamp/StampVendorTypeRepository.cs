using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampVendorTypeRepository : Repository<StampVendorType, CTSDBContext>, IStampVendorTypeRepository
    {
        protected readonly CTSDBContext _context;
        public StampVendorTypeRepository(CTSDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
