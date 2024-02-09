using CTS_BE.DAL.Entities;

namespace CTS_BE.DAL.Interfaces
{
    public interface ITokenRepository : IRepository<Token>
    {
        public Task<int> GenarateToken(long billid, long userid, string remarks, DateOnly phybilldate);
        public Task<bool> GenerateReturnMemo(long tokenId, string referenceNo, long userId, int ownType);
    }
}