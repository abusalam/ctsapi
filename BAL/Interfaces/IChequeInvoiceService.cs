using CTS_BE.DAL.Entities;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeInvoiceService
    {
        public Task<bool> InsertIndentInvoice(ChequeInvoiceDTO chequeInvoiceDTO);
        public Task<IEnumerable<ChequeInvoiceListDTO>> ChequeInvoiceList(DynamicListQueryParameters dynamicListQueryParameters, List<int> statusIds);
        public Task<bool> UpdateInvoiceStatus(ChequeInvoice chequeInvoice, int statusId);
        public Task<ChequeInvoice> ChequeInvoiceById(long chequeInvoiceId,short statusId);
       public Task<List<ChequeInvoiceDetailsByIdDTO>> ChequeInvoiceAndInvoiceDetailsById(long chequeInvoice);
    }
}