using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Model.Cheque;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services
{
    public class ChequeReceivedService : IChequeReceivedService
    {
        private readonly CTSDBContext _context;
        private readonly IChequeReceivedRepository _ChequeReceivedRepository;
        private readonly IChequeInvoiceDetailRepository _ChequeInvoiceDetailRepository;

        public ChequeReceivedService(IChequeReceivedRepository ChequeReceivedRepository, IChequeInvoiceDetailRepository ChequeInvoiceDetailRepository, CTSDBContext cTSDBContext)
        {
            _ChequeReceivedRepository = ChequeReceivedRepository;
            _ChequeInvoiceDetailRepository = ChequeInvoiceDetailRepository;
            _context = cTSDBContext;
        }
        public async Task<Int16?> ChequeReceived(ChequeReceivedDTO chequeReceivedDTO)
        {
            List<ChequeReceivedModel> chequeReceivedModel = new List<ChequeReceivedModel>();
            List<int> exclusions = new List<int>();
            List<Tuple<Int16, List<int>>> exclusionlist = new();

            foreach (var chequeReceivedDamagedDetail in chequeReceivedDTO.ChequeReceivedDamagedDetails)
            {
                exclusions = chequeReceivedDamagedDetail.DamageIndex.Split(',').Select(int.Parse).ToList();
                var invoiceDeatilsId = chequeReceivedDamagedDetail.InvoiceDeatilsId;
                var invoiceDetails = await _ChequeInvoiceDetailRepository.GetSingleSelectedColumnByConditionAsync(entity => entity.ChequeInvoiceId == 108, entity => new
                {
                    Start = entity.Start,
                    End = entity.End
                });
                List<(int start, int end)> values = CommonHelper.SplitRange(invoiceDetails.Start, invoiceDetails.End, exclusions);
                foreach (var item in values)
                {
                    chequeReceivedModel.Add(new ChequeReceivedModel { Start = item.start, End = item.end, Quantity = item.end - item.start, InvoiceDeatilsId = invoiceDeatilsId });
                }
                //
                Tuple<Int16, List<int>> exclusion = new(chequeReceivedDamagedDetail.DamageType, exclusions);

                exclusionlist.Add(exclusion);
            }
            string chequeReceivedData = JSONHelper.ObjectToJson(chequeReceivedModel);
            string exclusionListData = JSONHelper.ObjectToJson(exclusionlist);

            return await _ChequeReceivedRepository.Insert(chequeReceivedData, exclusionListData);
        }

        public async Task<IEnumerable<ChequeReceivedListDTO>> ChequeReceivedList(DynamicListQueryParameters dynamicListQueryParameters, List<int> statusIds)
        {
            var chequeReceivedList = await _ChequeReceivedRepository.GetSelectedColumnByConditionAsync(
                entity => statusIds.Contains((int)entity.Status),
                entity => new ChequeReceivedListDTO
                {
                    Id = entity.Id,
                    InvoiceDeatilsId = entity.ChequeInvoiceDetailsId,
                    Quantity = entity.Quantity,
                },
                dynamicListQueryParameters);

            return chequeReceivedList;
        }

        public async Task<IEnumerable<ChequeReceivedDataWithMICRDTO>> AllChequeReceivedList()
        {
            var result = await (from cr in _context.ChequeReceiveds
                                join cid in _context.ChequeInvoiceDetails on cr.InvoiceId equals cid.ChequeInvoiceId
                                join ce in _context.ChequeEntries on cid.ChequeEntryId equals ce.Id
                                select new ChequeReceivedDataWithMICRDTO
                                {
                                    InvoiceId = cr.InvoiceId,
                                    Start = cr.Start,
                                    End = cr.End,
                                    Quantity = (short?)(cr.End - cr.Start),
                                    MicrCode = ce.MicrCode
                                }).ToListAsync();

            return result;
        }


        internal class ChequeInvoiceDetailService
        {
            public ChequeInvoiceDetailService()
            {
            }
        }
    }


}