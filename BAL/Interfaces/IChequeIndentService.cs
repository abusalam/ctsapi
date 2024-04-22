using CTS_BE.DAL.Entities;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeIndentService
    {
        public Task<bool> Insert(ChequeIndentDTO chequeIndentDTO);
        public Task<ChequeIndent> ChequeIndentByIdStatus(long indentId, short statusId);
        public Task<bool> ApproveRejectIndent(ChequeIndent chequeIndent,long userId, short status);
        public Task<IEnumerable<ChequeIndentListDTO>> ChequeIndentList(DynamicListQueryParameters dynamicListQueryParameters);

    }
}