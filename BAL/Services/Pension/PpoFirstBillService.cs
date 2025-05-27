using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoFirstBillService(
        IClaimService claimService,
        IMapper mapper,
        IPpoFirstBillRepository ppoFirstBillRepository,
        IBankBranchRepository bankBranchRepository,
        ITreasuryRepository treasuryRepository,
        ILogger<PpoFirstBillService> logger
    ) : PpoBillService(claimService), IPpoFirstBillService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPpoFirstBillRepository _ppoFirstBillRepository = ppoFirstBillRepository;
        private readonly IBankBranchRepository _bankBranchRepository = bankBranchRepository;
        private readonly ITreasuryRepository _treasuryRepository = treasuryRepository;
        private readonly ILogger<PpoFirstBillService> _logger = logger;

        public async Task<T> GetPposForFirstBillGeneration<T>(
            short financialYear,
            string treasuryCode
        )
        {
            PpoListResponseDTO ppoListResponseDTO = new();

            try
            {
                List<Pensioner>? ppoList =
                    await _ppoFirstBillRepository.GetPensionersForFirstBillGeneration(
                        financialYear,
                        treasuryCode
                    );

                ppoListResponseDTO.PpoList = _mapper.Map<List<PensionerListItemDTO>>(ppoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPOs for first bill generation for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                    financialYear,
                    treasuryCode
                );
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillErrorInDataSource(new { }, "ServiceException: ", ex);
                return ppoBillResponseDTO;
            }
            _logger.LogInformation(
                "Successfully fetched PPOs for first bill generation for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );

            return _mapper.Map<T>(ppoListResponseDTO);
        }

        public async Task<T> GetPposForFirstBillPrint<T>(short financialYear, string treasuryCode)
        {
            _logger.LogInformation(
                "Received request to get PPOs for first bill print for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );
            PpoListResponseDTO ppoListResponseDTO = new();

            try
            {
                List<Pensioner>? ppoList =
                    await _ppoFirstBillRepository.GetPensionersForFirstBillPrint(
                        financialYear,
                        treasuryCode
                    );

                ppoListResponseDTO.PpoList = _mapper.Map<List<PensionerListItemDTO>>(ppoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPOs for first bill print for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                    financialYear,
                    treasuryCode
                );
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillErrorInDataSource(
                    ppoBillResponseDTO,
                    $"ServiceException:",
                    ex
                );
                return ppoBillResponseDTO;
            }

            _logger.LogInformation(
                "Successfully fetched PPOs for first bill print for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );
            return _mapper.Map<T>(ppoListResponseDTO);
        }

        public async Task<PpoBillResponseDTO> GetFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Received request to get first bill by PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                ppoId,
                financialYear,
                treasuryCode
            );
            PpoBillResponseDTO ppoBillResponseDTO = new();
            try
            {
                if (
                    !await _ppoFirstBillRepository.IsFirstBillAlreadyGenerated(
                        ppoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    _logger.LogWarning(
                        "First Pension Bill not found for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                        ppoId,
                        financialYear,
                        treasuryCode
                    );
                    ppoBillResponseDTO.FillErrorInDataSource(
                        ppoId,
                        $"First Pension Bill not found! Please generate first pension bill or check PPO id: {ppoId}."
                    );
                }

                var ppoBillEntity = await _ppoFirstBillRepository.GetPpoFirstBillByPpoId<PpoBill>(
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
                _logger.LogError(
                    ex,
                    "Error occurred while fetching first bill by PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                    ppoId,
                    financialYear,
                    treasuryCode
                );
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
            _logger.LogInformation(
                "Received request to save first pension bill with data: {InitiateFirstPensionBillEntryDTO}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                initiateFirstPensionBillDTO,
                financialYear,
                treasuryCode
            );
            PpoBillSaveResponseDTO response = new();
            try
            {
                Pensioner? pensioner = await _ppoFirstBillRepository.GetPensionerByPpoId(
                    initiateFirstPensionBillDTO.PpoId,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    _logger.LogWarning(
                        "Pensioner not found for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
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
                    !await _ppoFirstBillRepository.IsPpoApproved(
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    _logger.LogWarning(
                        "PPO is not approved for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
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

                if (pensioner.Category.ComponentRates.Count == 0)
                {
                    _logger.LogWarning(
                        "Component rates not found for Pensioner with PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        new
                        {
                            initiateFirstPensionBillDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        $"Component rates not found for the Pensioner, Please check PPO Category ID: {pensioner.Category.Id}"
                    );
                    return _mapper.Map<T>(response);
                }

                if (
                    await _ppoFirstBillRepository.IsFirstBillAlreadyGenerated(
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    _logger.LogWarning(
                        "First Pension Bill already exists for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
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

                PpoBill ppoBillEntity = _ppoFirstBillRepository.GenerateFirstPensionBill<PpoBill>(
                    pensioner,
                    initiateFirstPensionBillDTO,
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
                    AccountHeadId = await _ppoFirstBillRepository.GetHoaIdByPpoId(
                        initiateFirstPensionBillDTO.PpoId,
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

                long hoaId = await _ppoFirstBillRepository.GetHoaIdByPpoId(
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
                _logger.LogError(
                    ex,
                    "Error occurred while saving first pension bill with data: {InitiateFirstPensionBillEntryDTO}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                    initiateFirstPensionBillDTO,
                    financialYear,
                    treasuryCode
                );
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
            _logger.LogInformation(
                "Successfully saved first pension bill for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                initiateFirstPensionBillDTO.PpoId,
                financialYear,
                treasuryCode
            );
            return _mapper.Map<T>(response);
        }

        public async Task<T> GenerateFirstPensionBill<T>(
            InitiateFirstPensionBillEntryDTO initiateFirstPensionBillDTO,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Received request to generate first pension bill with data: {InitiateFirstPensionBillEntryDTO}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                initiateFirstPensionBillDTO,
                financialYear,
                treasuryCode
            );
            InitiateFirstPensionBillResponseDTO response = new();
            try
            {
                Pensioner? pensioner = await _ppoFirstBillRepository.GetPensionerByPpoId(
                    initiateFirstPensionBillDTO.PpoId,
                    treasuryCode
                );

                if (pensioner == null)
                {
                    _logger.LogWarning(
                        "Pensioner not found for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
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
                    !await _ppoFirstBillRepository.IsPpoApproved(
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    _logger.LogWarning(
                        "PPO is not approved for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
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
                    await _ppoFirstBillRepository.IsFirstBillAlreadyGenerated(
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    _logger.LogWarning(
                        "First Pension Bill already generated for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                        initiateFirstPensionBillDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        new
                        {
                            initiateFirstPensionBillDTO.PpoId,
                            treasuryCode,
                            financialYear,
                        },
                        $"First Bill already generated! Please check PPO ID: {initiateFirstPensionBillDTO.PpoId}"
                    );
                    return _mapper.Map<T>(response);
                }

                response =
                    _ppoFirstBillRepository.GenerateFirstPensionBill<InitiateFirstPensionBillResponseDTO>(
                        pensioner,
                        initiateFirstPensionBillDTO,
                        financialYear,
                        treasuryCode
                    );

                response.BillDate = initiateFirstPensionBillDTO.ToDate;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while generating first pension bill with data: {InitiateFirstPensionBillEntryDTO}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                    initiateFirstPensionBillDTO,
                    financialYear,
                    treasuryCode
                );
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
            _logger.LogInformation(
                "Successfully generated first pension bill for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                initiateFirstPensionBillDTO.PpoId,
                financialYear,
                treasuryCode
            );
            return _mapper.Map<T>(response);
        }
    }
}
