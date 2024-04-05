using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;

namespace CTS_BE.BAL
{
    public class ChequeInvoiceService : IChequeInvoiceService
    {
        private readonly IChequeInvoiceRepository _ChequeInvoiceRepository;
        private readonly IMapper _mapper;
        public ChequeInvoiceService(IChequeInvoiceRepository ChequeInvoiceRepository, IMapper mapper) {
            _ChequeInvoiceRepository = ChequeInvoiceRepository;
            _mapper = mapper;
        }
        public async Task<bool> InsertIndentInvoice(ChequeInvoiceDTO chequeInvoiceDTO)
        {
            string chequeInvoiceData =  JSONHelper.ObjectToJson(chequeInvoiceDTO);
            return await _ChequeInvoiceRepository.Insert(chequeInvoiceData);
        }
    }
}