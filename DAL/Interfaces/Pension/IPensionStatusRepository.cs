using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.PensionEnum;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPensionStatusRepository : IRepository<PpoStatusFlag>
    {
        
    }
}