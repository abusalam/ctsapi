using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stampRequisition;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL.Repositories.stampRequisition
{
    public class StampRequisitionStagingRepository : Repository<VendorRequisitionStaging, CTSDBContext>, IStampRequisitionStagingRepository
    {
        protected readonly CTSDBContext _context;
        public StampRequisitionStagingRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<VendorRequisitionStaging>()
            //    .Include(t => t.Vendor)
                .Include(t => t.VendorRequisition);
            //    .Include(t => t.Vendor);
        }

        
    }
}