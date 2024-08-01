using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IReceiptSequenceService : IBaseService
    {
        public Task<string> GenerateTreasuryReceiptNo(short finYear, string treasuryCode);
    }
}