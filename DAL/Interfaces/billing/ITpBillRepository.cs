using CTS_BE.DAL.Entities;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.billing
{
    public interface ITpBillRepository : IRepository<TpBill>
    {
        public Task<IEnumerable<BillsListDTO>> AllNewBills(string treasuryCode);
        public Task<int> NewBillCount(string treasuryCode);
    }
}