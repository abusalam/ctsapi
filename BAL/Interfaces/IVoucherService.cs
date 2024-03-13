using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IVoucherService
    {
        public Task<bool> InsertNewVoucher(List<CreateShrtListDTO> createShrtListDTOs, long userId);
    }
}