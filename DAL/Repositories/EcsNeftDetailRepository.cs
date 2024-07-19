using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.DAL
{
   public class EcsNeftDetailRepository : Repository<EcsNeftDetail, CTSDBContext>, IEcsNeftDetailRepository
   {
       public EcsNeftDetailRepository(CTSDBContext context) : base(context)
       {
       }
   }
}