using CTS_BE.DAL.Entities;
namespace CTS_BE.DAL.Interfaces
{
    public interface IChequeInvoiceRepository: IRepository<ChequeInvoice>
    {
        public Task<bool> Insert(string chequeInvoiceData);
    }
}