using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.DAL
{
   public class ChequeEntryRepository : Repository<ChequeEntry, CTSDBContext>, IChequeEntryRepository
   {
       public ChequeEntryRepository(CTSDBContext context) : base(context)
       {
       }
   }
}