using AutoMapper;
using CTS_BE.BAL.Services.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoRegularBillRepository : PpoBillRepository, IPpoRegularBillRepository
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;
        private readonly ILogger<PpoRegularBillRepository> _logger;

        public PpoRegularBillRepository(
            PensionDbContext context,
            IMapper mapper,
            IClaimService claimService,
            ILogger<PpoRegularBillRepository> logger
        )
            : base(context, mapper, logger)
        {
            _pensionDbContext = context;
            _mapper = mapper;
            _claimService = claimService;
            _logger = logger;
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

        public async Task<List<Pensioner>> GetAvailablePensionersForRegularBillGeneration(
            short year,
            short month,
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
                        .Any(pb => pb.PpoId == entity.PpoId && pb.BillType == BillType.RegularBill)
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
                                    && pb.BillType == BillType.RegularBill
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
                                        && pb.BillType == BillType.RegularBill
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

        public T GenerateRegularPensionBill<T>(
            Pensioner pensioner,
            PpoBillEntryDTO ppoBillEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBill ppoRegularBill = new();
            InitiateFirstPensionBillResponseDTO response = new();
            try
            {
                response = new()
                {
                    BillType = BillType.RegularBill,
                    PpoId = pensioner.PpoId,
                    GrossAmount = 0,
                    NetAmount = 0,
                    PpoBillBreakups = PensionCalculator.CalculatePpoBillBreakupsForRegularBill(
                        pensioner.PpoComponentRevisions,
                        new DateOnly(ppoBillEntryDTO.Year, ppoBillEntryDTO.Month, 1),
                        new DateOnly(
                            ppoBillEntryDTO.Year,
                            ppoBillEntryDTO.Month,
                            DateTime.DaysInMonth(ppoBillEntryDTO.Year, ppoBillEntryDTO.Month)
                        )
                    ),
                };
                response.GrossAmount = response.PpoBillBreakups.Sum(entity => entity.BreakupAmount);
                response.NetAmount = response.PpoBillBreakups.Sum(entity => entity.BreakupAmount);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while calculating PPO bill breakups for PPO ID: {PpoId}, month: {Month}, year: {Year}",
                    pensioner.PpoId,
                    ppoBillEntryDTO.Month,
                    ppoBillEntryDTO.Year
                );
                response.FillErrorInDataSource(
                    new
                    {
                        ppoBillEntryDTO.PpoId,
                        treasuryCode,
                        financialYear,
                    },
                    "RepositoryException-GenerateRegularPensionBill:",
                    ex
                );
            }

            return _mapper.Map<T>(response);
        }
    }
}
