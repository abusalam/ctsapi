using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeInvoiceService
    {
        public Task<bool> InsertIndentInvoice(ChequeInvoiceDTO chequeInvoiceDTO);
    }
}