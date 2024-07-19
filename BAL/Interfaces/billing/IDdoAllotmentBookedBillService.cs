using CTS_BE.DTOs;
namespace CTS_BE.BAL.Interfaces.billing
{
    public interface IDdoAllotmentBookedBillService
    {
        Task<IEnumerable<AllotmentDTO>> AllotmentDetailsByBillId(long billId);
    }
}