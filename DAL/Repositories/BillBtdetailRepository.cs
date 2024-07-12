using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.DAL
{
   public class BillBtdetailRepository : Repository<BillBtdetail, CTSDBContext>, IBillBtdetailRepository
   {
       public BillBtdetailRepository(CTSDBContext context) : base(context)
       {
       }
   }
}