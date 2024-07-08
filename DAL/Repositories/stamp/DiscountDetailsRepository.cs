using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class DiscountDetailsRepository : Repository<DiscountDetail, CTSDBContext>, IDiscountDetailsRepository
    {
        protected readonly CTSDBContext _context;
        public DiscountDetailsRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<DiscountDetail>()
                .Include(t => t.VendorTypeNavigation);
        }

    }
}
