using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoComponentRevisionRepository :
        Repository<PpoComponentRevision, PensionDbContext>,
        IPpoComponentRevisionRepository
    {
        public PpoComponentRevisionRepository(PensionDbContext context) : base(context)
        {
        }
    }
}