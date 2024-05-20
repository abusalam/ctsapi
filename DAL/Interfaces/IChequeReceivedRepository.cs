using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces
{
    public interface IChequeReceivedRepository : IRepository<ChequeReceived>
    {
        public Task<ChequeReceivedDTO> Insert(string chequeReceivedData);
    }
}