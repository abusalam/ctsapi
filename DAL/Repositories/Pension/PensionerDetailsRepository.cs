using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PensionerDetailsRepository : Repository<Pensioner, PensionDbContext>, IPensionerDetailsRepository
    {
        public PensionerDetailsRepository(PensionDbContext context) : base(context)
        {
        }
    }
}