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
           return await  _ChequeIndentRepository.GetSelectedColumnByConditionAsync(entity=>true,entity=> new ChequeIndentListDTO
            {
                IndentDate = entity.IndentDate,
                IndentId = entity.IndentId,
                MemoDate = entity.MemoDate,
                MemoNo = entity.MemoNo,
                Remarks = entity.Remarks
            },dynamicListQueryParameters);
        }
        public async Task<bool> ApproveRejectIndent(long userId,short status)
        {
            ChequeIndent chequeIndent = new ChequeIndent
            {
                Status = status,
                ApprovedRejectedBy = userId,
                ApprovedRejectedAt = DateTime.Now,
            };
            if (_ChequeIndentRepository.Add(chequeIndent))
            {
                _ChequeIndentRepository.SaveChangesManaged();
                return true;
            }
            return false;
        }
    }
}