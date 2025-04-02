using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoArrearBillService(
        IClaimService claimService,
        IMapper mapper,
        IPpoFirstBillRepository ppoFirstBillRepository,
        IPpoArrearBillRepository ppoArrearBillRepository
    ) : PpoBillService(claimService), IPpoArrearBillService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPpoFirstBillRepository _ppoFirstBillRepository = ppoFirstBillRepository;
        private readonly IPpoArrearBillRepository _ppoArrearBillRepository =
            ppoArrearBillRepository;

        public async Task<T> GetPposForArrearBillGeneration<T>(
            short financialYear,
            string treasuryCode
        )
        {
            PpoArrearBillListResponseDTO ppoListResponseDTO = new();

            try
            {
                List<Pensioner>? ppoList =
                    await _ppoArrearBillRepository.GetPensionersForArrearBillGeneration(
                        financialYear,
                        treasuryCode
                    );

                ppoListResponseDTO.PpoList = _mapper.Map<List<PpoArrearBillResponseDTO>>(ppoList);

                foreach (var pensioner in ppoList)
                {
                    var ppo = ppoListResponseDTO.PpoList.FirstOrDefault(x => x.Id == pensioner.Id);
                    if (ppo != null && pensioner.Branch != null && pensioner.Branch.Bank != null)
                    {
                        ppo.BankName = pensioner.Branch.Bank.BankName;
                    }
                }

                return _mapper.Map<T>(ppoListResponseDTO);
            }
            catch (Exception ex)
            {
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillErrorInDataSource(new { }, "ServiceException: ", ex);
                return ppoBillResponseDTO;
            }
        }

        public async Task<PpoArrearBillResponseDTO> GetArrearBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            PpoArrearBillResponseDTO ppoBillResponseDTO = new();
            try
            {
                if (
                    !await _ppoArrearBillRepository.IsFirstBillAlreadyGenerated(
                        ppoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    ppoBillResponseDTO.FillErrorInDataSource(
                        ppoId,
                        $"First Pension Bill Not Found. Please Generate First Pension Bill Or Check PPO Id: {ppoId}."
                    );
                    return ppoBillResponseDTO;
                }

                var pensioner = await _ppoArrearBillRepository.GetPpoArrearBillByPpoId(
                    ppoId,
                    financialYear,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    ppoBillResponseDTO.FillErrorInDataSource(
                        ppoId,
                        "Pensioner not found. Please check PPO ID."
                    );
                    return ppoBillResponseDTO;
                }

                bool isRunning = pensioner.PpoStatusFlags.Any(status =>
                    status.ActiveFlag && status.StatusFlag == PensionStatusFlag.PpoRunning
                );

                if (!isRunning)
                {
                    ppoBillResponseDTO.FillErrorInDataSource(
                        ppoId,
                        "Pensioner's status is not running."
                    );
                    return ppoBillResponseDTO;
                }

                ppoBillResponseDTO.Id = pensioner.Id;
                ppoBillResponseDTO.PpoId = pensioner.PpoId;
                ppoBillResponseDTO.PpoNo = pensioner.PpoNo;
                ppoBillResponseDTO.PensionerName = pensioner.PensionerName;
                ppoBillResponseDTO.BankAcNo = pensioner.BankAcNo;

                if (pensioner != null && pensioner.Branch != null && pensioner.Branch.Bank != null)
                {
                    ppoBillResponseDTO.BankName = pensioner.Branch.Bank.BankName;
                }

                return ppoBillResponseDTO;
            }
            catch (Exception ex)
            {
                ppoBillResponseDTO.FillErrorInDataSource(ppoId, $"ServiceException: {ex.Message}");
                return ppoBillResponseDTO;
            }
        }

        public async Task<T> GenerateArrearPensionBill<T>(
            PpoArrearBillEntryDTO ppoArrearBillEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            InitiateFirstPensionBillResponseDTO response = new();
            try
            {
                Pensioner? pensioner = await _ppoFirstBillRepository.GetPensionerByPpoId(
                    ppoArrearBillEntryDTO.PpoId,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoArrearBillEntryDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "Pensioner not found! Please check PPO ID."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    !await _ppoFirstBillRepository.IsPpoApproved(
                        ppoArrearBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoArrearBillEntryDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "PPO is not Approved! Please check PPO ID or approve PPO."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    !await _ppoFirstBillRepository.IsFirstBillAlreadyGenerated(
                        ppoArrearBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoArrearBillEntryDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        $"First Bill not generated! Please check PPO ID: {ppoArrearBillEntryDTO.PpoId}"
                    );
                    return _mapper.Map<T>(response);
                }

                response =
                    _ppoArrearBillRepository.GenerateArrearPensionBill<InitiateFirstPensionBillResponseDTO>(
                        pensioner,
                        ppoArrearBillEntryDTO,
                        financialYear,
                        treasuryCode
                    );

                response.BillDate = ppoArrearBillEntryDTO.PeriodTo;
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
                    $"ServiceException-GenerateArrearPensionBill: ",
                    ex
                );
                return _mapper.Map<T>(response);
            }
            return _mapper.Map<T>(response);
        }

        public async Task<T> SaveArrearPensionBill<T>(
            PpoArrearBillEntryDTO ppoArrearBillEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBillSaveResponseDTO response = new();
            try
            {
                Pensioner? pensioner = await _ppoFirstBillRepository.GetPensionerByPpoId(
                    ppoArrearBillEntryDTO.PpoId,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoArrearBillEntryDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "Pensioner not found! Please check PPO ID."
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    !await _ppoFirstBillRepository.IsPpoApproved(
                        ppoArrearBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoArrearBillEntryDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "PPO is not Approved! Please check PPO ID or approve PPO."
                    );
                    return _mapper.Map<T>(response);
                }

                if (pensioner.Category.ComponentRates.Count == 0)
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoArrearBillEntryDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        $"Component rates not found for the Pensioner, Please check PPO Category ID: {pensioner.Category.Id}"
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    !await _ppoFirstBillRepository.IsFirstBillAlreadyGenerated(
                        ppoArrearBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        new
                        {
                            ppoArrearBillEntryDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        "First Pension Bill does not exist! Please check PPO ID, bill date and bill type."
                    );
                    return _mapper.Map<T>(response);
                }

                PpoBill ppoBillEntity = _ppoArrearBillRepository.GenerateArrearPensionBill<PpoBill>(
                    pensioner,
                    ppoArrearBillEntryDTO,
                    financialYear,
                    treasuryCode
                );

                SetCreatedBy(ppoBillEntity);

                ppoBillEntity.Bill = new()
                {
                    ActiveFlag = true,
                    CreatedBy = ppoBillEntity.CreatedBy,
                    CreatedAt = DateTime.Now,
                    BillDate = ppoArrearBillEntryDTO.PeriodTo,
                    TreasuryCode = treasuryCode,
                    FinancialYear = financialYear,
                    FromDate = pensioner.DateOfCommencement,
                    ToDate = ppoArrearBillEntryDTO.PeriodTo,
                    AccountHeadId = await _ppoFirstBillRepository.GetHoaIdByPpoId(
                        ppoArrearBillEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    ),
                    BranchId = pensioner.BranchId,
                    BillNo = await _ppoFirstBillRepository.GetNextBillNo(
                        financialYear,
                        treasuryCode
                    ),
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
                    await _ppoFirstBillRepository.GetPpoComponentRevisionsByPensionerId(
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
                ppoBillEntity.Remarks = "Arrear Bill Generated";
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

                long hoaId = await _ppoFirstBillRepository.GetHoaIdByPpoId(
                    ppoArrearBillEntryDTO.PpoId,
                    financialYear,
                    treasuryCode
                );

                ppoBillEntity.Bill = new()
                {
                    ActiveFlag = true,
                    CreatedBy = ppoBillEntity.CreatedBy,
                    CreatedAt = DateTime.Now,
                    BillDate = ppoArrearBillEntryDTO.PeriodTo,
                    TreasuryCode = treasuryCode,
                    FinancialYear = financialYear,
                    FromDate = pensioner.DateOfCommencement,
                    ToDate = ppoArrearBillEntryDTO.PeriodTo,
                    AccountHeadId = hoaId,
                    BranchId = pensioner.BranchId,
                    BillNo = await _ppoFirstBillRepository.GetNextBillNo(
                        financialYear,
                        treasuryCode
                    ),
                };

                response = await _ppoFirstBillRepository.SavePpoBill<PpoBillSaveResponseDTO>(
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
                        ppoArrearBillEntryDTO.PpoId,
                        treasuryCode,
                        financialYear,
                    },
                    $"ServiceException-SaveArrearPensionBill: ",
                    ex
                );
                return _mapper.Map<T>(response);
            }
            return _mapper.Map<T>(response);
        }

        public async Task<T> GetPposForArrearBillPrint<T>(short financialYear, string treasuryCode)
        {
            PpoArrearBillListResponseDTO ppoListResponseDTO = new();

            try
            {
                List<Pensioner>? ppoList =
                    await _ppoArrearBillRepository.GetPensionersForArrearBillPrint(
                        financialYear,
                        treasuryCode
                    );

                ppoListResponseDTO.PpoList = _mapper.Map<List<PpoArrearBillResponseDTO>>(ppoList);
                foreach (var pensioner in ppoList)
                {
                    var ppo = ppoListResponseDTO.PpoList.FirstOrDefault(x => x.Id == pensioner.Id);
                    if (ppo != null && pensioner.Branch != null && pensioner.Branch.Bank != null)
                    {
                        ppo.BankName = pensioner.Branch.Bank.BankName;
                    }
                }
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
    }
}
