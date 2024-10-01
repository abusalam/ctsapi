using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoBillService : BaseService, IPpoBillService
    {
        private readonly IMapper _mapper;
        private readonly IPpoBillRepository _ppoBillRepository;
        private readonly PensionDbContext _pensionDbContext;
        public PpoBillService(
            IPpoBillRepository ppoBillRepository,
            IMapper mapper,
            IClaimService claimService
        ) : base(claimService)
        {
            _mapper = mapper;
            _ppoBillRepository = ppoBillRepository;
            _pensionDbContext = (PensionDbContext) this._ppoBillRepository.GetDbContext();
        }

        public async Task<BillListResponseDTO> GetRegularPensionBills(
            short year,
            short month,
            short financialYear,
            string treasuryCode,
            long? categoryId = null,
            long? bankCode = null
        )
        {
            BillListResponseDTO billListResponseDTO = new();
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
                    entity => entity.PpoBills.Where(
                        entity => entity.ActiveFlag
                        && entity.BillType == BillType.RegularBill
                        && entity.TreasuryCode == treasuryCode
                        && entity.Pensioner.BankAccounts.Any(
                            entity => entity.ActiveFlag
                            && bankCode == null || entity.BankCode == bankCode
                        )
                        && categoryId == null || entity.Pensioner.Category.Id == categoryId
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
                    entity => entity.Pensioner
                )
                .ThenInclude(
                    entity => entity.BankAccounts
                    .Where(
                        entity => entity.ActiveFlag
                    )
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


                billListResponseDTO.Bills = _mapper.Map<List<BillResponseDTO>>(bills);
                billListResponseDTO.Bills.ForEach(
                    entity => {
                                var pensionBill = entity;
                        entity.PpoBills.ForEach(
                            entity => {
                                var pensioner = entity.Pensioner;
                                entity.PpoBillBreakups.ForEach(
                                    entity => {
                                        entity.ComponentName = entity.Revision?.Rate?.Breakup?.ComponentName ?? "";
                                        entity.ComponentType = entity.Revision?.Rate?.Breakup?.ComponentType ?? '-';
                                        entity.AmountPerMonth = entity.Revision?.AmountPerMonth ?? 0;
                                        entity.BaseAmount = PensionCalculator.CalculateBaseAmount(
                                                entity.Revision?.Rate?.Breakup?.ComponentName ?? "",
                                                entity.Revision?.Rate?.RateType ?? '-',
                                                pensioner.BasicPensionAmount,
                                                pensioner.CommutedPensionAmount ?? 0
                                            );
                                    }
                                );
                                entity.BillDate = pensionBill.BillDate;
                                entity.BillNo = pensionBill.BillNo;
                                entity.ToDate = pensionBill.ToDate;
                                entity.PreparedBy = GetUserName();
                                entity.PreparedOn = DateOnly.FromDateTime(DateTime.Today);
                            }
                        );
                        entity.GrossAmount = entity.PpoBills.Sum(bill => bill.GrossAmount);
                        entity.NetAmount = entity.PpoBills.Sum(bill => bill.NetAmount);
                        entity.ByTransferAmount = entity.PpoBills.Sum(bill => bill.ByTransferAmount);
                        entity.PreparedOn = DateOnly.FromDateTime(DateTime.Today);
                        entity.PreparedBy = GetUserName();
                    }
                );
                billListResponseDTO.PreparedBy = GetUserName();
                billListResponseDTO.PreparedOn = DateOnly.FromDateTime(DateTime.Today);
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