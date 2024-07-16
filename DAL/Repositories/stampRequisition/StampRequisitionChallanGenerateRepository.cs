using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stampRequisition;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL.Repositories.stampRequisition
{
    public class StampRequisitionChallanGenerateRepository : Repository<VendorRequisitionChallanGenerate, CTSDBContext>, IStampRequisitionChallanGenerateRepository
    {
        protected readonly CTSDBContext _context;
        public StampRequisitionChallanGenerateRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<VendorRequisitionChallanGenerate>()
            //    .Include(t => t.Vendor)
            //    .Include(t => t.VendorRequisitionApprove)
                .Include(t => t.VendorRequisitionStaging);
        }

        
    }
}