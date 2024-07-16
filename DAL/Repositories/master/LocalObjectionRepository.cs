using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.master;

namespace CTS_BE.DAL.Repositories.master
{
    public class LocalObjectionRepository : Repository<LocalObjection, CTSDBContext>, ILocalObjectionRepository
    {
        public LocalObjectionRepository(CTSDBContext context) : base(context) {
        }
    }
}