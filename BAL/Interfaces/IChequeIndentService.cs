using CTS_BE.DAL.Entities;
using CTS_BE.DTOs;
using CTS_BE.Model;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeIndentService
    {
        public Task<bool> Insert(ChequeIndentModel chequeIndentModel);
        public Task<ChequeIndent> ChequeIndentByIdStatus(long indentId, int statusId);
        public Task<bool> ApproveRejectIndent(ChequeIndent chequeIndent,long userId, int status);
        public  Task<ChequeIndentDTO> ChequeIndentDetailsByIdStatus(long indentId, int statusId);
        public Task<IEnumerable<ChequeIndentListDTO>> ChequeIndentList(DynamicListQueryParameters dynamicListQueryParameters, List<int> statusIds);

    }
}