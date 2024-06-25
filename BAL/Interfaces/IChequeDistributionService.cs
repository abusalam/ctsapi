using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeDistributionService
    {
        public Task<IEnumerable<UserListDTO>> UserList();
        public Task<bool> saveChequeDistribution(ChequeDistributionDTO chequeDistributionDTO);


    }
}