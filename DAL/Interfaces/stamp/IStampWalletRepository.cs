using CTS_BE.DAL.Entities;

namespace CTS_BE.DAL.Interfaces.stamp
{
    public interface IStampWalletRepository : IRepository<StampWallet>
    {
        public Task<bool> WalletRefil(string TreasuryCode, short ClearBalance);
    }
}
