using CTS_BE.DAL.Entities;

namespace CTS_BE.DAL.Interfaces.stamp
{
    public interface IStampIndentRepository : IRepository<StampIndent>
    {
        public Task<bool> IndentApprove(string RaisedToTreasuryCode, short sheet, short label, long combinationId);
        public Task<bool> IndentRecieve(short sheet, short label, long IndentId);

    }
}
