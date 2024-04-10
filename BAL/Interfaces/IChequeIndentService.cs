using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeIndentService
    {
        public Task<bool> Insert(ChequeIndentDTO chequeIndentDTO);
        public Task<bool> ApproveRejectIndent(long userId, short status);
        public Task<IEnumerable<ChequeIndentListDTO>> ChequeIndentList(DynamicListQueryParameters dynamicListQueryParameters);

    }
}