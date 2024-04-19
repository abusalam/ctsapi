using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface ITokenService
    {
        public Task<int> InsterNewToken(TokenDTO tokenDTO, long userId);
        public Task<TokenDetailsDto> TokenDeatisById(long tokenId);
        public Task<IEnumerable<TokenList>> AllTokens(string treasuryCode);
        public Task<DynamicListResult<IEnumerable<TokenList>>> Tokens(string treasuryCode, List<int> tokenStatus, List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        //public Task<IEnumerable<TokenList>> Tokens(string treasuryCode, List<int> tokenStatus, List<FilterParameter> filters = null);
        public Task<int> TokenCountByStatus(string treasuryCode, List<int> tokenStatus, List<FilterParameter> dynamicFilters = null);
        public Task<int> AllTokensCount();
        public Task<ReturnMemoBillDetailsDTO> ReturnMemoBillDetails(long tokenId);
        public Task<bool> GenerateReturnMemo(long tokenId, string referenceNo, long userId, int ownType);
        public Task<TokenPrintDTO> PrintByTokenId(long tokenId);
    }
}