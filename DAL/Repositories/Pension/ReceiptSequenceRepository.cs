using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ReceiptSequenceRepository : 
        Repository<PpoReceiptSequence, PensionDbContext>, 
        IReceiptSequenceRepository
    {
        protected readonly PensionDbContext _context;
        public ReceiptSequenceRepository(PensionDbContext context) : 
            base(context)
        {
            _context = context;
        }


        public async Task<string> GenerateTreasuryReceiptNo(short finYear, string treasuryCode) {
            PpoReceiptSequence ppoReceiptSequence = new();
            string treasuryReceiptNo = "";
            int seqValue = 0;
            
            try {
                ppoReceiptSequence = await GetSingleAysnc(entity => entity.FinancialYear == finYear && entity.TreasuryCode == treasuryCode);
                if(ppoReceiptSequence?.NextSequenceValue > 0) {
                    ppoReceiptSequence.NextSequenceValue++;
                    if(Update(ppoReceiptSequence)) {
                        seqValue = ppoReceiptSequence.NextSequenceValue;
                    }

                } else {
                    ppoReceiptSequence = new () {
                            FinancialYear = finYear,
                            TreasuryCode = treasuryCode,
                            NextSequenceValue = 1
                        };
                    Add(ppoReceiptSequence);
                }
                if(await SaveChangesManagedAsync()>0) {
                    seqValue = ppoReceiptSequence.NextSequenceValue;
                }
            }
            finally {
                
                string paddedNextSequenceValue = $"{seqValue}".PadLeft(6,'0');
                treasuryReceiptNo = $"{treasuryCode}{finYear}{paddedNextSequenceValue}";
            }
            return treasuryReceiptNo;
        }

    }
}