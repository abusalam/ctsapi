using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.DAL
{
   public class ChequeCountRepository : Repository<ChequeCount, CTSDBContext>, IChequeCountRepository
   {
       public ChequeCountRepository(CTSDBContext context) : base(context)
       {
       }
   }
}