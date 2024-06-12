using CTS_BE.Model;
using CTS_BE.Model.e_Kuber;

namespace CTS_BE.BAL.Interfaces
{
    public interface ITransactionLotService
    {
        public Task<bool> CreateLot(long userId);
        public Task<List<TransactionLotModel>> pendingLots();
        public Task<EKuber> GetXMLData(long lotId,string FileSequenceNumber,string paymentDate);
    }
}