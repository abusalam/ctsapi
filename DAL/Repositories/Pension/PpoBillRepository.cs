using AutoMapper;
using CTS_BE.BAL.Services.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoBillRepository(PensionDbContext context, IMapper mapper) : IPpoBillRepository
    {
        private readonly PensionDbContext _pensionDbContext = context;
        private readonly IMapper _mapper = mapper;

        public async Task<int> GetNextBillNo(short financialYear, string treasuryCode)
        {
            int nextBillNo = await _pensionDbContext
                .Bills.Where(entity =>
                    entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.FinancialYear == financialYear
                )
                .OrderByDescending(entity => entity.BillNo)
                .Select(entity => entity.BillNo)
                .FirstOrDefaultAsync();

            return nextBillNo + 1;
        }

        public async Task<Bill?> GetBillByPpoBillId(
            long ppoBillId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Bills.Where(entity =>
                    entity
                        .PpoBills.Where(entity =>
                            entity.ActiveFlag
                            && entity.Id == ppoBillId
                            && entity.FinancialYear == financialYear
                            && entity.TreasuryCode == treasuryCode
                        )
                        .Any()
                )
                .FirstOrDefaultAsync();
        }

        public async Task<List<PpoComponentRevision>> GetPpoComponentRevisionsByPensionerId(
            long pensionerId
        )
        {
            return await _pensionDbContext
                .PpoComponentRevisions.Where(entity =>
                    entity.ActiveFlag && entity.PensionerId == pensionerId
                )
                .ToListAsync();
        }

        public async Task<T> SavePpoBill<T>(
            PpoBill ppoBillEntity,
            short financialYear,
            string treasuryCode
        )
        {
            T response = _mapper.Map<T>(ppoBillEntity);
            try
            {
                await _pensionDbContext.PpoBills.AddAsync(ppoBillEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    response.FillErrorInDataSource(ppoBillEntity, "Pension bill not saved.");
                    return response;
                }
                response = _mapper.Map<T>(ppoBillEntity);
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(ppoBillEntity, "RepositoryException: ", ex);
                return response;
            }

            return response;
        }

        public async Task<Pensioner?> GetPensionerByPpoId(int ppoId, string treasuryCode)
        {
            return await _pensionDbContext
                .Pensioners.Include(p => p.Branch)
                .ThenInclude(b => b.Bank)
                .Include(p => p.Category)
                .ThenInclude(c => c.PrimaryCategory)
                .ThenInclude(pc => pc.AccountHead)
                .Include(p => p.Category)
                .ThenInclude(c => c.SubCategory)
                .Include(p => p.Receipt)
                .Include(p => p.Category)
                .ThenInclude(c => c.ComponentRates)
                .ThenInclude(cr => cr.Breakup)
                .Include(p => p.PpoComponentRevisions)
                .AsSplitQuery()
                .FirstOrDefaultAsync(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                );
        }

        public async Task<long> GetHoaIdByPpoId(
            long ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Category)
                .ThenInclude(entity => entity.PrimaryCategory)
                .Select(entity => entity.Category.PrimaryCategory.AccountHeadId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsPpoApproved(long ppoId, short financialYear, string treasuryCode)
        {
            return await _pensionDbContext
                .PpoStatusFlags.Where(entity =>
                    entity.ActiveFlag
                    && entity.StatusFlag == PensionStatusFlag.PpoApproved
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                    && entity.FinancialYear == financialYear
                )
                .AnyAsync();
        }

        public async Task<bool> IsFirstBillAlreadyGenerated(
            long ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .PpoBills.Where(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.BillType == BillType.FirstBill
                    // && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .AnyAsync();
        }

        public async Task<bool> IsRegularBillAlreadyGenerated(
            long ppoId,
            int month,
            int year,
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .PpoBills.Where(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.BillType == BillType.RegularBill
                    // && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                    && entity.Bill.FromDate.Month == month
                    && entity.Bill.FromDate.Year == year
                )
                .AnyAsync();
        }

        public async Task<DateOnly> LastBillGeneratedUpTo(long ppoId, string treasuryCode)
        {
            return await _pensionDbContext
                .PpoBillBreakups.Where(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    // && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .OrderByDescending(entity => entity.ToDate)
                .Select(entity => entity.ToDate)
                .FirstOrDefaultAsync();
        }
    }
}
