using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoBillRepository : Repository<PpoBill, PensionDbContext>, IPpoBillRepository
    {
        private readonly PensionDbContext _pensionDbContext;
        public PpoBillRepository(PensionDbContext context) : base(context)
        {
            _pensionDbContext = context;
        }

        public async Task<int> GetNextBillNo(int financialYear, string treasuryCode)
        {
            int nextBillNo = await _pensionDbContext.PpoBills
                .Where(
                    entity => entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.FinancialYear == financialYear
                )
                .OrderByDescending(
                    entity => entity.BillNo
                )
                .Select(
                    entity => entity.BillNo
                )
                .FirstOrDefaultAsync();
            
            return nextBillNo + 1;
        }

        public async Task<PpoBill> SavePpoBillBreakups(long ppoBillId, List<PpoBillBreakup> ppoBillBreakups)
        {
            var ppoBill = await _pensionDbContext.PpoBills
                .Where(
                    entity => entity.Id == ppoBillId
                )
                .FirstOrDefaultAsync() ?? new();
            ppoBill.PpoBillBreakups = ppoBillBreakups;
            await _pensionDbContext.PpoBills.AddAsync(ppoBill);
            await _pensionDbContext.SaveChangesAsync();
            return ppoBill;
        }
    }
}