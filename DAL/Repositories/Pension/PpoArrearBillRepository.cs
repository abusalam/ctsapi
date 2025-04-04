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
    public class PpoArrearBillRepository(PensionDbContext context, IMapper mapper)
        : PpoBillRepository(context, mapper),
            IPpoArrearBillRepository
    {
        private readonly PensionDbContext _pensionDbContext = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Pensioner?> GetPpoArrearBillByPpoId(
            int ppoId,
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
                .Include(entity => entity.Branch)
                .ThenInclude(entity => entity.Bank)
                .Include(entity => entity.PpoStatusFlags.Where(status => status.ActiveFlag))
                .FirstOrDefaultAsync();
        }

        public async Task<List<Pensioner>> GetPensionersForArrearBillGeneration(
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoStatusFlags.Any(entity =>
                        entity.ActiveFlag && entity.StatusFlag == PensionStatusFlag.PpoRunning
                    )
                    && entity.PpoBills.Any(entity => entity.ActiveFlag)
                )
                .Include(entity => entity.PpoStatusFlags.Where(entity => entity.ActiveFlag))
                .Include(entity => entity.PpoBills.Where(entity => entity.ActiveFlag))
                .Include(entity => entity.Branch)
                .ThenInclude(entity => entity.Bank)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<Pensioner>> GetPensionersForArrearBillPrint(
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Pensioners.Where(p => p.ActiveFlag && p.TreasuryCode == treasuryCode)
                .Where(p =>
                    p.PpoStatusFlags.Any(s =>
                        s.ActiveFlag && s.StatusFlag == PensionStatusFlag.PpoRunning
                    )
                )
                .Where(p => p.PpoBills.Any(b => b.ActiveFlag && b.BillType == BillType.ArrearBill))
                .Include(p => p.PpoStatusFlags.Where(s => s.ActiveFlag))
                .Include(p =>
                    p.PpoBills.Where(b => b.ActiveFlag && b.BillType == BillType.ArrearBill)
                )
                .Include(p => p.Branch.Bank)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<T> GenerateArrearPensionBill<T>(
            Pensioner pensioner,
            PpoArrearBillEntryDTO ppoArrearBillEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            InitiateFirstPensionBillResponseDTO response = new();
            try
            {
                response = new()
                {
                    PpoId = ppoArrearBillEntryDTO.PpoId,
                    BillType = BillType.ArrearBill,
                    FromDate = ppoArrearBillEntryDTO.PeriodFrom,
                    BranchId = pensioner.BranchId,
                    BillGeneratedUptoDate = ppoArrearBillEntryDTO.PeriodTo,
                    TreasuryVoucherNo = "N/A",
                    BillDate = ppoArrearBillEntryDTO.PeriodTo,
                    Id = pensioner.Id,
                    GrossAmount = 0,
                    NetAmount = 0,
                    Pensioner = _mapper.Map<PensionerResponseDTO>(pensioner),
                    PensionerPayments = await PensionCalculator.CalculatePpoPaymentsForArrearBill(
                        pensioner.Category.ComponentRates,
                        pensioner.DateOfCommencement,
                        PensionCalculator
                            .CalculatePeriodEndDate(ppoArrearBillEntryDTO.PeriodTo)
                            .AddDays(1),
                        pensioner.BasicPensionAmount,
                        pensioner.CommutedPensionAmount,
                        ppoArrearBillEntryDTO.PpoId,
                        this
                    ),
                };

                response.GrossAmount = response.PensionerPayments.Sum(entity => entity.DueAmount);
                response.NetAmount = response.PensionerPayments.Sum(entity => entity.NetAmount);

                if (typeof(T) != typeof(InitiateFirstPensionBillResponseDTO))
                {
                    response.PpoBillBreakups =
                    [
                        .. response.PensionerPayments.Select(
                            ppoPayment => new PpoBillBreakupResponseDTO
                            {
                                PpoId = pensioner.PpoId,
                                DrawnAmount = ppoPayment.DrawnAmount,
                                DueAmount = ppoPayment.DueAmount,
                                ComponentName = ppoPayment.ComponentName,
                                ComponentType = ppoPayment.ComponentType,
                                AmountPerMonth = ppoPayment.AmountPerMonth,
                                BaseAmount = ppoPayment.BaseAmount,
                                BreakupAmount =
                                    ppoPayment.AmountPerMonth * ppoPayment.PeriodInMonths,
                                FromDate = ppoPayment.FromDate,
                                ToDate = ppoPayment.ToDate,
                                Revision = new PpoComponentRevisionResponseDTO
                                {
                                    AmountPerMonth = ppoPayment.AmountPerMonth,
                                    FromDate = ppoPayment.FromDate,
                                    RateId = ppoPayment.RateId,
                                },
                            }
                        ),
                    ];
                }
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    new
                    {
                        ppoArrearBillEntryDTO.PpoId,
                        treasuryCode,
                        financialYear,
                    },
                    "RepositoryException- GenerateArrearPensionBill:",
                    ex
                );
            }

            return _mapper.Map<T>(response);
        }

        public async Task<T> GetArrearBillByPpoIdForPrintAsync<T>(
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
                        && entity.BillType == BillType.ArrearBill
                        && entity.PpoId == ppoId
                        && entity.FinancialYear == financialYear
                        && entity.TreasuryCode == treasuryCode
                    )
                    .FirstAsync();

                _pensionDbContext
                    .PpoBills.Include(entity => entity.Bill)
                    .Include(entity => entity.PpoBillBreakups)
                    .ThenInclude(entity => entity.Revision)
                    .ThenInclude(entity => entity.Rate)
                    .ThenInclude(entity => entity.Breakup)
                    .Load();

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
                    $"RepositoryException- GetArrearBillByPpoIdForPrintAsync: ",
                    ex
                );
            }
            return response;
        }

        public async Task<BreakupAmountDto> GetBreakupAmountForPeriodAndComponent(
            int ppoId,
            DateOnly fromDate,
            DateOnly toDate,
            string componentName
        )
        {
            var response = new BreakupAmountDto();
            int? breakupAmount = null;

            try
            {
                breakupAmount = await _pensionDbContext
                    .PpoBillBreakups.Where(ppb =>
                        ppb.PpoId == ppoId
                        && ppb.ActiveFlag
                        && ppb.FromDate <= toDate
                        && ppb.ToDate >= fromDate
                        && ppb.Revision.Rate.Breakup.ComponentName == componentName
                        && ppb.Revision.ActiveFlag
                        && ppb.Revision.Rate.ActiveFlag
                        && ppb.Revision.Rate.Breakup.ActiveFlag
                    )
                    .OrderByDescending(ppb => ppb.CreatedAt)
                    .Select(ppb => ppb.BreakupAmount)
                    .FirstOrDefaultAsync();

                response.Amount = breakupAmount;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    breakupAmount,
                    $"Error retrieving breakup amount for PPO {ppoId}, component {componentName}:",
                    ex
                );
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
