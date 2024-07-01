using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.stamp
{
    public interface IStampWalletService
    {
        Task<bool> CreateOrUpdateStampWallet(StampWalletInsertDTO stampWallet);
        Task<short> GetWalletBalanceByTreasuryCode(String TreasuryCode);
    }
}