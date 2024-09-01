using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoBillRepository : Repository<PpoBill, PensionDbContext>, IPpoBillRepository
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;
        public PpoBillRepository(
            PensionDbContext context,
            IMapper mapper
        ) : base(context)
        {
            _pensionDbContext = context;
            _mapper = mapper;
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

        public async Task<PpoBill?> GetPpoBillById(
            long ppoBillId
        )
        {
            var ppoBill = await _pensionDbContext.PpoBills
                .Where(
                    entity => entity.ActiveFlag
                    && entity.Id == ppoBillId
                )
                .FirstOrDefaultAsync();
            return ppoBill;
        }

        public async Task<PpoBill?> GetPpoBillByPpoId(
            int ppoId,
            string treasuryCode
        )
        {
            var ppoBill = await _pensionDbContext.PpoBills
                .Where(
                    entity => entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                )
                .FirstOrDefaultAsync();
            return ppoBill;
        }

        public async Task<PpoBill?> GetPpoBillByPpoId(
            int ppoId,
            string treasuryCode,
            short financialYear
        )
        {
            var ppoBill = await _pensionDbContext.PpoBills
                .Where(
                    entity => entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .FirstOrDefaultAsync();
            return ppoBill;
        }
    
        public async Task<T> SavePpoBill<T>(
            char billType,
            PpoBill ppoBillEntity,
            short financialYear,
            string treasuryCode
        )
        {
            T ppoBillResponseDTO = default!;

            ppoBillResponseDTO = _mapper.Map<T>(ppoBillEntity);

            Pensioner? pensioner = await _pensionDbContext.Pensioners
                .Where(
                    entity => entity.ActiveFlag
                    && entity.PpoId == ppoBillEntity.PpoId
                    && entity.TreasuryCode == treasuryCode
                )
                .FirstOrDefaultAsync();
            if(pensioner == null) {
                ppoBillResponseDTO.FillDataSource(
                    pensioner,
                    "Pensioner not found!"
                );
                return ppoBillResponseDTO;
            }
            _pensionDbContext.Entry(pensioner)
                .Collection(entity => entity.BankAccounts)
                .Query().Where(entity => entity.ActiveFlag)
                .Load();
            if(pensioner.BankAccounts.Count == 0) {
                ppoBillResponseDTO.FillDataSource(
                    pensioner,
                    "Pensioner bank account not found!"
                );
                return ppoBillResponseDTO;
            }

            ppoBillEntity.ActiveFlag = true;
            ppoBillEntity.FromDate = pensioner.DateOfCommencement;
            ppoBillEntity.PensionerId = pensioner.Id;
            ppoBillEntity.BankAccountId = pensioner.BankAccounts.First().Id;
            ppoBillEntity.FinancialYear = financialYear;
            ppoBillEntity.TreasuryCode = treasuryCode;
            ppoBillEntity.PpoBillBreakups.ToList().ForEach(
                entity => {
                    entity.Revision.PensionerId = pensioner.Id;
                    entity.TreasuryCode = treasuryCode;
                    entity.FinancialYear = financialYear;
                }
            );
            ppoBillEntity.BillNo = await GetNextBillNo(financialYear, treasuryCode);
            await _pensionDbContext.PpoBills.AddAsync(ppoBillEntity);
            await _pensionDbContext.SaveChangesAsync();
            _pensionDbContext.Entry(ppoBillEntity)
                .Reference(entity => entity.Pensioner)
                .Load();
            _pensionDbContext.Entry(ppoBillEntity.Pensioner)
                .Reference(entity => entity.Receipt)
                .Load();
            _pensionDbContext.Entry(ppoBillEntity.Pensioner)
                .Reference(entity => entity.Category)
                .Load();

            ppoBillEntity.PpoBillBreakups.ToList().ForEach(
                entity => {
                    _pensionDbContext.Entry(entity)
                        .Reference(entity => entity.Revision)
                        .Load();
                    _pensionDbContext.Entry(entity.Revision)
                        .Reference(entity => entity.Rate)
                        .Load();
                    _pensionDbContext.Entry(entity.Revision.Rate)
                        .Reference(entity => entity.Breakup)
                        .Load();
                }
            );
            ppoBillResponseDTO = _mapper.Map<T>(ppoBillEntity);

            return ppoBillResponseDTO;
        }
    }
}