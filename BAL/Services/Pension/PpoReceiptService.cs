using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoReceiptService : BaseService, IPpoReceiptService
    {
        private readonly IManualPpoReceiptRepository _manualPpoReceiptRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        private readonly PensionDbContext _pensionDbContext;
        private readonly ILogger<PpoReceiptService> _logger;

        public PpoReceiptService(
            IManualPpoReceiptRepository manualPpoReceiptRepository,
            IClaimService claimService,
            IMapper mapper,
            PensionDbContext pensionDbContext,
            ILogger<PpoReceiptService> logger
        )
            : base(claimService)
        {
            _manualPpoReceiptRepository = manualPpoReceiptRepository;
            _claimService = claimService;
            _mapper = mapper;
            _userId = _claimService.GetUserId();
            _pensionDbContext = pensionDbContext;
            _logger = logger;
        }

        public async Task<ManualPpoReceiptResponseDTO> GetPpoReceipt(string treasuryReceiptNo)
        {
            ManualPpoReceiptResponseDTO manualPpoReceiptResponseDTO;
            try
            {
                manualPpoReceiptResponseDTO = _mapper.Map<ManualPpoReceiptResponseDTO>(
                    await _manualPpoReceiptRepository
                        .GetQueryablePpoReceipts()
                        .FirstOrDefaultAsync(entity =>
                            entity.ActiveFlag && entity.TreasuryReceiptNo == treasuryReceiptNo
                        )
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPO receipt for Treasury Receipt No: {TreasuryReceiptNo}",
                    treasuryReceiptNo
                );
                ManualPpoReceiptResponseDTO errorResponse =
                    _mapper.Map<ManualPpoReceiptResponseDTO>(null);
                errorResponse.FillErrorInDataSource(
                    new PpoReceipt(),
                    ex.InnerException?.Message ?? ex.Message
                );
                return errorResponse;
            }
            _logger.LogInformation(
                "PPO receipt for Treasury Receipt No: {TreasuryReceiptNo} fetched successfully",
                treasuryReceiptNo
            );

            return manualPpoReceiptResponseDTO;
        }

        public async Task<ManualPpoReceiptResponseDTO> GetPpoReceipt(long receiptId)
        {
            _logger.LogInformation("Fetching PPO receipt with ID: {ReceiptId}", receiptId);
            ManualPpoReceiptResponseDTO manualPpoReceiptResponseDTO;
            try
            {
                manualPpoReceiptResponseDTO = _mapper.Map<ManualPpoReceiptResponseDTO>(
                    await _manualPpoReceiptRepository
                        .GetQueryablePpoReceipts()
                        .FirstOrDefaultAsync(entity => entity.ActiveFlag && entity.Id == receiptId)
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPO receipt with ID: {ReceiptId}",
                    receiptId
                );
                ManualPpoReceiptResponseDTO errorResponse =
                    _mapper.Map<ManualPpoReceiptResponseDTO>(null);
                errorResponse.FillErrorInDataSource(
                    new PpoReceipt(),
                    ex.InnerException?.Message ?? ex.Message
                );
                return errorResponse;
            }
            _logger.LogInformation(
                "PPO receipt with ID: {ReceiptId} fetched successfully",
                receiptId
            );
            return manualPpoReceiptResponseDTO;
        }

        public async Task<ManualPpoReceiptResponseDTO> CreatePpoReceipt(
            ManualPpoReceiptEntryDTO manualPpoReceiptDTO,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Creating PPO receipt with Treasury Code: {TreasuryCode} and Financial Year: {FinancialYear}",
                treasuryCode,
                financialYear
            );
            PpoReceipt manualPpoReceiptEntity = _mapper.Map<PpoReceipt>(manualPpoReceiptDTO);
            ManualPpoReceiptResponseDTO manualPpoReceiptDTOResponse =
                _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
            try
            {
                manualPpoReceiptEntity = _mapper.Map<PpoReceipt>(manualPpoReceiptDTO);
                manualPpoReceiptEntity.TreasuryCode = treasuryCode;
                manualPpoReceiptEntity.FinancialYear = financialYear;
                manualPpoReceiptEntity.PpoStatus = $"PPO Received";
                SetCreatedBy(manualPpoReceiptEntity);
                manualPpoReceiptDTOResponse =
                    await _manualPpoReceiptRepository.CreatePpoReceiptWithTreasuryReceiptNo<ManualPpoReceiptResponseDTO>(
                        financialYear,
                        treasuryCode,
                        manualPpoReceiptEntity
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating PPO receipt with Treasury Code: {TreasuryCode} and Financial Year: {FinancialYear}",
                    treasuryCode,
                    financialYear
                );
                manualPpoReceiptDTOResponse.FillErrorInDataSource(
                    manualPpoReceiptEntity,
                    ex.Message
                );
                return manualPpoReceiptDTOResponse;
            }
            _logger.LogInformation(
                "PPO receipt created successfully with Treasury Code: {TreasuryCode} and Financial Year: {FinancialYear}",
                treasuryCode,
                financialYear
            );
            return manualPpoReceiptDTOResponse;
        }

        public async Task<List<ListAllPpoReceiptsResponseDTO>> GetAllPpoReceipts(
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Fetching all PPO receipts for Financial Year: {FinancialYear} and Treasury Code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );
            return await _manualPpoReceiptRepository
                .GetQueryablePpoReceipts()
                .Where(entity =>
                    entity.ActiveFlag
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .Select(entity => _mapper.Map<ListAllPpoReceiptsResponseDTO>(entity))
                .ToListAsync();
        }

        public async Task<List<T>> GetPpoReceipts<T>(short financialYear, string treasuryCode)
        {
            return await _manualPpoReceiptRepository.GetPpoReceiptsAsync(
                financialYear,
                treasuryCode,
                entity => _mapper.Map<T>(entity)
            );
        }

        public async Task<List<T>> GetAllUnusedPpoReceipts<T>(
            short financialYear,
            string treasuryCode
        )
        {
            return await _manualPpoReceiptRepository.GetAllUnusedPpoReceipts(
                financialYear,
                treasuryCode,
                entity => _mapper.Map<T>(entity)
            );
        }

        public async Task<ManualPpoReceiptResponseDTO> UpdatePpoReceipt(
            string treasuryReceiptNo,
            ManualPpoReceiptEntryDTO manualPpoReceiptDTO
        )
        {
            _logger.LogInformation(
                "Updating PPO receipt with Treasury Receipt No: {TreasuryReceiptNo}",
                treasuryReceiptNo
            );
            PpoReceipt? manualPpoReceiptEntity = new();

            ManualPpoReceiptResponseDTO manualPpoReceiptDTOResponse =
                _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
            try
            {
                manualPpoReceiptEntity = await _manualPpoReceiptRepository
                    .GetQueryablePpoReceipts()
                    .FirstOrDefaultAsync(entity => entity.TreasuryReceiptNo == treasuryReceiptNo);

                if (manualPpoReceiptEntity is null)
                {
                    _logger.LogWarning(
                        "PPO receipt with Treasury Receipt No: {TreasuryReceiptNo} does not exist",
                        treasuryReceiptNo
                    );
                    manualPpoReceiptDTOResponse.FillErrorInDataSource(
                        manualPpoReceiptEntity,
                        "Treasury Receipt No does not exist!"
                    );
                    return manualPpoReceiptDTOResponse;
                }
                manualPpoReceiptEntity.FillFrom(manualPpoReceiptDTO);
                SetUpdatedBy(manualPpoReceiptEntity);
                _pensionDbContext.PpoReceipts.Update(manualPpoReceiptEntity);
                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    PensionStatusDTO pensionStatusDTO = _mapper.Map<PensionStatusDTO>(
                        manualPpoReceiptEntity
                    );
                    _logger.LogWarning(
                        "Failed to update PPO receipt with Treasury Receipt No: {TreasuryReceiptNo}. Status Flag is not cleared.",
                        treasuryReceiptNo
                    );
                    manualPpoReceiptDTOResponse.FillErrorInDataSource(
                        manualPpoReceiptEntity,
                        "Status Flag is not cleared."
                    );
                    return manualPpoReceiptDTOResponse;
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating PPO receipt with Treasury Receipt No: {TreasuryReceiptNo}",
                    treasuryReceiptNo
                );
                ManualPpoReceiptResponseDTO errorResponse =
                    _mapper.Map<ManualPpoReceiptResponseDTO>(null);
                errorResponse.FillErrorInDataSource(
                    manualPpoReceiptEntity,
                    ex.InnerException?.Message ?? ex.Message
                );
                return errorResponse;
            }
            _logger.LogInformation(
                "PPO receipt with Treasury Receipt No: {TreasuryReceiptNo} updated successfully",
                treasuryReceiptNo
            );
            return _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
        }

        public async Task<ManualPpoReceiptResponseDTO> UpdatePpoReceipt(
            long receiptId,
            ManualPpoReceiptEntryDTO manualPpoReceiptDTO
        )
        {
            _logger.LogInformation("Updating PPO receipt with ID: {ReceiptId}", receiptId);
            PpoReceipt? manualPpoReceiptEntity = new();

            ManualPpoReceiptResponseDTO manualPpoReceiptDTOResponse =
                _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
            try
            {
                manualPpoReceiptEntity = await _pensionDbContext.PpoReceipts.FirstOrDefaultAsync(
                    entity => entity.ActiveFlag && entity.Id == receiptId
                );

                if (manualPpoReceiptEntity is null)
                {
                    _logger.LogWarning(
                        "PPO receipt with ID: {ReceiptId} does not exist or has been deleted",
                        receiptId
                    );
                    manualPpoReceiptDTOResponse.FillErrorInDataSource(
                        manualPpoReceiptEntity,
                        "Receipt does not exist! or has been deleted"
                    );
                    return manualPpoReceiptDTOResponse;
                }
                manualPpoReceiptEntity.FillFrom(manualPpoReceiptDTO);
                SetUpdatedBy(manualPpoReceiptEntity);
                _pensionDbContext.PpoReceipts.Update(manualPpoReceiptEntity);
                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    _logger.LogWarning(
                        "Failed to update PPO receipt with ID: {ReceiptId}. Update operation did not affect any rows.",
                        receiptId
                    );
                    manualPpoReceiptDTOResponse.FillErrorInDataSource(
                        manualPpoReceiptEntity,
                        "Update Failed!"
                    );
                    return manualPpoReceiptDTOResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating PPO receipt with ID: {ReceiptId}",
                    receiptId
                );
                ManualPpoReceiptResponseDTO errorResponse =
                    _mapper.Map<ManualPpoReceiptResponseDTO>(null);
                errorResponse.FillErrorInDataSource(
                    new PpoReceipt(),
                    ex.InnerException?.Message ?? ex.Message
                );
                return errorResponse;
            }

            _logger.LogInformation(
                "PPO receipt with ID: {ReceiptId} updated successfully",
                receiptId
            );
            return _mapper.Map<ManualPpoReceiptResponseDTO>(manualPpoReceiptEntity);
        }
    }
}
