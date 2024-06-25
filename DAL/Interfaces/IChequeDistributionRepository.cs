using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.DAL.Interfaces
{
    public interface IChequeDistributionRepository
    {
        public Task<bool> Insert(string chequeDistributionDetails);


    }
}