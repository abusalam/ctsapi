using System.Globalization;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoRegularBillService(
        IClaimService claimService,
        IMapper mapper,
        IPpoRegularBillRepository ppoRegularBillRepository,
        ITreasuryRepository treasuryRepository,
        ILogger<PpoRegularBillService> logger
    ) : PpoBillService(claimService), IPpoRegularBillService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPpoRegularBillRepository _ppoRegularBillRepository =
            ppoRegularBillRepository;
        private readonly ITreasuryRepository _treasuryRepository = treasuryRepository;
        private readonly ILogger<PpoRegularBillService> _logger = logger;

        public async Task<RegularBillListResponseDTO> GetRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? bankId = null,
            long[]? branchIds = null
        )
        {
            _logger.LogInformation(
                "Received request to get regular pension bills for year: {Year}, month: {Month}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                year,
                month,
                financialYear,
                treasuryCode
            );
            RegularBillListResponseDTO billListResponseDTO = new();
            List<Bill>? bills = null;
            Dictionary<string, string>? treasuryNameCache = []; // Cache for treasury names

            try
            {
                bills = await _ppoRegularBillRepository.GetSavedRegularPensionBills(
                    year,
                    month,
                    financialYear,
                    treasuryCode,
                    categoryId,
                    bankId,
                    branchIds
                );

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
                                    ppoBill.BasicPensionAmount = billBreakup.BreakupAmount;
                                    break;
                                case "DEARNESS RELIEF":
                                    ppoBill.DearnessReliefAmount = billBreakup.BreakupAmount;
                                    break;
                                case "MEDICAL RELIEF":
                                    ppoBill.MedicalReliefAmount = billBreakup.BreakupAmount;
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
                _logger.LogError(
                    ex,
                    "Error occurred while fetching regular pension bills for year: {Year}, month: {Month}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                    year,
                    month,
                    financialYear,
                    treasuryCode
                );
                billListResponseDTO.FillErrorInDataSource(new { }, "ServiceException: ", ex);
                return billListResponseDTO;
            }

            return billListResponseDTO;
        }

        public async Task<T> GetPposForRegularBillGeneration<T>(
            short year,
            short month,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Received request to get PPOs for regular bill generation for year: {Year}, month: {Month}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                year,
                month,
                financialYear,
                treasuryCode
            );
            PpoListResponseDTO ppoListResponseDTO = new();

            try
            {
                List<Pensioner>? ppoList =
                    await _ppoRegularBillRepository.GetAvailablePensionersForRegularBillGeneration(
                        year,
                        month,
                        financialYear,
                        treasuryCode
                    );

                ppoListResponseDTO.PpoList = _mapper.Map<List<PensionerListItemDTO>>(ppoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPOs for regular bill generation for year: {Year}, month: {Month}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                    year,
                    month,
                    financialYear,
                    treasuryCode
                );
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillErrorInDataSource(new { }, "ServiceException: ", ex);
                return ppoBillResponseDTO;
            }
            _logger.LogInformation(
                "Successfully fetched PPOs for regular bill generation for year: {Year}, month: {Month}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                year,
                month,
                financialYear,
                treasuryCode
            );

            return _mapper.Map<T>(ppoListResponseDTO);
        }

        public async Task<T> SaveRegularPensionBill<T>(
            PpoBillEntryDTO ppoBillEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Received request to save regular pension bill with data: {PpoBillEntryDTO}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                ppoBillEntryDTO,
                financialYear,
                treasuryCode
            );
            PpoBillSaveResponseDTO response = new();
            try
            {
                if (
                    !await _ppoRegularBillRepository.IsFirstBillAlreadyGenerated(
                        ppoBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    _logger.LogWarning(
                        "First bill not generated for PPO ID: {PpoId}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                        ppoBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoBillEntryDTO.PpoId,
                            financialYear,
                            treasuryCode,
                        },
                        "First Bill not generated. Please check PPO ID or generate first bill."
                    );
                    return _mapper.Map<T>(response);
                }

                DateOnly lastBillDate = await _ppoRegularBillRepository.LastBillGeneratedUpTo(
                    ppoBillEntryDTO.PpoId,
                    treasuryCode
                );

                DateOnly billToBeGeneratedFromDate = DateOnly.FromDateTime(
                    new DateTime(ppoBillEntryDTO.Year, ppoBillEntryDTO.Month, 1)
                );

                if (lastBillDate.AddDays(1) != billToBeGeneratedFromDate)
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoBillEntryDTO.PpoId,
                            lastBillDate,
                            billToBeGeneratedFromDate,
                            treasuryCode,
                            financialYear,
                        },
                        $"Last Bill generated upto {lastBillDate.ToLongDateString()}. You cannot generate regular bill for {ppoBillEntryDTO.Month}/{ppoBillEntryDTO.Year}."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    await _ppoRegularBillRepository.IsRegularBillAlreadyGenerated(
                        ppoBillEntryDTO.PpoId,
                        ppoBillEntryDTO.Month,
                        ppoBillEntryDTO.Year,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    _logger.LogWarning(
                        "Regular bill already exists for PPO ID: {PpoId}, month: {Month}, year: {Year}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                        ppoBillEntryDTO.PpoId,
                        ppoBillEntryDTO.Month,
                        ppoBillEntryDTO.Year,
                        financialYear,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoBillEntryDTO.PpoId,
                            financialYear,
                            treasuryCode,
                        },
                        "Bill already exists. Please check PPO ID, bill date and bill type."
                    );
                    return _mapper.Map<T>(response);
                }

                Pensioner? pensioner = await _ppoRegularBillRepository.GetPensionerByPpoId(
                    ppoBillEntryDTO.PpoId,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    _logger.LogWarning(
                        "Pensioner not found for PPO ID: {PpoId}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                        ppoBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoBillEntryDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        $"Pensioner not found. Please check PPO ID: {ppoBillEntryDTO.PpoId} and try again."
                    );
                    return _mapper.Map<T>(response);
                }

                PpoBill ppoBillEntity =
                    _ppoRegularBillRepository.GenerateRegularPensionBill<PpoBill>(
                        pensioner,
                        ppoBillEntryDTO,
                        financialYear,
                        treasuryCode
                    );

                SetCreatedBy(ppoBillEntity);

                long hoaId = await _ppoRegularBillRepository.GetHoaIdByPpoId(
                    ppoBillEntryDTO.PpoId,
                    financialYear,
                    treasuryCode
                );

                ppoBillEntity.Bill =
                    await _ppoRegularBillRepository.GetExistingBillForRegularBill(
                        hoaId,
                        pensioner.BranchId,
                        DateOnly.FromDateTime(
                            new DateTime(ppoBillEntryDTO.Year, ppoBillEntryDTO.Month, 1)
                        ),
                        DateOnly.FromDateTime(
                            new DateTime(
                                ppoBillEntryDTO.Year,
                                ppoBillEntryDTO.Month,
                                DateTime.DaysInMonth(ppoBillEntryDTO.Year, ppoBillEntryDTO.Month)
                            )
                        ),
                        financialYear,
                        treasuryCode
                    )
                    ?? new()
                    {
                        ActiveFlag = true,
                        CreatedBy = ppoBillEntity.CreatedBy,
                        CreatedAt = DateTime.Now,
                        BillDate = ppoBillEntryDTO.ToDate,
                        TreasuryCode = treasuryCode,
                        FinancialYear = financialYear,
                        FromDate = DateOnly.FromDateTime(
                            new DateTime(ppoBillEntryDTO.Year, ppoBillEntryDTO.Month, 1)
                        ),
                        ToDate = DateOnly.FromDateTime(
                            new DateTime(
                                ppoBillEntryDTO.Year,
                                ppoBillEntryDTO.Month,
                                DateTime.DaysInMonth(ppoBillEntryDTO.Year, ppoBillEntryDTO.Month)
                            )
                        ),
                        BranchId = pensioner.BranchId,
                        AccountHeadId = hoaId,
                        BillNo = await _ppoRegularBillRepository.GetNextBillNo(
                            financialYear,
                            treasuryCode
                        ),
                    };

                var ppoComponentRevisions =
                    await _ppoRegularBillRepository.GetPpoComponentRevisionsByPensionerId(
                        pensioner.Id
                    );

                ppoBillEntity.ActiveFlag = true;
                ppoBillEntity.PensionerId = pensioner.Id;
                ppoBillEntity.PpoId = pensioner.PpoId;
                ppoBillEntity.FinancialYear = financialYear;
                ppoBillEntity.TreasuryCode = treasuryCode;
                ppoBillEntity.AccountHolderName = pensioner.AccountHolderName;
                ppoBillEntity.BankAcNo = pensioner.BankAcNo;
                ppoBillEntity.IfscCode = pensioner.Branch.IfscCode;
                ppoBillEntity.PaymentStatus = 'I';
                ppoBillEntity.CorrectionStatus = 'P';
                ppoBillEntity.FailedReason = "Processing";
                ppoBillEntity.Remarks = "Bill Generated";
                ppoBillEntity
                    .PpoBillBreakups.ToList()
                    .ForEach(entity =>
                    {
                        entity.ActiveFlag = true;
                        entity.Revision = PreparePpoComponentRevision(
                            entity.Revision,
                            ppoComponentRevisions,
                            pensioner.Id,
                            pensioner.PpoId,
                            ppoBillEntity.CreatedBy
                        );
                        entity.PpoId = pensioner.PpoId;
                        entity.RevisionId = entity.Revision.Id;
                        entity.FromDate = ppoBillEntity.Bill.FromDate;
                        entity.ToDate = ppoBillEntity.Bill.ToDate;
                        entity.TreasuryCode = treasuryCode;
                        entity.FinancialYear = financialYear;
                        entity.CreatedAt = DateTime.Now;
                        entity.CreatedBy = ppoBillEntity.CreatedBy;
                        entity.BreakupAmount = entity.Revision.AmountPerMonth;
                    });

                ppoBillEntity.GrossAmount = ppoBillEntity.PpoBillBreakups.Sum(entity =>
                    entity.BreakupAmount
                );

                ppoBillEntity.NetAmount =
                    ppoBillEntity.GrossAmount - ppoBillEntity.BytransferAmount;

                response = await _ppoRegularBillRepository.SavePpoBill<PpoBillSaveResponseDTO>(
                    ppoBillEntity,
                    financialYear,
                    treasuryCode
                );
                response.BillDate = ppoBillEntity.Bill.BillDate;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while saving regular pension bill for PPO ID: {PpoId}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                    ppoBillEntryDTO.PpoId,
                    financialYear,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    new
                    {
                        ppoBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode,
                    },
                    $"SaveRegularPensionBill-ServiceException: {ex.InnerException?.Message ?? ex.Message} ",
                    ex
                );
                return _mapper.Map<T>(response);
            }
            _logger.LogInformation(
                "Successfully saved regular pension bill for PPO ID: {PpoId}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                ppoBillEntryDTO.PpoId,
                financialYear,
                treasuryCode
            );
            return _mapper.Map<T>(response);
        }
    }
}
