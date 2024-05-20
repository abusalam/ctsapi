using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Model.Cheque;

namespace CTS_BE.BAL.Services
{
    public class ChequeReceivedService : IChequeReceivedService
    {
        private readonly IChequeReceivedRepository _ChequeReceivedRepository;
        private readonly IChequeInvoiceDetailRepository _ChequeInvoiceDetailRepository;

        public ChequeReceivedService(IChequeReceivedRepository ChequeReceivedRepository, IChequeInvoiceDetailRepository ChequeInvoiceDetailRepository){
            _ChequeReceivedRepository = ChequeReceivedRepository;
            _ChequeInvoiceDetailRepository = ChequeInvoiceDetailRepository;
        }
        public async Task<Int16?> ChequeReceived(ChequeReceivedDTO chequeReceivedDTO)
        {
            List<ChequeReceivedModel> chequeReceivedModel = new List<ChequeReceivedModel>();
            List<int> exclusions = new List<int>();
            List<Tuple<Int16, List<int>>> exclusionlist = new();

            foreach (var chequeReceivedDamagedDetail in chequeReceivedDTO.ChequeReceivedDamagedDetails)
            {
                exclusions = chequeReceivedDamagedDetail.DamageIndex.Split(',').Select(int.Parse).ToList();
                var chequeEntryId = chequeReceivedDamagedDetail.ChequeEntryId;
                var invoiceDetails = await _ChequeInvoiceDetailRepository.GetSingleSelectedColumnByConditionAsync(entity => entity.ChequeInvoiceId == 91, entity => new
                {
                    Start = entity.Start,
                    End = entity.End
                });
                List<(int start, int end)> values = CommonHelper.SplitRange(invoiceDetails.Start, invoiceDetails.End, exclusions);
                foreach (var item in values)
                {
                    chequeReceivedModel.Add(new ChequeReceivedModel { Start = item.start, End = item.end, Quantity = item.end - item.start, ChequeEntryId = chequeEntryId });
                }
                //
                Tuple<Int16, List<int>> exclusion = new(chequeReceivedDamagedDetail.DamageType, exclusions);
         
                exclusionlist.Add(exclusion);
            }
            string chequeReceivedData = JSONHelper.ObjectToJson(chequeReceivedModel);
            string exclusionListData = JSONHelper.ObjectToJson(exclusionlist);
            
            return await _ChequeReceivedRepository.Insert(chequeReceivedData, exclusionListData);
        }

      

        internal class ChequeInvoiceDetailService
        {
            public ChequeInvoiceDetailService()
            {
            }
        }
    }
}