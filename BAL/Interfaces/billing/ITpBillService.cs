using CTS_BE.DTOs;
namespace CTS_BE.BAL.Interfaces.billing
{
    public interface ITpBillService
    {
        public Task<IEnumerable<BillsListDTO>> Bills();
        public Task<IEnumerable<BillsListDTO>> NewBills();
        public Task<int> CountNewBills();
        public Task<BillDetailsDetailsByRef> BillDetailsByRefNo(string refNo);
        public Task<BillDetailsDetailsByRef> BillDetailsByBillId(long billId);
        public Task<int> BillCountByStatus(int statusCode);
    }
}