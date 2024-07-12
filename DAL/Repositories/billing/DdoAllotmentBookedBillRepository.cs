using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.billing;

namespace CTS_BE.DAL.Repositories.billing
{
    public class DdoAllotmentBookedBillRepository : Repository<DdoAllotmentBookedBill, CTSDBContext>, IDdoAllotmentBookedBillRepository
    {
        public DdoAllotmentBookedBillRepository(CTSDBContext context) : base(context)
        {
        }
    }
}