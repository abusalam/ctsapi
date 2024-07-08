using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampVendorRepository : Repository<StampVendor, CTSDBContext>, IStampVendorRepository
    {
        protected readonly CTSDBContext _context;
        public StampVendorRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<StampVendor>()
                .Include(t => t.VendorTypeNavigation);
        }

    }
}
