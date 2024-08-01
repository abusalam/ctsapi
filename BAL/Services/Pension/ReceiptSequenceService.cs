using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DAL.Repositories.Pension;

namespace CTS_BE.BAL.Services.Pension
{
    public class ReceiptSequenceService : BaseService, IReceiptSequenceService
    {
        private readonly IReceiptSequenceRepository _receiptSequenceRepository;

        public ReceiptSequenceService(
            IReceiptSequenceRepository receiptSequenceRepository
            )
        {
            _receiptSequenceRepository = receiptSequenceRepository;
        }

        public async Task<string> GenerateTreasuryReceiptNo(short finYear, string treasuryCode)
        {
            PpoReceiptSequence ppoReceiptSquence = new();
            string treasuryReceiptNo = "";
            int seqValue = 0;
            
            try {
                ppoReceiptSquence = await _receiptSequenceRepository
                    .GetSingleAysnc(entity => entity.FinancialYear == finYear && entity.TreasuryCode == treasuryCode);
                if(ppoReceiptSquence.NextSequenceValue > 0) {
                    ppoReceiptSquence.NextSequenceValue++;
                    if(_receiptSequenceRepository.Update(ppoReceiptSquence)) {
                        seqValue = ppoReceiptSquence.NextSequenceValue;
                    }

                } else {
                    _receiptSequenceRepository.Add(
                        new PpoReceiptSequence() {
                            FinancialYear = finYear,
                            TreasuryCode = treasuryCode,
                            NextSequenceValue = 1
                        }
                    );
                }
                if(await _receiptSequenceRepository.SaveChangesManagedAsync()>0) {
                    seqValue = ppoReceiptSquence.NextSequenceValue;
                }
            }
            finally {
                
                string paddedNextSequenceValue = $"{seqValue}".PadLeft(8,'0');
                treasuryReceiptNo = $"{treasuryCode}{finYear}{paddedNextSequenceValue}";
            }
            return treasuryReceiptNo;
        }
    }
}