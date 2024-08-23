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

        public async Task<int> GetNextBillNo(short financialYear, string treasuryCode)
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

        public async Task<PpoBill> SavePpoBillBreakups(
            long ppoBillId,
            List<PpoBillBreakup> ppoBillBreakups
        )
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

        public async Task<PpoBill?> GetPpoFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            var ppoBill = await _pensionDbContext.PpoBills
                .Where(
                    entity => entity.ActiveFlag
                    && entity.BillType == 'F'
                    && entity.PpoId == ppoId
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .FirstOrDefaultAsync();

            if(ppoBill == null) {
                return null;
            }
            //Eager loading
            _pensionDbContext.PpoBills
                .Include(entity => entity.PpoBillBreakups)
                .ThenInclude(entity => entity.Revision)
                .ThenInclude(entity => entity.Rate)
                .ThenInclude(entity => entity.Breakup)
                .Load();

            //Explicit loading
            _pensionDbContext.Entry(ppoBill)
                .Reference(entity => entity.Pensioner)
                .Load();
            _pensionDbContext.Entry(ppoBill)
                .Reference(entity => entity.BankAccount)
                .Load();
            _pensionDbContext.Entry(ppoBill)
                .Collection(entity => entity.PpoBillBreakups)
                .Load();
            // ppoBill.PpoBillBreakups.ToList().ForEach(
            //     entity => {
            //     _pensionDbContext.Entry(entity)
            //         .Reference(entity => entity.Revision)
            //         .Load();
            //     _pensionDbContext.Entry(entity.Revision)
            //         .Reference(entity => entity.Rate)
            //         .Load();
            //     _pensionDbContext.Entry(entity.Revision.Rate)
            //         .Reference(entity => entity.Breakup)
            //         .Load();
            // });
            _pensionDbContext.Entry(ppoBill.Pensioner)
                .Reference(entity => entity.Category)
                .Load();
            _pensionDbContext.Entry(ppoBill.Pensioner.Category)
                .Reference(entity => entity.PrimaryCategory)
                .Load();
            _pensionDbContext.Entry(ppoBill.Pensioner)
                .Reference(entity => entity.Receipt)
                .Load();
            return ppoBill;
        }
    }
}