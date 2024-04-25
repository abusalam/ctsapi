using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Model;

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
        public async Task<bool> Insert(ChequeIndentModel chequeIndentModel)
        {
            string indentData = JSONHelper.ObjectToJson(chequeIndentModel);
            return await _ChequeIndentRepository.InsertIndent(indentData);
        }
        public async Task<IEnumerable<ChequeIndentListDTO>> ChequeIndentList(DynamicListQueryParameters dynamicListQueryParameters,List<int> statusIds)
        {
            return await _ChequeIndentRepository.GetSelectedColumnByConditionAsync(entity => statusIds.Contains((int)entity.Status), entity => new ChequeIndentListDTO
            {
                Id = entity.Id,
                IndentDate = entity.IndentDate.Value.ToString("dd/MM/yyyy"),
                IndentId = entity.IndentId,
                MemoDate = entity.MemoDate.Value.ToString("dd/MM/yyyy"),
                MemoNo = entity.MemoNo,
                Remarks = entity.Remarks,
                CurrentStatus = entity.StatusNavigation.Name,
                CurrentStatusId = entity.Status,
            },dynamicListQueryParameters);
        }
        public async Task<ChequeIndent> ChequeIndentByIdStatus(long indentId,int statusId)
        {
           return await _ChequeIndentRepository.GetSingleAysnc(entity=>entity.Id==indentId&&entity.Status==statusId);
        }
        public async Task<bool> ApproveRejectIndent(ChequeIndent chequeIndent,long userId,int status)
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
        public async Task<List<ChequeIndentDTO>> ChequeIndentDetailsByIdStatus(long indentId, int statusId)
        {
            List<ChequeIndentDTO> chequeIndentDetails = (List<ChequeIndentDTO>) await _ChequeIndentRepository.GetSelectedColumnByConditionAsync(entity => entity.Id == indentId && entity.Status == statusId, entity => new ChequeIndentDTO
            {
                IndentId =  entity.Id,
                IndentDate = entity.IndentDate.Value.ToString("dd/MM/yyyy"),
                MemoDate = entity.MemoDate.Value.ToString("dd/MM/yyyy"),
                MemoNumber = entity.MemoNo,
                Remarks = entity.Remarks,
                ChequeIndentDeatils = entity.ChequeIndentDetails.Select(iDetils=> new ChequeIndentDeatilsDTO
                {
                    IndentDeatilsId = iDetils.Id,
                    ChequeType = iDetils.ChequeType,
                    MicrCode = iDetils.MicrCode,
                    Quantity = iDetils.Quantity,
                }).ToList(),
            });
            return chequeIndentDetails;
        }
    }
}