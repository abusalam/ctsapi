using CTS_BE.DAL.Entities;
namespace CTS_BE.DAL.Interfaces
{
    public interface ITokenHasObjectionsRepository: IRepository<TokenHasObjection>
    {
        public Task<bool> BillCheck(long tokenId, string referenceNo, string billObjections, string overruledObjections, long userId, int ownType);
    }
}