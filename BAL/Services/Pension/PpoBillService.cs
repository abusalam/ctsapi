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
    public class PpoBillService(
        IClaimService claimService,
        IMapper mapper,
        IPpoBillRepository ppoBillRepository,
        IBankBranchRepository bankBranchRepository,
        ITreasuryRepository treasuryRepository
    ) : BaseService(claimService), IPpoBillService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPpoBillRepository _ppoBillRepository = ppoBillRepository;
        private readonly IBankBranchRepository _bankBranchRepository = bankBranchRepository;
        private readonly ITreasuryRepository _treasuryRepository = treasuryRepository;

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
            RegularBillListResponseDTO billListResponseDTO = new();
            List<Bill>? bills = null;
            Dictionary<string, string>? treasuryNameCache = []; // Cache for treasury names

            try
            {
                bills = await _ppoBillRepository.GetSavedRegularPensionBills(
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
                billListResponseDTO.FillErrorInDataSource(new { }, "ServiceException: ", ex);
                return billListResponseDTO;
            }

            return billListResponseDTO;
        }

        public async Task<T> GetPposForBillGeneration<T>(
            short year,
            short month,
            char billType,
            short financialYear,
            string treasuryCode
        )
        {
            PpoListResponseDTO ppoListResponseDTO = new();

            try
            {
                List<Pensioner>? ppoList =
                    await _ppoBillRepository.GetAvailablePensionersForBillGeneration(
                        year,
                        month,
                        billType,
                        financialYear,
                        treasuryCode
                    );

                ppoListResponseDTO.PpoList = _mapper.Map<List<PensionerListItemDTO>>(ppoList);
            }
            catch (Exception ex)
            {
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillErrorInDataSource(new { }, "ServiceException: ", ex);
                return ppoBillResponseDTO;
            }

            return _mapper.Map<T>(ppoListResponseDTO);
        }

        public async Task<T> GetPposForFirstBillGeneration<T>(
            short financialYear,
            string treasuryCode
        )
        {
            PpoListResponseDTO ppoListResponseDTO = new();

            try
            {
                List<Pensioner>? ppoList =
                    await _ppoBillRepository.GetPensionersForFirstBillGeneration(
                        financialYear,
                        treasuryCode
                    );

                ppoListResponseDTO.PpoList = _mapper.Map<List<PensionerListItemDTO>>(ppoList);
            }
            catch (Exception ex)
            {
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillErrorInDataSource(new { }, "ServiceException: ", ex);
                return ppoBillResponseDTO;
            }

            return _mapper.Map<T>(ppoListResponseDTO);
        }

        public async Task<T> GetPposForFirstBillPrint<T>(short financialYear, string treasuryCode)
        {
            PpoListResponseDTO ppoListResponseDTO = new();

            try
            {
                List<Pensioner>? ppoList = await _ppoBillRepository.GetPensionersForFirstBillPrint(
                    financialYear,
                    treasuryCode
                );

                ppoListResponseDTO.PpoList = _mapper.Map<List<PensionerListItemDTO>>(ppoList);
            }
            catch (Exception ex)
            {
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillErrorInDataSource(
                    ppoBillResponseDTO,
                    $"ServiceException:",
                    ex
                );
                return ppoBillResponseDTO;
            }

            return _mapper.Map<T>(ppoListResponseDTO);
        }

        public async Task<PpoBillResponseDTO> GetFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBillResponseDTO ppoBillResponseDTO = new();
            try
            {
                if (
                    !await _ppoBillRepository.IsFirstBillAlreadyGenerated(
                        ppoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    ppoBillResponseDTO.FillErrorInDataSource(
                        ppoId,
                        $"First Pension Bill not found! Please generate first pension bill or check PPO id: {ppoId}."
                    );
                }

                var ppoBillEntity = await _ppoBillRepository.GetPpoFirstBillByPpoId<PpoBill>(
                    ppoId,
                    financialYear,
                    treasuryCode
                );

                ppoBillResponseDTO = _mapper.Map<PpoBillResponseDTO>(ppoBillEntity);

                ppoBillResponseDTO.BillNo = ppoBillEntity.Bill.BillNo;
                ppoBillResponseDTO.BillDate = ppoBillEntity.Bill.BillDate;
                ppoBillResponseDTO.FromDate = ppoBillEntity.Bill.FromDate;
                ppoBillResponseDTO.ToDate = ppoBillEntity.Bill.ToDate;
                ppoBillResponseDTO.TreasuryVoucherNo =
                    $"TV-{ppoBillEntity.Bill.Id}-{ppoBillEntity.Id}";
                ppoBillResponseDTO.TreasuryVoucherDate = ppoBillEntity.Bill.BillDate;

                ppoBillResponseDTO.BankBranchName =
                    await _bankBranchRepository.GetBankBranchNameByPpoId(treasuryCode, ppoId);

                ppoBillResponseDTO.TreasuryName = await _treasuryRepository.GetTreasuryNameAsync(
                    treasuryCode
                );

                ppoBillResponseDTO.AmountInWords = PensionCalculator.InWords(
                    ppoBillResponseDTO.NetAmount
                );

                ppoBillResponseDTO.PreparedBy = GetUserName();
                ppoBillResponseDTO.PreparedOn = DateOnly.FromDateTime(DateTime.Now);

                return ppoBillResponseDTO;
            }
            catch (Exception ex)
            {
                ppoBillResponseDTO.FillErrorInDataSource(ppoId, $"ServiceException:", ex);
                return ppoBillResponseDTO;
            }
        }

        public async Task<T> SaveFirstPensionBill<T>(
            InitiateFirstPensionBillEntryDTO initiateFirstPensionBillDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBillSaveResponseDTO response = new();
            try
            {
                Pensioner? pensioner = await _ppoBillRepository.GetPensionerByPpoId(
                    initiateFirstPensionBillDTO.PpoId,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            initiateFirstPensionBillDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "Pensioner not found! Please check PPO ID."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    !await _ppoBillRepository.IsPpoApproved(
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            initiateFirstPensionBillDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "PPO is not Approved! Please check PPO ID or approve PPO."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    await _ppoBillRepository.IsFirstBillAlreadyGenerated(
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            initiateFirstPensionBillDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "First Pension Bill already exists! Please check PPO ID, bill date and bill type."
                    );
                    return _mapper.Map<T>(response);
                }

                PpoBill ppoBillEntity = _ppoBillRepository.GeneratePensionBill<PpoBill>(
                    pensioner,
                    new()
                    {
                        PpoId = initiateFirstPensionBillDTO.PpoId,
                        ToDate = initiateFirstPensionBillDTO.ToDate,
                    },
                    BillType.FirstBill,
                    financialYear,
                    treasuryCode
                );

                SetCreatedBy(ppoBillEntity);

                ppoBillEntity.Bill = new()
                {
                    ActiveFlag = true,
                    CreatedBy = ppoBillEntity.CreatedBy,
                    CreatedAt = DateTime.Now,
                    BillDate = initiateFirstPensionBillDTO.ToDate,
                    TreasuryCode = treasuryCode,
                    FinancialYear = financialYear,
                    FromDate = pensioner.DateOfCommencement,
                    ToDate = initiateFirstPensionBillDTO.ToDate,
                    AccountHeadId = await _ppoBillRepository.GetHoaIdByPpoId(
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    ),
                    BranchId = pensioner.BranchId,
                    BillNo = await _ppoBillRepository.GetNextBillNo(financialYear, treasuryCode),
                };

                pensioner.PpoStatusFlags.Add(
                    new PpoStatusFlag()
                    {
                        ActiveFlag = true,
                        TreasuryCode = treasuryCode,
                        FinancialYear = financialYear,
                        StatusFlag = PensionStatusFlag.PpoRunning,
                        PpoId = pensioner.PpoId,
                        StatusWef = DateOnly.FromDateTime(DateTime.Now),
                        CreatedAt = DateTime.Now,
                        CreatedBy = ppoBillEntity.CreatedBy,
                    }
                );

                ppoBillEntity.Pensioner = pensioner;

                List<PpoComponentRevision>? ppoComponentRevisions =
                    await _ppoBillRepository.GetPpoComponentRevisionsByPensionerId(pensioner.Id);
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
                        entity.RevisionId = entity.Revision.Id;
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

                SetCreatedBy(ppoBillEntity);

                long hoaId = await _ppoBillRepository.GetHoaIdByPpoId(
                    initiateFirstPensionBillDTO.PpoId,
                    financialYear,
                    treasuryCode
                );

                ppoBillEntity.Bill = new()
                {
                    ActiveFlag = true,
                    CreatedBy = ppoBillEntity.CreatedBy,
                    CreatedAt = DateTime.Now,
                    BillDate = initiateFirstPensionBillDTO.ToDate,
                    TreasuryCode = treasuryCode,
                    FinancialYear = financialYear,
                    FromDate = pensioner.DateOfCommencement,
                    ToDate = initiateFirstPensionBillDTO.ToDate,
                    AccountHeadId = hoaId,
                    BranchId = pensioner.BranchId,
                    BillNo = await _ppoBillRepository.GetNextBillNo(financialYear, treasuryCode),
                };

                response = await _ppoBillRepository.SavePpoBill<PpoBillSaveResponseDTO>(
                    ppoBillEntity,
                    financialYear,
                    treasuryCode
                );
                response.BillDate = ppoBillEntity.Bill.BillDate;
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    new
                    {
                        initiateFirstPensionBillDTO.PpoId,
                        treasuryCode,
                        financialYear,
                    },
                    $"ServiceException-SaveFirstPensionBill: ",
                    ex
                );
                return _mapper.Map<T>(response);
            }
            return _mapper.Map<T>(response);
        }

        private static PpoComponentRevision PreparePpoComponentRevision(
            PpoComponentRevision revision,
            List<PpoComponentRevision> ppoComponentRevisions,
            long pensionerId,
            int ppoId,
            int createdBy
        )
        {
            PpoComponentRevision? ppoComponentRevisionFound = ppoComponentRevisions.FirstOrDefault(
                entity => entity.RateId == revision.RateId
            );
            if (ppoComponentRevisionFound != null)
            {
                revision = ppoComponentRevisionFound;
                return revision;
            }
            revision.ActiveFlag = true;
            revision.PensionerId = pensionerId;
            revision.PpoId = ppoId;
            revision.CreatedBy = createdBy;
            revision.CreatedAt = DateTime.Now;
            return revision;
        }

        public async Task<T> SaveRegularPensionBill<T>(
            PpoBillEntryDTO ppoBillEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBillSaveResponseDTO response = new();
            try
            {
                DateOnly lastBillDate = await _ppoBillRepository.LastBillGeneratedUpTo(
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
                        $"Last Bill generated upto {lastBillDate.ToLongDateString()}. You cannot generate regular bill for {ppoBillEntryDTO.Month}/{ppoBillEntryDTO.Year}"
                    );
                    return _mapper.Map<T>(response);
                }

                Pensioner? pensioner = await _ppoBillRepository.GetPensionerByPpoId(
                    ppoBillEntryDTO.PpoId,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoBillEntryDTO.PpoId,
                            financialYear,
                            treasuryCode,
                        },
                        "Pensioner not found! Please check PPO ID."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    await _ppoBillRepository.IsRegularBillAlreadyGenerated(
                        ppoBillEntryDTO.PpoId,
                        ppoBillEntryDTO.Month,
                        ppoBillEntryDTO.Year,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoBillEntryDTO.PpoId,
                            financialYear,
                            treasuryCode,
                        },
                        "Bill already exists! Please check PPO ID, bill date and bill type."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    !await _ppoBillRepository.IsPpoApproved(
                        ppoBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoBillEntryDTO.PpoId,
                            financialYear,
                            treasuryCode,
                        },
                        "PPO is not Approved! Please check PPO ID or approve PPO."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    !await _ppoBillRepository.IsFirstBillAlreadyGenerated(
                        ppoBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoBillEntryDTO.PpoId,
                            financialYear,
                            treasuryCode,
                        },
                        "First Bill not generated! Please check PPO ID or generate first bill."
                    );
                    return _mapper.Map<T>(response);
                }

                PpoBill ppoBillEntity = _ppoBillRepository.GeneratePensionBill<PpoBill>(
                    pensioner,
                    ppoBillEntryDTO,
                    BillType.RegularBill,
                    financialYear,
                    treasuryCode
                );

                SetCreatedBy(ppoBillEntity);

                long hoaId = await _ppoBillRepository.GetHoaIdByPpoId(
                    ppoBillEntryDTO.PpoId,
                    financialYear,
                    treasuryCode
                );

                ppoBillEntity.Bill =
                    await _ppoBillRepository.GetExistingBillForRegularBill(
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
                        BillNo = await _ppoBillRepository.GetNextBillNo(
                            financialYear,
                            treasuryCode
                        ),
                    };

                var ppoComponentRevisions =
                    await _ppoBillRepository.GetPpoComponentRevisionsByPensionerId(pensioner.Id);

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

                response = await _ppoBillRepository.SavePpoBill<PpoBillSaveResponseDTO>(
                    ppoBillEntity,
                    financialYear,
                    treasuryCode
                );
                response.BillDate = ppoBillEntity.Bill.BillDate;
            }
            catch (Exception ex)
            {
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
            return _mapper.Map<T>(response);
        }

        public async Task<T> GenerateFirstPensionBill<T>(
            InitiateFirstPensionBillEntryDTO initiateFirstPensionBillDTO,
            short financialYear,
            string treasuryCode
        )
        {
            InitiateFirstPensionBillResponseDTO response = new();
            try
            {
                Pensioner? pensioner = await _ppoBillRepository.GetPensionerByPpoId(
                    initiateFirstPensionBillDTO.PpoId,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            initiateFirstPensionBillDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "Pensioner not found! Please check PPO ID."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    !await _ppoBillRepository.IsPpoApproved(
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            initiateFirstPensionBillDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "PPO is not Approved! Please check PPO ID or approve PPO."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    await _ppoBillRepository.IsFirstBillAlreadyGenerated(
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            initiateFirstPensionBillDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "First Bill already generated! Please check PPO ID."
                    );
                    return _mapper.Map<T>(response);
                }

                response =
                    _ppoBillRepository.GeneratePensionBill<InitiateFirstPensionBillResponseDTO>(
                        pensioner,
                        new()
                        {
                            PpoId = initiateFirstPensionBillDTO.PpoId,
                            ToDate = initiateFirstPensionBillDTO.ToDate,
                        },
                        BillType.FirstBill,
                        financialYear,
                        treasuryCode
                    );

                response.BillDate = initiateFirstPensionBillDTO.ToDate;
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    new
                    {
                        initiateFirstPensionBillDTO.PpoId,
                        treasuryCode,
                        financialYear,
                    },
                    $"ServiceException-GenerateFirstPensionBill: ",
                    ex
                );
                return _mapper.Map<T>(response);
            }
            return _mapper.Map<T>(response);
        }
    }
}
