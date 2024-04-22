using CTS_BE.DAL.Entities;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeIndentService
    {
        public Task<bool> Insert(ChequeIndentDTO chequeIndentDTO);
        public Task<ChequeIndent> ChequeIndentByIdStatus(long indentId, int statusId);
        public Task<bool> ApproveRejectIndent(ChequeIndent chequeIndent,long userId, int status);
        public Task<List<ChequeIndentDTO>> ChequeIndentDetailsByIdStatus(long indentId, int statusId);
        public Task<IEnumerable<ChequeIndentListDTO>> ChequeIndentList(DynamicListQueryParameters dynamicListQueryParameters, int status = (int)Enum.IndentStatus.NewIndent);

    }
}