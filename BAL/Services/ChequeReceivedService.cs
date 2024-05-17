using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<ChequeReceivedDTO> ChequeReceived(ChequeReceivedDTO chequeReceivedDTO)
        {
            List<ChequeReceivedModel> chequeReceivedModel = new List<ChequeReceivedModel>();

            foreach (var chequeReceivedDamagedDetail in chequeReceivedDTO.ChequeReceivedDamagedDetails)
            {
                List<int> exclusions = chequeReceivedDamagedDetail.DamageIndex.Split(',').Select(int.Parse).ToList();
                var chequeEntryId = chequeReceivedDamagedDetail.ChequeEntryId;
                var invoiceDetails = await _ChequeInvoiceDetailRepository.GetSingleSelectedColumnByConditionAsync(entity => entity.Id == 1, entity => new
                {
                    Start = entity.Start,
                    End = entity.End
                });
                List<(int start, int end)> values = CommonHelper.SplitRange(invoiceDetails.Start, invoiceDetails.End, exclusions);
                foreach (var item in values)
                {
                    chequeReceivedModel.Add(new ChequeReceivedModel { Start = item.start, End = item.end, Quantity = item.end - item.start, ChequeEntryId = chequeEntryId });
                }
            }
            string chequeReceivedData = JSONHelper.ObjectToJson(chequeReceivedModel);
            return await _ChequeReceivedRepository.Insert(chequeReceivedData);
        }

        Task IChequeReceivedService.ChequeReceived(ChequeReceivedDTO chequeReceivedDTO)
        {
            throw new NotImplementedException();
        }

        internal class ChequeInvoiceDetailService
        {
            public ChequeInvoiceDetailService()
            {
            }
        }
    }
}