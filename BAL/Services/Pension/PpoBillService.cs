using System.Globalization;
using AutoMapper;
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

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoBillService : BaseService, IPpoBillService
    {
        private readonly IMapper _mapper;
        private readonly IPpoBillRepository _ppoBillRepository;
        private readonly IBankBranchRepository _bankBranchRepository;
        private readonly PensionDbContext _pensionDbContext;
        public PpoBillService(
            IPpoBillRepository ppoBillRepository,
            IBankBranchRepository bankBranchRepository,
            IMapper mapper,
            IClaimService claimService
        ) : base(claimService)
        {
            _mapper = mapper;
            _ppoBillRepository = ppoBillRepository;
            _bankBranchRepository = bankBranchRepository;
            _pensionDbContext = (PensionDbContext) this._ppoBillRepository.GetDbContext();
        }

        public async Task<RegularBillListResponseDTO> GetRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? branchId = null
        )
        {
            RegularBillListResponseDTO billListResponseDTO = new();
            List<Bill>? bills = null;
            try {

                bills = await _pensionDbContext.Bills
                .Where(
                    entity => entity.ActiveFlag
                    && entity.FromDate == new DateOnly(year, month, 1)
                    && entity.ToDate == new DateOnly(year, month, DateTime.DaysInMonth(year, month))
                    && entity.TreasuryCode == treasuryCode
                    && entity.FinancialYear == financialYear
                )
                .Include(
                    entity => entity.Branch
                )
                .ThenInclude(
                    entity => entity.Bank
                )
                .Include(
                    entity => entity.PpoBills.Where(
                        entity => entity.ActiveFlag
                        && entity.BillType == BillType.RegularBill
                        && entity.TreasuryCode == treasuryCode
                        && (branchId == null || entity.Pensioner.BranchId == branchId)
                        && (categoryId == null || entity.Pensioner.Category.Id == categoryId)
                    )
                )
                .ThenInclude(
                    entity => entity.Pensioner
                )
                .ThenInclude(
                    entity => entity.Category
                )
                .ThenInclude(
                    entity => entity.PrimaryCategory
                )
                .Include(
                    entity => entity.PpoBills
                )
                .ThenInclude(
                    entity => entity.PpoBillBreakups
                )
                .ThenInclude(
                    entity => entity.Revision
                )
                .ThenInclude(
                    entity => entity.Rate
                )
                .ThenInclude(
                    entity => entity.Breakup
                )
                .AsSplitQuery()
                .ToListAsync();


                billListResponseDTO.RegularBills = _mapper.Map<List<RegularBillResponseDTO>>(bills);
                billListResponseDTO.RegularBills.ForEach(
                    regularBill => {
                        regularBill.TreasuryName = "Malda - I";
                        regularBill.Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                        regularBill.Year = "" + year;
                        regularBill.BankBranchName = regularBill.Branch?.Bank?.BankName + "-" + regularBill.Branch?.BranchName;
                        regularBill.Category = "" + regularBill.PpoBills[0].Pensioner?.Category?.PrimaryCategory.PrimaryCategoryName;
                        regularBill.PreparedBy = GetUserName();
                        regularBill.PreparedOn = DateOnly.FromDateTime(DateTime.Today);
                        
                        regularBill.PpoBills.ForEach(
                            ppoBill => {
                                ppoBill.PpoNo = ppoBill.Pensioner?.PpoNo ?? "";
                                ppoBill.PensionerName = ppoBill.Pensioner?.PensionerName ?? "";
                                ppoBill.BankAcNo = ppoBill.Pensioner?.BankAcNo ?? "";
                                ppoBill.BasicPensionAmount = ppoBill.Pensioner?.BasicPensionAmount ?? 0;
                                ppoBill.CommutedPensionAmount = ppoBill.Pensioner?.CommutedPensionAmount ?? 0;
                                ppoBill.PpoBillBreakups.ForEach(
                                    billBreakup => {
                                        switch (billBreakup.Revision?.Rate?.Breakup?.ComponentName) {
                                            case "BASIC PENSION":
                                            ppoBill.BasicPensionAmount = (int) billBreakup.BreakupAmount; 
                                            break;
                                            case "DEARNESS RELIEF":
                                            ppoBill.DearnessReliefAmount = (int) billBreakup.BreakupAmount;
                                            break;
                                            case "MEDICAL RELIEF":
                                            ppoBill.MedicalReliefAmount = (int) billBreakup.BreakupAmount;
                                            break;
                                            // case "AMOUNT COMMUTED":
                                            // ppoBill.CommutedPensionAmount = (int) billBreakup.BreakupAmount; 
                                            // break;
                                        }
                                    }
                                );
                                ppoBill.PpoBillBreakups = null!;
                                ppoBill.Pensioner = null!;
                            }
                        );

                        regularBill.GrossAmount = regularBill.PpoBills.Sum(bill => bill.TotalPayableAmount + bill.ByTransferAmount);
                        regularBill.ByTransferAmount = regularBill.PpoBills.Sum(bill => bill.ByTransferAmount);
                        regularBill.NetAmount = regularBill.PpoBills.Sum(bill => bill.TotalPayableAmount);
                        regularBill.AmountInWords = PensionCalculator.InWords(regularBill.NetAmount).Titleize() + " Only.";
                        
                        regularBill.Branch = null;
                    }
                );
            }
            catch (Exception ex) {
                billListResponseDTO.FillDataSource(
                    new object(),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return billListResponseDTO;
            }

            return billListResponseDTO;
        }

        public async Task<T> GetAllPposForBillGeneration<T>(
            short year,
            short month,
            char billType,
            short financialYear,
            string treasuryCode
        ) where T : BaseDTO
        {

            PpoListResponseDTO ppoListResponseDTO = new();

            try {

                var ppoList = await _pensionDbContext.Pensioners
                .Where(
                    entity => entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoStatusFlags.Any(
                        entity => entity.ActiveFlag
                        && entity.StatusFlag == PensionStatusFlag.PpoRunning
                    )
                    && entity.PpoComponentRevisions.Any(
                        entity => entity.ActiveFlag
                    )
                    && entity.PpoStatusFlags.Count > 0
                    && entity.PpoComponentRevisions.Count > 0
                )
                .Include(
                    entity => entity.PpoStatusFlags
                    .Where(
                        entity => entity.ActiveFlag
                    )
                )
                .Include(
                    entity => entity.PpoComponentRevisions
                    .Where(
                        entity => entity.ActiveFlag
                    )
                )
                .ToListAsync();

                ppoListResponseDTO.PpoList = _mapper.Map<List<PensionerListItemDTO>>(ppoList);
            }
            catch (Exception ex) {
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillDataSource(
                    ppoBillResponseDTO,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return ppoBillResponseDTO;
            }

            return _mapper.Map<T>(ppoListResponseDTO);
        }

        public async Task<T> GetAllPposForFirstBillGeneration<T>(
            short financialYear,
            string treasuryCode
        ) where T : BaseDTO
        {

            PpoListResponseDTO ppoListResponseDTO = new();

            try {

                List<Pensioner>? ppoList = await _pensionDbContext.Pensioners
                .Where(
                    entity => entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoStatusFlags.Any(
                        entity => entity.ActiveFlag
                        && entity.StatusFlag == PensionStatusFlag.PpoApproved
                    )
                    && entity.PpoBills.Count == 0
                )
                .Include(
                    entity => entity.PpoStatusFlags
                    .Where(
                        entity => entity.ActiveFlag
                    )
                )
                .Include(
                    entity => entity.PpoBills
                    .Where(
                        entity => entity.ActiveFlag
                        && entity.BillType == BillType.FirstBill
                    )
                )
                // .Select(
                //     entity => new {
                //         entity.Id,
                //         entity.PpoId,
                //         entity.PpoNo,
                //         entity.PensionerName,
                //         entity.MobileNumber,
                //         entity.DateOfBirth,
                //         entity.DateOfRetirement,
                //         entity.DateOfCommencement
                //     }
                // )
                .AsSplitQuery()
                .ToListAsync();


                ppoListResponseDTO.PpoList = _mapper.Map<List<PensionerListItemDTO>>(ppoList);
            }
            catch (Exception ex) {
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillDataSource(
                    ppoBillResponseDTO,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return ppoBillResponseDTO;
            }

            return _mapper.Map<T>(ppoListResponseDTO);
        }

        public async Task<T> SavePpoBill<T>(
            PensionerFirstBillResponseDTO ppoBillDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBill ppoBillEntity = _mapper.Map<PpoBill>(ppoBillDTO);
            T ppoBillResponseDTO = _mapper.Map<T>(ppoBillEntity);
            try {
                PpoStatusFlag? ppoApprovedFlag = await _pensionDbContext.PpoStatusFlags
                    .Where(
                        entity => entity.ActiveFlag
                        && entity.StatusFlag == PensionStatusFlag.PpoApproved
                        && entity.PpoId == ppoBillDTO.PpoId
                        && entity.TreasuryCode == treasuryCode
                        && entity.FinancialYear == financialYear
                    )
                    .FirstOrDefaultAsync();

                if(ppoApprovedFlag == null)
                {
                    ppoBillResponseDTO = _mapper.Map<T>(new PpoBill());
                    ppoBillResponseDTO.FillDataSource(
                        ppoApprovedFlag,
                        "PPO is not Approved! Please check PPO ID or approve PPO."
                    );
                    return ppoBillResponseDTO;
                }

                PpoBill? ppoBill = await _pensionDbContext.PpoBills
                    .Where(
                        entity => entity.ActiveFlag
                        && entity.PpoId == ppoBillDTO.PpoId
                        && entity.BillType == ppoBillDTO.BillType
                        && entity.FinancialYear == financialYear
                        && entity.TreasuryCode == treasuryCode
                    )
                    .FirstOrDefaultAsync();
                if(ppoBill != null) {
                    ppoBillResponseDTO = _mapper.Map<T>(ppoBill);
                    ppoBillResponseDTO.FillDataSource(
                        ppoBill,
                        "Bill already exists! Please check PPO ID, bill date and bill type."
                    );
                    return ppoBillResponseDTO;
                }
                if(ppoBillDTO.BillType == BillType.RegularBill) {   
                    PpoBill? ppoFirstBill = await _pensionDbContext.PpoBills
                        .Where(
                            entity => entity.ActiveFlag
                            && entity.PpoId == ppoBillDTO.PpoId
                            && entity.BillType == BillType.FirstBill
                            && entity.TreasuryCode == treasuryCode
                        )
                        .FirstOrDefaultAsync();
                    if(ppoFirstBill == null) {
                        ppoBillResponseDTO = _mapper.Map<T>(new PpoBill());
                        ppoBillResponseDTO.FillDataSource(
                            ppoFirstBill,
                            "First Bill not generated! Please check PPO ID or generate first bill."
                        );
                        return ppoBillResponseDTO;
                    }
                }

                SetCreatedBy(ppoBillEntity);

                string hoaId = await _pensionDbContext.Pensioners
                    .Where(
                        entity => entity.ActiveFlag
                        && entity.PpoId == ppoBillDTO.PpoId
                        && entity.TreasuryCode == treasuryCode
                    )
                    .Include(
                        entity => entity.Category
                    )
                    .ThenInclude(
                        entity => entity.PrimaryCategory
                    )
                    .Select(entity => entity.Category.PrimaryCategory.HoaId)
                    .FirstOrDefaultAsync() ?? "";

                Bill billEntity = new()
                    {
                        ActiveFlag = true,
                        CreatedBy = ppoBillEntity.CreatedBy,
                        CreatedAt = DateTime.Now,
                        BillDate = ppoBillDTO.ToDate,
                        TreasuryCode = treasuryCode,
                        FinancialYear = financialYear,
                        FromDate = ppoBillDTO.FromDate,
                        ToDate = ppoBillDTO.ToDate,
                        HoaId = hoaId,
                        BranchId = ppoBillDTO.BranchId,
                        BillNo = await _ppoBillRepository.GetNextBillNo(financialYear, treasuryCode)
                    };
                
                if(ppoBillDTO.BillType == BillType.RegularBill) {

                    billEntity =  await _pensionDbContext.Bills
                    .Where(
                        entity => entity.ActiveFlag
                        && entity.TreasuryCode == treasuryCode
                        && entity.FromDate == PensionCalculator.CalculatePeriodStartDate(ppoBillDTO.BillDate)
                        && entity.ToDate == PensionCalculator.CalculatePeriodEndDate(ppoBillDTO.BillDate)
                        && entity.FinancialYear == financialYear
                        && entity.HoaId == hoaId
                        && entity.BranchId == ppoBillDTO.BranchId
                    )
                    .FirstOrDefaultAsync() ?? new()
                    {
                        ActiveFlag = true,
                        CreatedBy = ppoBillEntity.CreatedBy,
                        CreatedAt = DateTime.Now,
                        BillDate = ppoBillDTO.ToDate,
                        TreasuryCode = treasuryCode,
                        FinancialYear = financialYear,
                        FromDate = PensionCalculator.CalculatePeriodStartDate(ppoBillDTO.BillDate),
                        ToDate = PensionCalculator.CalculatePeriodEndDate(ppoBillDTO.BillDate),
                        BranchId = ppoBillDTO.BranchId,
                        HoaId = hoaId,
                        BillNo = billEntity.BillNo
                    };
                    // $"RegularBill ID: {billEntity.Id}".PrintOut();
                    // $"RegularBill HoA: {billEntity.HoaId}".PrintOut();
                    // $"RegularBill PensionerHoA: {hoaId}".PrintOut();
                }

                ppoBillEntity.Bill = billEntity;

                ppoBillResponseDTO = await _ppoBillRepository.SavePpoBill<T>(
                        ppoBillEntity,
                        financialYear,
                        treasuryCode
                    );
                // ppoBillResponseDTO.PreparedBy = GetUserName();
                // ppoBillResponseDTO.PreparedOn = DateOnly.FromDateTime(DateTime.Now);
            }
            catch (DbUpdateException ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<T>(ppoBillEntity),
                    $"DbUpdateException: {ex.InnerException?.Message} {this.ToString()}"
                );
                return ppoBillResponseDTO;
            }
            catch (Exception ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<T>(ppoBillEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message} {this.ToString()}"
                );
                return ppoBillResponseDTO;
            }
            return ppoBillResponseDTO;
        }

        public async Task<PpoBillResponseDTO> GetFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBill? ppoBillEntity = new ();
            PpoBillResponseDTO ppoBillResponseDTO = _mapper.Map<PpoBillResponseDTO>(ppoBillEntity);
            try {

                ppoBillEntity = await _ppoBillRepository.GetPpoFirstBillByPpoId(
                    ppoId,
                    financialYear,
                    treasuryCode
                );
                if(ppoBillEntity == null) {
                    ppoBillResponseDTO.FillDataSource(
                        ppoBillEntity,
                        "Bill not found! Please add bill first or check PPO id."
                    );
                    return ppoBillResponseDTO;
                }
                ppoBillResponseDTO = _mapper.Map<PpoBillResponseDTO>(ppoBillEntity);
                ppoBillResponseDTO.BankBranchName = await _bankBranchRepository.GetBankBranchNameByPpoId(
                    treasuryCode,
                    ppoId
                );
                ppoBillResponseDTO.PreparedBy = GetUserName();
                ppoBillResponseDTO.PreparedOn = DateOnly.FromDateTime(DateTime.Now);
                return ppoBillResponseDTO;
            }
            catch (DbUpdateException ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<PpoBillResponseDTO>(ppoBillEntity),
                    $"DbUpdateException: {ex.InnerException?.Message} {this.ToString()}"
                );
                return ppoBillResponseDTO;
            }
            catch (Exception ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<PpoBillResponseDTO>(ppoBillEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message} {this.ToString()}"
                );
                return ppoBillResponseDTO;
            }
        }
    }
}