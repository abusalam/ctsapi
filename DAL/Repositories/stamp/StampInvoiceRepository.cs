using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampInvoiceRepository : Repository<StampInvoice, CTSDBContext>, IStampInvoiceRepository
    {
        protected readonly CTSDBContext _context;
        public StampInvoiceRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<StampInvoice>()
                .Include(t => t.StampIndent);
        }

    }
}
