using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stampRequisition;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.stampRequisition
{
    public class StampRequisitionRepository : Repository<VendorStampRequisition, CTSDBContext>, IStampRequisitionRepository
    {
        protected readonly CTSDBContext _context;
        public StampRequisitionRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<VendorStampRequisition>()
                .Include(t => t.Vendor)
                .Include(t => t.VendorRequisitionApprove)
                .Include(t => t.Vendor);
        }
    }
}