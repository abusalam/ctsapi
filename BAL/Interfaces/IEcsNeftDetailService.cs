using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IEcsNeftDetailService
    {
        Task<ECSNEFT> ECSByBillId(long billId);
        Task<int> countBeneficiariesByBillId(long billId);
    }
}