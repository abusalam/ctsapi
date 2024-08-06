using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoComponentRateRepository :
        Repository<PpoComponentRate, PensionDbContext>,
        IPpoComponentRateRepository
    {
        public PpoComponentRateRepository(PensionDbContext context) : base(context)
        {
        }
    }
}