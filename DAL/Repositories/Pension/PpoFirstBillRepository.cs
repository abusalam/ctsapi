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
    public class PpoFirstBillRepository(PensionDbContext context, IMapper mapper)
        : PpoBillRepository(context, mapper),
            IPpoFirstBillRepository
    {
        private readonly PensionDbContext _pensionDbContext = context;
        private readonly IMapper _mapper = mapper;

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

        public T GenerateFirstPensionBill<T>(
            Pensioner pensioner,
            InitiateFirstPensionBillEntryDTO ppoFirstBillEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            InitiateFirstPensionBillResponseDTO response = new();
            try
            {
                response = new()
                {
                    PpoId = ppoFirstBillEntryDTO.PpoId,
                    BillType = BillType.FirstBill,
                    FromDate = pensioner.DateOfCommencement,
                    BranchId = pensioner.BranchId,
                    BillGeneratedUptoDate = ppoFirstBillEntryDTO.ToDate,
                    TreasuryVoucherNo = "N/A",
                    BillDate = ppoFirstBillEntryDTO.ToDate,
                    Id = 0,
                    GrossAmount = 0,
                    NetAmount = 0,
                    Pensioner = _mapper.Map<PensionerResponseDTO>(pensioner),
                    PensionerPayments = PensionCalculator.CalculatePpoPaymentsForFirstBill(
                        pensioner.Category.ComponentRates,
                        pensioner.DateOfCommencement,
                        PensionCalculator
                            .CalculatePeriodEndDate(ppoFirstBillEntryDTO.ToDate)
                            .AddDays(1),
                        pensioner.BasicPensionAmount,
                        pensioner.CommutedPensionAmount
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
                        ppoFirstBillEntryDTO.PpoId,
                        treasuryCode,
                        financialYear,
                    },
                    "RepositoryException-GeneratePensionBill:",
                    ex
                );
            }

            return _mapper.Map<T>(response);
        }
    }
}
