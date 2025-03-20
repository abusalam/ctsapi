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
    public class PpoBillRepository : Repository<PpoBill, PensionDbContext>, IPpoBillRepository
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;

        public PpoBillRepository(PensionDbContext context, IMapper mapper)
            : base(context)
        {
            _pensionDbContext = context;
            _mapper = mapper;
        }

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

        public async Task<T> GetPpoFirstBillByPpoId<T>(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            T response = _mapper.Map<T>(new PpoBill());
            try
            {
                PpoBill ppoBill = await _pensionDbContext
                    .PpoBills.Where(entity =>
                        entity.ActiveFlag
                        && entity.BillType == BillType.FirstBill
                        && entity.PpoId == ppoId
                        && entity.FinancialYear == financialYear
                        && entity.TreasuryCode == treasuryCode
                    )
                    .FirstAsync();

                //Eager loading
                _pensionDbContext
                    .PpoBills.Include(entity => entity.Bill)
                    .Include(entity => entity.PpoBillBreakups)
                    .ThenInclude(entity => entity.Revision)
                    .ThenInclude(entity => entity.Rate)
                    .ThenInclude(entity => entity.Breakup)
                    .Load();

                //Explicit loading
                _pensionDbContext.Entry(ppoBill).Reference(entity => entity.Pensioner).Load();

                _pensionDbContext
                    .Entry(ppoBill)
                    .Collection(entity => entity.PpoBillBreakups)
                    .Load();

                _pensionDbContext
                    .Entry(ppoBill.Pensioner)
                    .Reference(entity => entity.Category)
                    .Load();
                _pensionDbContext
                    .Entry(ppoBill.Pensioner.Category)
                    .Reference(entity => entity.PrimaryCategory)
                    .Load();
                _pensionDbContext
                    .Entry(ppoBill.Pensioner.Category.PrimaryCategory)
                    .Reference(entity => entity.AccountHead)
                    .Load();
                _pensionDbContext
                    .Entry(ppoBill.Pensioner)
                    .Reference(entity => entity.Receipt)
                    .Load();

                response = _mapper.Map<T>(ppoBill);
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    new
                    {
                        ppoId,
                        financialYear,
                        treasuryCode,
                    },
                    $"RepositoryException: ",
                    ex
                );
            }
            return response;
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
                    response.FillErrorInDataSource(ppoBillEntity, "Pension bill not saved!");
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
                .AsSplitQuery()
                .FirstOrDefaultAsync(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                );
        }

        public T GeneratePensionBill<T>(
            Pensioner pensioner,
            PpoBillEntryDTO ppoBillEntryDTO,
            char billType,
            short financialYear,
            string treasuryCode
        )
        {
            InitiateFirstPensionBillResponseDTO response = new();
            try
            {
                if (pensioner.Category.ComponentRates.Count == 0)
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoBillEntryDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "Component rates not found!"
                    );
                    return _mapper.Map<T>(response);
                }

                List<PpoPaymentListItemDTO>? ppoPayments = PensionCalculator.CalculatePpoPayments(
                    pensioner.Category.ComponentRates,
                    billType == BillType.FirstBill
                        ? pensioner.DateOfCommencement
                        : PensionCalculator.CalculatePeriodStartDate(ppoBillEntryDTO.ToDate),
                    PensionCalculator.CalculatePeriodEndDate(ppoBillEntryDTO.ToDate).AddDays(1),
                    pensioner.BasicPensionAmount,
                    pensioner.CommutedPensionAmount
                );

                List<PpoBillBreakupResponseDTO> ppoBillBreakups =
                [
                    .. ppoPayments.Select(ppoPayment => new PpoBillBreakupResponseDTO
                    {
                        PpoId = pensioner.PpoId,
                        DrawnAmount = ppoPayment.DrawnAmount,
                        DueAmount = ppoPayment.DueAmount,
                        ComponentName = ppoPayment.ComponentName,
                        ComponentType = ppoPayment.ComponentType,
                        AmountPerMonth = ppoPayment.AmountPerMonth,
                        BaseAmount = ppoPayment.BaseAmount,
                        BreakupAmount = ppoPayment.AmountPerMonth * ppoPayment.PeriodInMonths,
                        FromDate = ppoPayment.FromDate,
                        ToDate = ppoPayment.ToDate,
                        Revision = new PpoComponentRevisionResponseDTO
                        {
                            AmountPerMonth = ppoPayment.AmountPerMonth,
                            FromDate = ppoPayment.FromDate,
                            RateId = ppoPayment.RateId,
                        },
                    }),
                ];

                response = new()
                {
                    PpoId = ppoBillEntryDTO.PpoId,
                    BillType = billType,
                    FromDate = pensioner.DateOfCommencement,
                    BranchId = pensioner.BranchId,
                    BillGeneratedUptoDate = ppoBillEntryDTO.ToDate,
                    TreasuryVoucherNo = "N/A",
                    BillDate = ppoBillEntryDTO.ToDate,
                    Id = 0,
                    GrossAmount = 0,
                    NetAmount = 0,
                };

                var pensionerResponse = _mapper.Map<PensionerResponseDTO>(pensioner);
                if (typeof(T) == typeof(InitiateFirstPensionBillResponseDTO))
                {
                    response.Pensioner = pensionerResponse;
                    response.PensionerPayments = ppoPayments;
                    response.GrossAmount = response.PensionerPayments.Sum(entity =>
                        entity.DueAmount
                    );
                    response.NetAmount = response.PensionerPayments.Sum(entity => entity.NetAmount);
                }
                else
                {
                    response.PpoBillBreakups = ppoBillBreakups;
                    response.GrossAmount = response.PpoBillBreakups.Sum(entity => entity.DueAmount);
                    response.NetAmount = response.PpoBillBreakups.Sum(entity => entity.NetAmount);
                }
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    new
                    {
                        ppoBillEntryDTO.PpoId,
                        treasuryCode,
                        financialYear,
                    },
                    "RepositoryException-GeneratePensionBill:",
                    ex
                );
            }

            return _mapper.Map<T>(response);
        }

        public async Task<Bill?> GetExistingBillForRegularBill(
            long hoaId,
            long branchId,
            DateOnly fromDate,
            DateOnly toDate,
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Bills.Where(entity =>
                    entity.ActiveFlag
                    && entity.AccountHeadId == hoaId
                    && entity.BranchId == branchId
                    && entity.FromDate == fromDate
                    && entity.ToDate == toDate
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .FirstOrDefaultAsync();
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

        public async Task<List<Bill>> GetSavedRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? bankId = null,
            long[]? branchIds = null
        )
        {
            return await _pensionDbContext
                .Bills.Where(entity =>
                    entity.ActiveFlag
                    && entity.FromDate == new DateOnly(year, month, 1)
                    && entity.ToDate == new DateOnly(year, month, DateTime.DaysInMonth(year, month))
                    && entity.TreasuryCode == treasuryCode
                    && entity.FinancialYear == financialYear
                    && (bankId == null || entity.Branch.Bank.Id == bankId)
                    && (
                        branchIds == null
                        || branchIds.Length == 0
                        || branchIds.Contains(entity.BranchId)
                    )
                )
                .Include(entity => entity.Branch)
                .ThenInclude(entity => entity.Bank)
                .Include(entity =>
                    entity.PpoBills.Where(entity =>
                        entity.ActiveFlag
                        && entity.BillType == BillType.RegularBill
                        && entity.TreasuryCode == treasuryCode
                        && (categoryId == null || entity.Pensioner.Category.Id == categoryId)
                    )
                )
                .ThenInclude(entity => entity.Pensioner)
                .ThenInclude(entity => entity.Category)
                .ThenInclude(entity => entity.PrimaryCategory)
                .ThenInclude(entity => entity.AccountHead)
                .Include(entity => entity.PpoBills)
                .ThenInclude(entity => entity.PpoBillBreakups)
                .ThenInclude(entity => entity.Revision)
                .ThenInclude(entity => entity.Rate)
                .ThenInclude(entity => entity.Breakup)
                .Where(entity => entity.PpoBills.Count > 0)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<Pensioner>> GetAvailablePensionersForBillGeneration(
            short year,
            short month,
            char billType,
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.FinancialYear == financialYear
                    && entity.PpoStatusFlags.Any(entity =>
                        entity.ActiveFlag && entity.StatusFlag == PensionStatusFlag.PpoRunning
                    )
                    && entity.PpoComponentRevisions.Any(entity => entity.ActiveFlag)
                    && entity.PpoStatusFlags.Count > 0
                    && entity.PpoComponentRevisions.Count > 0
                    && !_pensionDbContext
                        .PpoBills.Include(pb => pb.Bill)
                        .Any(pb => pb.PpoId == entity.PpoId && pb.BillType == billType)
                        ? (
                            _pensionDbContext
                                .PpoBills.Include(pb => pb.Bill)
                                .Any(pb =>
                                    pb.PpoId == entity.PpoId
                                    && pb.Bill.BillDate.Month == (month == 1 ? 12 : month - 1)
                                    && pb.Bill.BillDate.Year == (month == 1 ? year - 1 : year)
                                )
                        )
                        : (
                            !_pensionDbContext
                                .PpoBills.Include(pb => pb.Bill)
                                .Any(pb =>
                                    pb.PpoId == entity.PpoId
                                    && pb.BillType == billType
                                    && pb.Bill.FromDate.Month == month
                                    && pb.Bill.FromDate.Year == year
                                    && pb.Bill.ToDate.Month == month
                                    && pb.Bill.ToDate.Year == year
                                )
                            && (
                                _pensionDbContext
                                    .PpoBills.Include(pb => pb.Bill)
                                    .Any(pb =>
                                        pb.PpoId == entity.PpoId
                                        && pb.BillType == billType
                                        && pb.Bill.FromDate.Month == (month == 1 ? 12 : month - 1)
                                        && pb.Bill.FromDate.Year == (month == 1 ? year - 1 : year)
                                        && pb.Bill.ToDate.Month == (month == 1 ? 12 : month - 1)
                                        && pb.Bill.ToDate.Year == (month == 1 ? year - 1 : year)
                                    )
                            )
                        )
                )
                .Include(entity => entity.PpoStatusFlags.Where(entity => entity.ActiveFlag))
                .Include(entity => entity.PpoComponentRevisions.Where(entity => entity.ActiveFlag))
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<Pensioner>> GetPensionersForFirstBillPrint(
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoStatusFlags.Any(entity =>
                        entity.ActiveFlag && entity.StatusFlag == PensionStatusFlag.PpoApproved
                    )
                    && entity.PpoBills.Count > 0
                )
                .Include(entity => entity.PpoStatusFlags.Where(entity => entity.ActiveFlag))
                .Include(entity =>
                    entity.PpoBills.Where(entity =>
                        entity.ActiveFlag && entity.BillType == BillType.FirstBill
                    )
                )
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<Pensioner>> GetPensionersForFirstBillGeneration(
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoStatusFlags.Any(entity =>
                        entity.ActiveFlag && entity.StatusFlag == PensionStatusFlag.PpoApproved
                    )
                    && entity.PpoBills.Count == 0
                )
                .Include(entity => entity.PpoStatusFlags.Where(entity => entity.ActiveFlag))
                .Include(entity =>
                    entity.PpoBills.Where(entity =>
                        entity.ActiveFlag && entity.BillType == BillType.FirstBill
                    )
                )
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}
