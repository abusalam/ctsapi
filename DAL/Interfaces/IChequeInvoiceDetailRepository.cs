using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities;

namespace CTS_BE.DAL.Interfaces
{
    public interface IChequeInvoiceDetailRepository: IRepository<ChequeInvoiceDetail>
    {
        public Task<bool> Insert(string chequeInvoiceDetailsData);
    }
}