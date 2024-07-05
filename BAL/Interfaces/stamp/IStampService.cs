using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.stamp
{
    public interface IStampService
    {
        // Stamp Indent Interfaces
        Task<IEnumerable<StampIndentDTO>> StampIndentList(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<IEnumerable<StampIndentDTO>> ListAllStampIndentsProcessing(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<IEnumerable<StampIndentDTO>> ListAllStampIndentsProcessed(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<StampIndentDTO> GetStampIndentById(long IndentId);
        Task<bool> CreateNewStampIndent(StampIndentInsertDTO stampIndent);
        Task<bool> ApproveStampIndent(long stampIndentId, short sheet, short label);
        Task<bool> ReceiveStampIndent(short sheet, short label, long stampIndentId);
        Task<bool> RejectStampIndent(long stampIndentId);

        // Stamp Invoice Interfaces
        Task<IEnumerable<StampInvoiceDTO>> ListAllStampInvoices(List<FilterParameter> filters = null, int pageIndex = 0, int pageSize = 10, SortParameter sortParameters = null);
        Task<bool> CreateNewStampInvoice(StampInvoiceInsertDTO stampInvoice);


    }
}