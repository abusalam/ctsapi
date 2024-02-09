using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface ITokenHasObjectionService
    {
        public Task<bool> Insert(BillCheckingDto billCheckingDto,long userId, int ownType);
        public Task<List<TokenWithObjectionDto>> ObjectionByTokenId(long tokenId);
    }
}