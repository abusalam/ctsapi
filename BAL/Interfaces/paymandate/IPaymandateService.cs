using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.paymandate
{
    public interface IPaymandateService
    {
        public Task<IEnumerable<PayMandateShortListDTO>> TokenForShortList();
    }
}
