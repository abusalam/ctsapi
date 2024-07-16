using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ManualPpoReceiptRepository : 
        Repository<PpoReceipt, PensionDbContext>,
        IManualPpoReceiptRepository
    {
        protected readonly PensionDbContext _context;
        public ManualPpoReceiptRepository(PensionDbContext context) : 
            base(context)
        {
            _context = context;
        }

    }
}