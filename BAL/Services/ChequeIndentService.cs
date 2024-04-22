using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;

namespace CTS_BE.BAL
{
    public class ChequeIndentService : IChequeIndentService
    {
        private readonly IChequeIndentRepository _ChequeIndentRepository;
        private readonly IMapper _mapper;
        public ChequeIndentService(IChequeIndentRepository ChequeIndentRepository, IMapper mapper) {
            _ChequeIndentRepository = ChequeIndentRepository;
            _mapper = mapper;
        }
        public async Task<bool> Insert(ChequeIndentDTO chequeIndentDTO)
        {
            string indentData = JSONHelper.ObjectToJson(chequeIndentDTO);
            return await _ChequeIndentRepository.InsertIndent(indentData);
        }
        public async Task<IEnumerable<ChequeIndentListDTO>> ChequeIndentList(DynamicListQueryParameters dynamicListQueryParameters)
        {
           return await  _ChequeIndentRepository.GetSelectedColumnByConditionAsync(entity=>entity.Status==1,entity=> new ChequeIndentListDTO
            {
                Id = entity.Id,
                IndentDate = entity.IndentDate.Value.ToString("dd/MM/yyyy"),
                IndentId = entity.IndentId,
                MemoDate =entity.MemoDate.Value.ToString("dd/MM/yyyy"),
                MemoNo = entity.MemoNo,
                Remarks = entity.Remarks
            },dynamicListQueryParameters);
        }
        public async Task<ChequeIndent> ChequeIndentByIdStatus(long indentId,short statusId)
        {
           return await _ChequeIndentRepository.GetSingleAysnc(entity=>entity.Id==indentId&&entity.Status==statusId);
        }
        public async Task<bool> ApproveRejectIndent(ChequeIndent chequeIndent,long userId,short status)
        {

            chequeIndent.Status = status;
            chequeIndent.ApprovedRejectedBy = userId;
            chequeIndent.ApprovedRejectedAt = DateTime.Now;
 
            if (_ChequeIndentRepository.Update(chequeIndent))
            {
                _ChequeIndentRepository.SaveChangesManaged();
                return true;
            }
            return false;
        }
    }
}