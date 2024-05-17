using CTS_BE.DAL.Entities;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces
{
    public interface ITokenRepository : IRepository<Token>
    {
        public Task<GeneratedTokenDTO> GenarateToken(long billid, long userid, string remarks, DateOnly phybilldate);
        public Task<bool> GenerateReturnMemo(long tokenId, string referenceNo, long userId, int ownType);
        public Task<bool> PaymandateShortList(long loggedinUserId, string paymentData);
    }
}