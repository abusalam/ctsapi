using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs.PensionDTO;
using CTS_BE.Model.Pension;
using CTS_BE.DAL.Interfaces.Pension;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PensionerDetailsRepository : Repository<PMmPenPrepPensionerDetl, NewCtsContext>
    , IPensionerDetailsRepository
    {
        protected readonly NewCtsContext _context;
        public PensionerDetailsRepository(NewCtsContext context) : base(context)
        {
            _context = context;
        }

        public PMmPenPrepPensionerDetl Create(PMmPenPrepPensionerDetl entity)
        {
            throw new NotImplementedException();
        }

    }
}
