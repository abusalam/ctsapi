using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.DAL
{
   public class BankRepository : Repository<Bank, CTSDBContext>, IBankRepository
   {
       public BankRepository(CTSDBContext context) : base(context)
       {
       }
   }
}