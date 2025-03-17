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
        IByTransferHeadRepository byTransferHeadRepository
    ) : BaseService(claimService), IPpoByTransferAmountService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPpoByTransferAmountRepository _ppoByTransferRepository =
            ppoByTransferRepository;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository =
            pensionerDetailsRepository;
        private readonly IByTransferHeadRepository _byTransferHeadRepository =
            byTransferHeadRepository;

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
                    responseDTO.FillErrorInDataSource(
                        ppoByTransferEntity,
                        "Ppo By Transfer already exists."
                    );
                    return responseDTO;
                }

                if (ppoByTransferEntity.FromDate >= ppoByTransferEntity.ToDate)
                {
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
                responseDTO.FillErrorInDataSource(
                    ppoByTransferEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }

        public async Task<T> DeletePpoByTransferById<T>(long id)
        {
            T? response = _mapper.Map<T>(new PpoByTransferAmountResponseDTO());
            PpoBytransfer? existingEntity = new();

            try
            {
                existingEntity = await _ppoByTransferRepository.GetPpoByTransferByIdAsync(id);

                if (existingEntity is null)
                {
                    response.FillErrorInDataSource(existingEntity, "Ppo By Transfer not found");
                    return response;
                }

                bool isUsed = await _ppoByTransferRepository.IsUsedInOtherTables(
                    existingEntity.BytransferHeadId
                );

                if (isUsed)
                {
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
                response.FillErrorInDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    existingEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }

            return response;
        }

        public async Task<T> GetByTransfersByPpoId<T>(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
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
                        tableResponse.FillErrorInDataSource(
                            tableResponse.Data,
                            "Ppo By Transfer not found."
                        );
                        return _mapper.Map<T>(tableResponse);
                    }
                }
                catch (Exception ex)
                {
                    tableResponse.FillErrorInDataSource(
                        tableResponse.Data,
                        $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                    );
                }

                return _mapper.Map<T>(tableResponse);
            }
        }

        public async Task<T> UpdatePpoByTransfer<T>(long id, PpoByTransferAmountUpdateDTO updateDTO)
        {
            PpoBytransfer? existingEntity = new();
            T? response = _mapper.Map<T>(existingEntity);

            try
            {
                existingEntity = await _ppoByTransferRepository.GetPpoByTransferByIdAsync(id);

                if (existingEntity is null)
                {
                    response.FillErrorInDataSource(existingEntity, "Ppo By Transfer not found.");
                    return response;
                }

                bool isUsed = await _ppoByTransferRepository.IsUsedInOtherTables(
                    existingEntity.BytransferHeadId
                );
                if (isUsed)
                {
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
                response.FillErrorInDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    existingEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            return response;
        }
    }
}
