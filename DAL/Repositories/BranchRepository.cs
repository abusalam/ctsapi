using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
namespace CTS_BE.DAL
{
   public class BranchRepository : Repository<Branch, CTSDBContext>, IBranchRepository
   {
       public BranchRepository(CTSDBContext context) : base(context)
       {
       }
   }
}