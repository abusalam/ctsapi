using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionBillService : BaseService, IPensionBillService
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;

        public PensionBillService(
            PensionDbContext pensionDbContext,
            IClaimService claimService,
            IMapper mapper
        )
            : base(claimService)
        {
            _pensionDbContext = pensionDbContext;
            _mapper = mapper;
        }

        public async Task<T> GenerateFirstPensionBill<T>(
            InitiateFirstPensionBillDTO initiateFirstPensionBillDTO,
            char billType,
            short financialYear,
            string treasuryCode
        )
            where T : PensionerFirstBillResponseDTO
        {
            Pensioner? pensioner = await _pensionDbContext
                .Pensioners.Include(p => p.Branch)
                .ThenInclude(b => b.Bank)
                .Include(p => p.Category)
                .ThenInclude(c => c.PrimaryCategory)
                .ThenInclude(pc => pc.AccountHead)
                .Include(p => p.Category)
                .ThenInclude(c => c.SubCategory)
                .Include(p => p.Receipt)
                .Include(p => p.Category.ComponentRates.Where(cr => cr.ActiveFlag))
                .FirstOrDefaultAsync(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == initiateFirstPensionBillDTO.PpoId
                    && entity.TreasuryCode == treasuryCode
                );

            if (pensioner == null)
            {
                InitiateFirstPensionBillResponseDTO errResponse = new();
                errResponse.FillDataSource(pensioner, "Pensioner not found!");
                return _mapper.Map<T>(errResponse);
            }

            // Load Breakups for each ComponentRate
            foreach (var componentRate in pensioner.Category.ComponentRates)
            {
                await _pensionDbContext
                    .Entry(componentRate)
                    .Reference(cr => cr.Breakup)
                    .LoadAsync();
            }

            List<PpoPaymentListItemDTO>? ppoPayments = PensionCalculator.CalculatePpoPayments(
                pensioner.Category.ComponentRates,
                billType == BillType.FirstBill
                    ? pensioner.DateOfCommencement
                    : PensionCalculator.CalculatePeriodStartDate(
                        initiateFirstPensionBillDTO.ToDate
                    ),
                PensionCalculator
                    .CalculatePeriodEndDate(initiateFirstPensionBillDTO.ToDate)
                    .AddDays(1),
                pensioner.BasicPensionAmount,
                pensioner.CommutedPensionAmount
            );

            List<PpoBillBreakupResponseDTO> ppoBillBreakups = ppoPayments
                .Select(ppoPayment => new PpoBillBreakupResponseDTO
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
                })
                .ToList();

            InitiateFirstPensionBillResponseDTO response = new()
            {
                PpoId = initiateFirstPensionBillDTO.PpoId,
                BillType = billType,
                FromDate = pensioner.DateOfCommencement,
                BranchId = pensioner.BranchId,
                BillGeneratedUptoDate = initiateFirstPensionBillDTO.ToDate,
                TreasuryVoucherNo = "N/A",
                BillDate = initiateFirstPensionBillDTO.ToDate,
                Id = 0,
                GrossAmount = 0,
                NetAmount = 0,
            };

            if (typeof(T) == typeof(InitiateFirstPensionBillResponseDTO))
            {
                var pensionerResponse = _mapper.Map<PensionerResponseDTO>(pensioner);
                response.Pensioner = pensionerResponse;
                response.PensionerPayments = ppoPayments;
                response.GrossAmount = response.PensionerPayments.Sum(entity => entity.DueAmount);
                response.NetAmount = response.PensionerPayments.Sum(entity => entity.NetAmount);
            }
            else
            {
                response.PpoBillBreakups = ppoBillBreakups;
                response.GrossAmount = response.PpoBillBreakups.Sum(entity => entity.DueAmount);
                response.NetAmount = response.PpoBillBreakups.Sum(entity => entity.NetAmount);
            }

            return _mapper.Map<T>(response);
        }

        public async Task<T> SavePensionBill<T>(
            InitiateFirstPensionBillDTO initiateFirstPensionBillDTO,
            char billType,
            short financialYear,
            string treasuryCode
        )
            where T : PensionerFirstBillResponseDTO
        {
            Pensioner? pensioner = await _pensionDbContext
                .Pensioners.Include(p => p.Branch)
                .ThenInclude(b => b.Bank)
                .Include(p => p.Category)
                .ThenInclude(c => c.PrimaryCategory)
                .Include(p => p.Category)
                .ThenInclude(c => c.SubCategory)
                .Include(p => p.Receipt)
                .Include(p => p.Category.ComponentRates.Where(cr => cr.ActiveFlag))
                .FirstOrDefaultAsync(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == initiateFirstPensionBillDTO.PpoId
                    && entity.TreasuryCode == treasuryCode
                );

            if (pensioner == null)
            {
                InitiateFirstPensionBillResponseDTO errResponse = new();
                errResponse.FillDataSource(pensioner, "Pensioner not found!");
                return _mapper.Map<T>(errResponse);
            }

            // Load Breakups for each ComponentRate
            foreach (var componentRate in pensioner.Category.ComponentRates)
            {
                await _pensionDbContext
                    .Entry(componentRate)
                    .Reference(cr => cr.Breakup)
                    .LoadAsync();
            }

            List<PpoPaymentListItemDTO>? ppoPayments = PensionCalculator.CalculatePpoPayments(
                pensioner.Category.ComponentRates,
                billType == BillType.FirstBill
                    ? pensioner.DateOfCommencement
                    : PensionCalculator.CalculatePeriodStartDate(
                        initiateFirstPensionBillDTO.ToDate
                    ),
                PensionCalculator
                    .CalculatePeriodEndDate(initiateFirstPensionBillDTO.ToDate)
                    .AddDays(1),
                pensioner.BasicPensionAmount,
                pensioner.CommutedPensionAmount
            );

            List<PpoBillBreakupResponseDTO> ppoBillBreakups = ppoPayments
                .Select(ppoPayment => new PpoBillBreakupResponseDTO
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
                })
                .ToList();

            InitiateFirstPensionBillResponseDTO response = new()
            {
                PpoId = initiateFirstPensionBillDTO.PpoId,
                BillType = billType,
                FromDate = pensioner.DateOfCommencement,
                BranchId = pensioner.BranchId,
                BillGeneratedUptoDate = initiateFirstPensionBillDTO.ToDate,
                TreasuryVoucherNo = "N/A",
                BillDate = initiateFirstPensionBillDTO.ToDate,
                Id = 0,
                GrossAmount = 0,
                NetAmount = 0,
            };

            if (typeof(T) == typeof(InitiateFirstPensionBillResponseDTO))
            {
                response.Pensioner = _mapper.Map<PensionerResponseDTO>(pensioner);
                response.PensionerPayments = ppoPayments;
                response.GrossAmount = response.PensionerPayments.Sum(entity => entity.DueAmount);
                response.NetAmount = response.PensionerPayments.Sum(entity => entity.NetAmount);
            }
            else
            {
                response.PpoBillBreakups = ppoBillBreakups;
                response.GrossAmount = response.PpoBillBreakups.Sum(entity => entity.DueAmount);
                response.NetAmount = response.PpoBillBreakups.Sum(entity => entity.NetAmount);
            }

            return _mapper.Map<T>(response);
        }
    }
}
