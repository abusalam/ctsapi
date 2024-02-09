using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.master;

namespace CTS_BE.DAL.Repositories.master
{
    public class GobalObjectionRepository : Repository<GobalObjection, CTSDBContext>, IGobalObjectionRepository
    {
        public GobalObjectionRepository(CTSDBContext context) : base(context)
        {
        }
    }
}