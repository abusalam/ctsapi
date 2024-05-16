using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampLabelRepository: Repository<StampLabelMaster, CTSDBContext>, IStampLabelRepository
    {
        protected readonly CTSDBContext _context;
        public StampLabelRepository(CTSDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
