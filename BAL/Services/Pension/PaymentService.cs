using System.Globalization;
using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CTS_BE.BAL.Services.Pension
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IPpoBillRepository _ppoBillRepository;
        private readonly IBankBranchRepository _bankBranchRepository;
        private readonly ITreasuryRepository _treasuryRepository;
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMqService _mqService;

        public PaymentService(
            PensionDbContext pensionDbContext,
            IClaimService claimService,
            IMapper mapper,
            IPpoBillRepository ppoBillRepository,
            IBankBranchRepository bankBranchRepository,
            ITreasuryRepository treasuryRepository,
            IMqService mqService
        )
            : base(claimService)
        {
            _pensionDbContext = pensionDbContext;
            _ppoBillRepository = ppoBillRepository;
            _bankBranchRepository = bankBranchRepository;
            _treasuryRepository = treasuryRepository;
            _mapper = mapper;
            _mqService = mqService;
        }

        public async Task<PpoBillResponseDTO> GetPaymentFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBill? ppoBillEntity = new();
            PpoBillResponseDTO ppoBillResponseDTO = new();
            try
            {
                ppoBillEntity = await _ppoBillRepository.GetPpoFirstBillByPpoId(
                    ppoId,
                    financialYear,
                    treasuryCode
                );

                if (ppoBillEntity == null)
                {
                    ppoBillResponseDTO.FillDataSource(
                        ppoBillEntity,
                        "Bill not found! Please add bill first or check PPO id."
                    );
                    return ppoBillResponseDTO;
                }
                ppoBillResponseDTO = _mapper.Map<PpoBillResponseDTO>(ppoBillEntity);

                ppoBillResponseDTO.BankBranchName =
                    await _bankBranchRepository.GetBankBranchNameByPpoId(treasuryCode, ppoId);
                ppoBillResponseDTO.BillNo = ppoBillEntity.Bill.BillNo;
                ppoBillResponseDTO.BillDate = ppoBillEntity.Bill.BillDate;
                ppoBillResponseDTO.FromDate = ppoBillEntity.Bill.FromDate;
                ppoBillResponseDTO.ToDate = ppoBillEntity.Bill.ToDate;
                ppoBillResponseDTO.TreasuryName = await _treasuryRepository.GetTreasuryNameAsync(
                    treasuryCode
                );
                ppoBillResponseDTO.TreasuryVoucherNo =
                    treasuryCode + "-" + ppoBillEntity.Bill.BillNo;
                ppoBillResponseDTO.TreasuryVoucherDate = ppoBillEntity.Bill.BillDate;
                ppoBillResponseDTO.AmountInWords = PensionCalculator.InWords(
                    ppoBillResponseDTO.NetAmount
                );
                ppoBillResponseDTO.PreparedBy = GetUserName();
                ppoBillResponseDTO.PreparedOn = DateOnly.FromDateTime(DateTime.Now);

                _mqService.Despatch("cts", JsonConvert.SerializeObject(ppoBillResponseDTO));

                return ppoBillResponseDTO;
            }
            catch (Exception ex)
            {
                ppoBillResponseDTO.FillDataSource(
                    ppoBillEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return ppoBillResponseDTO;
            }
        }

        public async Task<RegularBillListResponseDTO> GetPaymentRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? bankId = null,
            long[]? branchIds = null
        )
        {
            RegularBillListResponseDTO billListResponseDTO = new();
            List<Bill>? bills = null;
            Dictionary<string, string>? treasuryNameCache = []; // Cache for treasury names

            try
            {
                bills = await _pensionDbContext
                    .Bills.Where(entity =>
                        entity.ActiveFlag
                        && entity.FromDate == new DateOnly(year, month, 1)
                        && entity.ToDate
                            == new DateOnly(year, month, DateTime.DaysInMonth(year, month))
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
                    .AsSplitQuery()
                    .ToListAsync();

                billListResponseDTO.RegularBills = _mapper.Map<List<RegularBillResponseDTO>>(
                    bills.Where(entity => entity.PpoBills.Count > 0)
                );

                foreach (var regularBill in billListResponseDTO.RegularBills)
                {
                    // Check if the treasury name is already cached
                    if (!treasuryNameCache.TryGetValue(treasuryCode, out string? treasuryName))
                    {
                        treasuryName = await _treasuryRepository.GetTreasuryNameAsync(treasuryCode);
                        treasuryNameCache[treasuryCode] = treasuryName; // Cache the treasury name
                    }

                    regularBill.TreasuryName = treasuryName;
                    regularBill.TreasuryVoucherNo = treasuryCode + "-" + regularBill.BillNo;
                    regularBill.TreasuryVoucherDate = regularBill.BillDate;
                    regularBill.Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(
                        month
                    );
                    regularBill.Year = "" + year;
                    regularBill.BankBranchName =
                        regularBill.Branch?.Bank?.BankName + "-" + regularBill.Branch?.BranchName;
                    regularBill.Category =
                        ""
                        + regularBill
                            .PpoBills[0]
                            .Pensioner
                            ?.Category
                            ?.PrimaryCategory
                            .PrimaryCategoryName;

                    if (
                        bills
                            .FirstOrDefault()
                            ?.PpoBills.FirstOrDefault()
                            ?.Pensioner?.Category?.PrimaryCategory?.AccountHead != null
                    )
                    {
                        var accountHead = bills
                            .FirstOrDefault()
                            ?.PpoBills.FirstOrDefault()
                            ?.Pensioner?.Category?.PrimaryCategory?.AccountHead;
                        regularBill.CategoryDescription =
                            $"{accountHead?.MajorHead}-{accountHead?.SubmajorHead}-{accountHead?.MinorHead}-"
                            + $"{accountHead?.PlanStatus}-{accountHead?.SchemeHead}-{accountHead?.VotedCharged}-"
                            + $"{accountHead?.DetailHead}-{accountHead?.SubdetailHead}";
                    }
                    regularBill.PreparedBy = GetUserName();
                    regularBill.PreparedOn = DateOnly.FromDateTime(DateTime.Today);

                    foreach (var ppoBill in regularBill.PpoBills)
                    {
                        ppoBill.PpoNo = ppoBill.Pensioner?.PpoNo ?? "";
                        ppoBill.PensionerName = ppoBill.Pensioner?.PensionerName ?? "";
                        ppoBill.BankAcNo = ppoBill.Pensioner?.BankAcNo ?? "";
                        ppoBill.BasicPensionAmount = ppoBill.Pensioner?.BasicPensionAmount ?? 0;
                        ppoBill.CommutedPensionAmount =
                            ppoBill.Pensioner?.CommutedPensionAmount ?? 0;

                        foreach (var billBreakup in ppoBill.PpoBillBreakups)
                        {
                            switch (billBreakup.Revision?.Rate?.Breakup?.ComponentName)
                            {
                                case "BASIC PENSION":
                                    ppoBill.BasicPensionAmount = (int)billBreakup.BreakupAmount;
                                    break;
                                case "DEARNESS RELIEF":
                                    ppoBill.DearnessReliefAmount = (int)billBreakup.BreakupAmount;
                                    break;
                                case "MEDICAL RELIEF":
                                    ppoBill.MedicalReliefAmount = (int)billBreakup.BreakupAmount;
                                    break;
                            }
                        }
                        ppoBill.PpoBillBreakups = null!;
                        ppoBill.Pensioner = null!;
                    }

                    regularBill.GrossAmount = regularBill.PpoBills.Sum(bill =>
                        bill.TotalPayableAmount + bill.ByTransferAmount
                    );
                    regularBill.ByTransferAmount = regularBill.PpoBills.Sum(bill =>
                        bill.ByTransferAmount
                    );
                    regularBill.NetAmount = regularBill.PpoBills.Sum(bill =>
                        bill.TotalPayableAmount
                    );
                    regularBill.AmountInWords =
                        PensionCalculator.InWords(regularBill.NetAmount).Titleize() + " Only.";

                    regularBill.Branch = null;
                }
            }
            catch (Exception ex)
            {
                billListResponseDTO.FillDataSource(
                    new object(),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return billListResponseDTO;
            }
            _mqService.Despatch("cts", JsonConvert.SerializeObject(billListResponseDTO));

            return billListResponseDTO;
        }
    }
}
