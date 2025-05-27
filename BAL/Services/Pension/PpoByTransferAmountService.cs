using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoByTransferAmountService(
        IClaimService claimService,
        IMapper mapper,
        IPpoByTransferAmountRepository ppoByTransferRepository,
        IPensionerDetailsRepository pensionerDetailsRepository,
        IByTransferHeadRepository byTransferHeadRepository,
        ILogger<PpoByTransferAmountService> logger
    ) : BaseService(claimService), IPpoByTransferAmountService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPpoByTransferAmountRepository _ppoByTransferRepository =
            ppoByTransferRepository;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository =
            pensionerDetailsRepository;
        private readonly IByTransferHeadRepository _byTransferHeadRepository =
            byTransferHeadRepository;
        private readonly ILogger<PpoByTransferAmountService> _logger = logger;

        public async Task<T> CreatePpoByTransfer<T>(
            int ppoId,
            PpoByTransferAmountEntryDTO ppoByTransferEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBytransfer ppoByTransferEntity = _mapper.Map<PpoBytransfer>(ppoByTransferEntryDTO);
            ppoByTransferEntity.PpoId = ppoId;
            ppoByTransferEntity.FinancialYear = financialYear;
            ppoByTransferEntity.TreasuryCode = treasuryCode;
            ppoByTransferEntity.PensionerId = ppoId;

            T responseDTO = _mapper.Map<T>(ppoByTransferEntity);

            try
            {
                var existingPpo = await _ppoByTransferRepository.GetExistingPpoByTransferAsync(
                    ppoByTransferEntity
                );

                if (existingPpo != null)
                {
                    _logger.LogWarning(
                        "Ppo By Transfer already exists for PPO ID: {PpoId} with FromDate: {FromDate} and ToDate: {ToDate}",
                        ppoId,
                        ppoByTransferEntity.FromDate,
                        ppoByTransferEntity.ToDate
                    );
                    responseDTO.FillErrorInDataSource(
                        ppoByTransferEntity,
                        "Ppo By Transfer already exists."
                    );
                    return responseDTO;
                }

                if (ppoByTransferEntity.FromDate >= ppoByTransferEntity.ToDate)
                {
                    _logger.LogWarning(
                        "Invalid date range for PPO ID: {PpoId} with FromDate: {FromDate} and ToDate: {ToDate}",
                        ppoId,
                        ppoByTransferEntity.FromDate,
                        ppoByTransferEntity.ToDate
                    );
                    responseDTO.FillErrorInDataSource(
                        ppoByTransferEntity,
                        "FromDate must be earlier than ToDate."
                    );
                    return responseDTO;
                }

                var overlappingRecords =
                    await _ppoByTransferRepository.ValidatePpoByTransferOverlapAsync(
                        ppoByTransferEntity,
                        ppoId
                    );

                if (overlappingRecords is not null)
                {
                    _logger.LogWarning(
                        "Date range overlaps for PPO ID: {PpoId} with FromDate: {FromDate} and ToDate: {ToDate}",
                        ppoId,
                        ppoByTransferEntity.FromDate,
                        ppoByTransferEntity.ToDate
                    );
                    responseDTO.FillErrorInDataSource(
                        ppoByTransferEntity,
                        "Date range overlaps for same PPO Id."
                    );
                    return responseDTO;
                }

                var pensioner = await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                    ppoId,
                    financialYear,
                    treasuryCode,
                    entity => entity
                );

                if (pensioner == null)
                {
                    _logger.LogWarning("Pensioner not found for PPO ID: {PpoId}", ppoId);
                    responseDTO.FillErrorInDataSource(
                        ppoByTransferEntity,
                        "Pensioner not found. Please check PPO Id"
                    );
                    return responseDTO;
                }

                var byTransferHead = await _byTransferHeadRepository.GetByTransferHeadMapById(
                    ppoByTransferEntryDTO.BytransferHeadId
                );

                if (byTransferHead == null)
                {
                    responseDTO.FillErrorInDataSource(
                        ppoByTransferEntity,
                        "ByTransferHead not found."
                    );
                    return responseDTO;
                }

                SetCreatedBy(ppoByTransferEntity);

                responseDTO = await _ppoByTransferRepository.CreatePpoByTransfer<T>(
                    ppoByTransferEntity
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating Ppo By Transfer for PPO ID: {PpoId} with data: {Data}",
                    ppoId,
                    ppoByTransferEntryDTO
                );
                responseDTO.FillErrorInDataSource(
                    ppoByTransferEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }

        public async Task<T> DeletePpoByTransferById<T>(long id)
        {
            _logger.LogInformation("Received request to delete Ppo By Transfer with ID: {Id}", id);
            T? response = _mapper.Map<T>(new PpoByTransferAmountResponseDTO());
            PpoBytransfer? existingEntity = new();

            try
            {
                existingEntity = await _ppoByTransferRepository.GetPpoByTransferByIdAsync(id);

                if (existingEntity is null)
                {
                    _logger.LogWarning("Ppo By Transfer not found for ID: {Id}", id);
                    response.FillErrorInDataSource(existingEntity, "Ppo By Transfer not found");
                    return response;
                }

                bool isUsed = await _ppoByTransferRepository.IsUsedInOtherTables(
                    existingEntity.BytransferHeadId
                );

                if (isUsed)
                {
                    _logger.LogWarning(
                        "Deletion failed for Ppo By Transfer ID: {Id} as it is in use.",
                        id
                    );
                    response.FillErrorInDataSource(
                        existingEntity,
                        "Deletion failed: Ppo By Transfer is in used."
                    );

                    return response;
                }
                response = await _ppoByTransferRepository.RemovePpoByTransferAsync<T>(id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while deleting Ppo By Transfer with ID: {Id}",
                    id
                );
                response.FillErrorInDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Service exception occurred while deleting Ppo By Transfer with ID: {Id}",
                    id
                );
                response.FillErrorInDataSource(
                    existingEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            _logger.LogInformation("Ppo By Transfer with ID: {Id} deleted successfully.", id);

            return response;
        }

        public async Task<T> GetByTransfersByPpoId<T>(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Received request to get By-Transfers for PPO ID: {PpoId}, Financial Year: {FinancialYear}, Treasury Code: {TreasuryCode}",
                ppoId,
                financialYear,
                treasuryCode
            );
            TableResponseDTO<PpoByTransferAmountResponseListDTO> tableResponse = new();
            {
                try
                {
                    tableResponse.Data =
                        await _ppoByTransferRepository.GetAllPpoByTransferByPpoIdAsync(
                            entity => _mapper.Map<PpoByTransferAmountResponseListDTO>(entity),
                            ppoId,
                            financialYear,
                            treasuryCode
                        );
                    if (tableResponse.Data.Count == 0)
                    {
                        _logger.LogWarning(
                            "No Ppo By Transfer records found for PPO ID: {PpoId}",
                            ppoId
                        );
                        tableResponse.FillErrorInDataSource(
                            tableResponse.Data,
                            "Ppo By Transfer not found."
                        );
                        return _mapper.Map<T>(tableResponse);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error occurred while fetching By-Transfers for PPO ID: {PpoId}",
                        ppoId
                    );
                    tableResponse.FillErrorInDataSource(
                        tableResponse.Data,
                        $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                    );
                }

                _logger.LogInformation(
                    "By-Transfers for PPO ID: {PpoId} retrieved successfully.",
                    ppoId
                );
                return _mapper.Map<T>(tableResponse);
            }
        }

        public async Task<T> UpdatePpoByTransfer<T>(long id, PpoByTransferAmountUpdateDTO updateDTO)
        {
            _logger.LogInformation("Received request to update Ppo By Transfer with ID: {Id}", id);
            PpoBytransfer? existingEntity = new();
            T? response = _mapper.Map<T>(existingEntity);

            try
            {
                existingEntity = await _ppoByTransferRepository.GetPpoByTransferByIdAsync(id);

                if (existingEntity is null)
                {
                    _logger.LogWarning("Ppo By Transfer not found for ID: {Id}", id);
                    response.FillErrorInDataSource(existingEntity, "Ppo By Transfer not found.");
                    return response;
                }

                bool isUsed = await _ppoByTransferRepository.IsUsedInOtherTables(
                    existingEntity.BytransferHeadId
                );
                if (isUsed)
                {
                    _logger.LogWarning(
                        "Updation failed for Ppo By Transfer ID: {Id} as it is in use.",
                        id
                    );
                    response.FillErrorInDataSource(
                        existingEntity,
                        "Updation failed: This Ppo By Transfer is used ."
                    );
                    return response;
                }
                existingEntity.Id = id;
                existingEntity.FillFrom(updateDTO);
                SetUpdatedBy(existingEntity);

                response = await _ppoByTransferRepository.UpdatePpoByTransfer<T>(existingEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while updating Ppo By Transfer with ID: {Id}",
                    id
                );
                response.FillErrorInDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Service exception occurred while updating Ppo By Transfer with ID: {Id}",
                    id
                );
                response.FillErrorInDataSource(
                    existingEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            _logger.LogInformation("Ppo By Transfer with ID: {Id} updated successfully.", id);
            return response;
        }
    }
}
