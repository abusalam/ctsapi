using CTS_BE.DAL.Entities;
namespace CTS_BE.DAL.Interfaces
{
    public interface IVoucherRepository: IRepository<Voucher>
    {
        public Task<bool> NewVoucher(string payload, long userId);
    }
}