using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampTypeRepository : Repository<StampType, CTSDBContext>, IStampTypeRepository
    {
        protected readonly CTSDBContext _context;
        public StampTypeRepository(CTSDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
