using CTS_BE.DAL.Entities;

namespace CTS_BE.DAL.Interfaces.stamp
{
    public interface IStampWalletRepository : IRepository<StampWallet>
    {
        Task<bool> WalletRefil(string TreasuryCode, long CombinationId, short AddSheet, short AddLabel);
    }
}
