using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.DAL
{
   public class DdoRepository : Repository<Ddo, CTSDBContext>, IDdoRepository
   {
       public DdoRepository(CTSDBContext context) : base(context)
       {
       }
   }
}