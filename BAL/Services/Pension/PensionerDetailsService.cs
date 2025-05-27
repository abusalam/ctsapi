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
    public class PensionerDetailsService : BaseService, IPensionerDetailsService
    {
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
        private readonly PensionDbContext _pensionDbContext;
        private readonly IPpoIdSequenceRepository _ppoIdSequenceRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        private readonly ILogger<PensionerDetailsService> _logger;

        public PensionerDetailsService(
            PensionDbContext pensionDbContext,
            IPensionerDetailsRepository pensionerDetailsRepository,
            IPpoIdSequenceRepository ppoIdSequenceRepository,
            IClaimService claimService,
            IMapper mapper,
            ILogger<PensionerDetailsService> logger
        )
            : base(claimService)
        {
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _pensionDbContext = pensionDbContext;
            _ppoIdSequenceRepository = ppoIdSequenceRepository;
            _claimService = claimService;
            _mapper = mapper;
            _userId = claimService.GetUserId();
            _logger = logger;
        }

        public async Task<PensionerResponseDTO> CreatePensioner(
            PensionerEntryDTO pensionerEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            Pensioner pensionerEntity = _mapper.Map<Pensioner>(pensionerEntryDTO);

            PensionerResponseDTO response = _mapper.Map<PensionerResponseDTO>(pensionerEntity);

            try
            {
                Category? category = await _pensionDbContext
                    .Categories.Where(entity =>
                        entity.ActiveFlag && entity.Id == pensionerEntryDTO.CategoryId
                    )
                    .Include(entity => entity.PrimaryCategory)
                    .FirstOrDefaultAsync();
                if (category == null)
                {
                    _logger.LogError(
                        "Category not found for Id: {CategoryId}",
                        pensionerEntryDTO.CategoryId
                    );
                    response.FillErrorInDataSource(
                        category,
                        "Pension category not found. Please check category Id. and try again."
                    );
                    return response;
                }

                Branch? branch = await _pensionDbContext
                    .Branches.Where(entity =>
                        entity.ActiveFlag && entity.Id == pensionerEntryDTO.BranchId
                    )
                    .Include(entity => entity.Bank)
                    .FirstOrDefaultAsync();
                if (branch == null)
                {
                    _logger.LogError(
                        "Bank branch not found for Id: {BranchId}",
                        pensionerEntryDTO.BranchId
                    );
                    response.FillErrorInDataSource(
                        branch,
                        "Bank branch not found. Please check branch Id. and try again."
                    );
                    return response;
                }

                PpoReceipt? ppoReceipt = await _pensionDbContext
                    .PpoReceipts.Where(entity => entity.PpoNo == pensionerEntryDTO.PpoNo)
                    .FirstOrDefaultAsync();
                if (ppoReceipt == null)
                {
                    _logger.LogError(
                        "PPO Receipt not found for PPO No: {PpoNo}",
                        pensionerEntryDTO.PpoNo
                    );
                    response.FillErrorInDataSource(
                        ppoReceipt,
                        "PPO Receipt not found. Please check PPO No. and try again."
                    );
                    return response;
                }

                if (pensionerEntity.DateOfCommencement != ppoReceipt.DateOfCommencement)
                {
                    _logger.LogError(
                        "Date of Commencement mismatch for PPO No: {PpoNo}. Expected: {ExpectedDate}, Actual: {ActualDate}",
                        pensionerEntryDTO.PpoNo,
                        ppoReceipt.DateOfCommencement,
                        pensionerEntity.DateOfCommencement
                    );
                    response.FillErrorInDataSource(
                        ppoReceipt,
                        "Date of Commencement does not match with PPO Receipt. Please check PPO No. and try again."
                    );
                    return response;
                }
                pensionerEntity.PpoId = await _ppoIdSequenceRepository.GetNextPpoId(
                    financialYear,
                    treasuryCode
                );
                pensionerEntity.FinancialYear = financialYear;
                pensionerEntity.TreasuryCode = treasuryCode;

                if (pensionerEntity.PpoId > 0)
                {
                    SetCreatedBy(pensionerEntity);
                    ppoReceipt.Pensioners.Add(pensionerEntity);
                    if (await _pensionDbContext.SaveChangesAsync() == 0)
                    {
                        pensionerEntity.PpoId = 0;
                    }
                }
                response = _mapper.Map<PensionerResponseDTO>(pensionerEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating pensioner with PPO No: {PpoNo}",
                    pensionerEntryDTO.PpoNo
                );
                response.FillErrorInDataSource(
                    pensionerEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            if (response.PpoId <= 0)
            {
                _logger.LogError(
                    "Failed to create pensioner with PPO No: {PpoNo}. PpoId: {PpoId}",
                    pensionerEntryDTO.PpoNo,
                    response.PpoId
                );
                response.FillErrorInDataSource(
                    pensionerEntity,
                    "Failed to create pensioner. Please try again."
                );
                return response;
            }
            return response;
        }

        public async Task<T> GetPensioner<T>(int ppoId, short financialYear, string treasuryCode)
        {
            Pensioner pensionerEntity = new();
            T pensionerResponseDTO = _mapper.Map<T>(pensionerEntity);
            try
            {
                Pensioner? pensioner =
                    await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                        ppoId,
                        financialYear,
                        treasuryCode,
                        entity => entity
                    );

                if (pensioner == null)
                {
                    pensionerResponseDTO.FillErrorInDataSource(
                        pensioner,
                        "Pensioner not found. Please check PPO Id and try again."
                    );
                    return pensionerResponseDTO;
                }

                pensionerResponseDTO = _mapper.Map<T>(pensioner);

                if (pensionerResponseDTO is PensionerResponseDTO response)
                {
                    PpoStatusFlag? latestStatus = pensioner
                        .PpoStatusFlags.Where(f => f.ActiveFlag)
                        .OrderByDescending(f => f.CreatedAt)
                        .FirstOrDefault();

                    if (latestStatus != null)
                    {
                        response.PensionerStatus = latestStatus.StatusFlag.ToString();
                    }
                    else
                    {
                        response.PensionerStatus = "PPO Created";
                    }

                    response.FirstPensionGenerated = pensioner.PpoStatusFlags.Any(f =>
                        f.ActiveFlag && f.StatusFlag == PensionStatusFlag.FirstPensionBillApproved
                    );
                }
            }
            catch (Exception ex)
            {
                pensionerResponseDTO.FillErrorInDataSource(
                    pensionerEntity,
                    ex.InnerException?.Message ?? ex.Message
                );
            }

            return pensionerResponseDTO;
        }

        public async Task<PensionerResponseDTO> UpdatePensioner(
            int ppoId,
            PensionerEntryDTO pensionerEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            Pensioner? pensionerEntity = new() { PpoId = 0 };
            PensionerResponseDTO response = new();
            try
            {
                pensionerEntity = await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                    ppoId,
                    financialYear,
                    treasuryCode,
                    selectExpression: entity => entity
                );

                if (pensionerEntity is null)
                {
                    response.FillErrorInDataSource(
                        pensionerEntity,
                        "Pensioner not found. Please check PPO Id. and try again."
                    );
                    return response;
                }

                pensionerEntity.FillFrom(pensionerEntryDTO);

                SetUpdatedBy(pensionerEntity);
                response =
                    await _pensionerDetailsRepository.UpdatePensionerDetails<PensionerResponseDTO>(
                        pensionerEntity,
                        treasuryCode
                    );

                PpoStatusFlag? latestStatus = pensionerEntity
                    ?.PpoStatusFlags.Where(f => f.ActiveFlag)
                    .OrderByDescending(f => f.CreatedAt)
                    .FirstOrDefault();

                if (latestStatus != null)
                {
                    response.PensionerStatus = latestStatus.StatusFlag.ToString();
                }
                else
                {
                    response.PensionerStatus = "PPO Created";
                }

                response.FirstPensionGenerated = pensionerEntity?.PpoStatusFlags.Any(f =>
                    f.ActiveFlag && f.StatusFlag == PensionStatusFlag.FirstPensionBillApproved
                );

                return response;
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    pensionerEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<List<PensionerListItemDTO>> GetAllPensioners(
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .Select(entity => _mapper.Map<PensionerListItemDTO>(entity))
                .ToListAsync();
        }

        public async Task<List<T>> GetPensioners<T>(short financialYear, string treasuryCode)
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.FinancialYear == financialYear && entity.TreasuryCode == treasuryCode
                )
                .Select(entity => _mapper.Map<T>(entity))
                .ToListAsync();
        }

        public async Task<List<PensionerListItemDTO>> GetAllNonApprovedPensioners(
            short financialYear,
            string treasuryCode
        )
        {
            var pensioners = (
                await _pensionerDetailsRepository.GetAllNotApprovedPensionerDetailsAsync(
                    financialYear,
                    treasuryCode,
                    entity => _mapper.Map<PensionerListItemDTO>(entity)
                )
            ).ToList();
            return pensioners;
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public async Task<List<PpoPaymentHistoryResponseDTO>> GetPensionerPaymentHistoryByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionerDetailsRepository.GetPensionerPaymentHistoryByPpoIdAsync(
                ppoId,
                financialYear,
                treasuryCode
            );
        }
    }
}
