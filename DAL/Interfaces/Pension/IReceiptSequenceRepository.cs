using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IReceiptSequenceRepository : IRepository<PpoReceiptSequence>
    {
        Task<string> GenerateTreasuryReceiptNoAsync(short finYear, string treasuryCode);
        string GenerateTreasuryReceiptNo(short finYear, string treasuryCode);
    }
}