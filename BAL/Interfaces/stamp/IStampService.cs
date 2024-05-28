using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.stamp
{
    public interface IStampService
    {
        // Stamp Indent Interfaces
        Task<IEnumerable<StampIndentDTO>> ListAllStampIndents(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        public Task<bool> CreateNewStampIndent(StampIndentInsertDTO stampIndent);
        public Task<bool> ApproveStampIndent(long stampIndentId);
        public Task<bool> RejectStampIndent(long stampIndentId);

        // Stamp Invoice Interfaces
        Task<IEnumerable<StampInvoiceDTO>> ListAllStampInvoices(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<bool> CreateNewStampInvoice(StampInvoiceInsertDTO stampInvoice);


    }
}