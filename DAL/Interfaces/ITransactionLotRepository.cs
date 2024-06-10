using CTS_BE.DAL.Entities;
namespace CTS_BE.DAL.Interfaces
{
    public interface ITransactionLotRepository : IRepository<TransactionLot>
    {
        public Task<bool> NewLot(long userId);
    }
}