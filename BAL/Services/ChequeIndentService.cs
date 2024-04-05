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
    }
}