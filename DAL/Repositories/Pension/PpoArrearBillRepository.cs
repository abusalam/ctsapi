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

        public T GenerateArrearPensionBill<T>(
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
                    //BillType = BillType.FirstBill,
                    FromDate = pensioner.DateOfCommencement,
                    BranchId = pensioner.BranchId,
                    BillGeneratedUptoDate = ppoArrearBillEntryDTO.PeriodTo,
                    TreasuryVoucherNo = "N/A",
                    BillDate = ppoArrearBillEntryDTO.PeriodTo,
                    Id = 0,
                    GrossAmount = 0,
                    NetAmount = 0,
                    Pensioner = _mapper.Map<PensionerResponseDTO>(pensioner),
                    PensionerPayments = PensionCalculator.CalculatePpoPaymentsForFirstBill(
                        pensioner.Category.ComponentRates,
                        pensioner.DateOfCommencement,
                        PensionCalculator
                            .CalculatePeriodEndDate(ppoArrearBillEntryDTO.PeriodTo)
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
                        ppoArrearBillEntryDTO.PpoId,
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
