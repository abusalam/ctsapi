using CTS_BE.DAL.Entities;

namespace CTS_BE.DAL.Interfaces.stamp
{
    public interface IStampIndentRepository : IRepository<StampIndent>
    {
        public Task<bool> IndentApprove(string RaisedToTreasuryCode, int Quantity);
        public Task<bool> IndentRecieve(string RaisedToTreasuryCode, string RaisedByTreasuryCode, int Quantity, long IndentId);

    }
}
