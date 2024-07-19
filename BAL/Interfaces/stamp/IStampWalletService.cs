using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.stamp
{
    public interface IStampWalletService
    {
        Task<bool> CreateOrUpdateStampWallet(StampWalletInsertDTO stampWallet);
        Task<StampWalletBalanceDTO> GetWalletBalanceByTreasuryCode(string TreasuryCode, long combinationId);
    }
}