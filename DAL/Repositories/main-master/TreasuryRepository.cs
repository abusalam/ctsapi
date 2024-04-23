using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.DAL
{
   public class TreasuryRepository : Repository<Treasury, CTSDBContext>, ITreasuryRepository
   {
       public TreasuryRepository(CTSDBContext context) : base(context)
       {
       }
   }
}