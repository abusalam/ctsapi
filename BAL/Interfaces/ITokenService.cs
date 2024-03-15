using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface ITokenService
    {
        public Task<int> InsterNewToken(TokenDTO tokenDTO, long userId);
        public Task<TokenDetailsDto> TokenDeatisById(long tokenId);
        public Task<IEnumerable<TokenList>> AllTokens(string treasuryCode);
        public Task<IEnumerable<TokenList>> Tokens(string treasuryCode, List<int> tokenStatus);
        public Task<int> TokenCountByStatus(string treasuryCode, List<int> tokenStatus);
        public Task<int> AllTokensCount();
        public Task<ReturnMemoBillDetailsDTO> ReturnMemoBillDetails(long tokenId);
        public Task<bool> GenerateReturnMemo(long tokenId, string referenceNo, long userId, int ownType);
        public Task<TokenPrintDTO> PrintByTokenId(long tokenId);
    }
}