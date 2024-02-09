using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.DAL
{
   public class TokenFlowRepository : Repository<TokenFlow, CTSDBContext>, ITokenFlowRepository
   {
       public TokenFlowRepository(CTSDBContext context) : base(context)
       {
       }
   }
}