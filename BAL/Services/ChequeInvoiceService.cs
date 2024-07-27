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
        public ChequeInvoiceService(IChequeInvoiceRepository ChequeInvoiceRepository, IMapper mapper)
        {
            _ChequeInvoiceRepository = ChequeInvoiceRepository;
            _mapper = mapper;
        }
        public async Task<bool> InsertIndentInvoice(ChequeInvoiceDTO chequeInvoiceDTO)
        {
            string chequeInvoiceData = JSONHelper.ObjectToJson(chequeInvoiceDTO);
            return await _ChequeInvoiceRepository.Insert(chequeInvoiceData);    
        }
        public async Task<IEnumerable<ChequeInvoiceListDTO>> ChequeInvoiceList(DynamicListQueryParameters dynamicListQueryParameters, List<int> statusIds)
        {
            return await _ChequeInvoiceRepository.GetSelectedColumnByConditionAsync(entity => statusIds.Contains((int)entity.Status), entity => new ChequeInvoiceListDTO
            {
                Id = entity.Id,
                InvoiceNumber = entity.InvoiceNumber,
                InvoiceDate = entity.InvoiceDate.Value.ToString("dd/MM/yyyy"),
                MemoNumber = entity.ChequeIndent.MemoNo,
                Quantity = entity.ChequeIndent.TotalApprovedQuantity,
                CurrentStatus = entity.StatusNavigation.Name,
                CurrentStatusId = entity.Status
            });
        }
        public async Task<bool> UpdateInvoiceStatus(ChequeInvoice chequeInvoice, int statusId)
        {
            chequeInvoice.Status = statusId;
            if (_ChequeInvoiceRepository.Update(chequeInvoice))
            {
                _ChequeInvoiceRepository.SaveChangesManaged();
                return true;
            }
            return false;
        }
        public async Task<ChequeInvoice> ChequeInvoiceById(long chequeInvoiceId, short statusId)
        {
            return await _ChequeInvoiceRepository.GetSingleAysnc(entity => entity.Id == chequeInvoiceId && entity.Status == statusId);
        }
        public async Task<ChequeInvoiceDetailsByIdDTO> ChequeInvoiceAndInvoiceDetailsById(long chequeInvoice)
        {
            ChequeInvoiceDetailsByIdDTO chequeInvoiceDetails = await _ChequeInvoiceRepository.GetSingleSelectedColumnByConditionAsync(entity => entity.Id == chequeInvoice, entity => new ChequeInvoiceDetailsByIdDTO
            {
                ChequeInvoiceSeries = entity.ChequeInvoiceDetails.Select(indentDetails => new ChequeInvoiceSeriesDTO
                {
                    Quantity = indentDetails.Quantity,
                    MicrCode = indentDetails.ChequeEntry.MicrCode,
                    TreasuryCode = indentDetails.ChequeEntry.TreasurieCode,
                    Series = indentDetails.ChequeEntry.SeriesNo,
                    InvoiceDeatilsId = indentDetails.Id,
                    Start = indentDetails.Start,
                    End = indentDetails.End,
                }).ToList(),
                Quantity = entity.ChequeIndent.TotalApprovedQuantity,
            });
            return chequeInvoiceDetails;
        }

    }
}