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
                    responseDTO.FillDataSource(
                        ppoByTransferEntity,
                        "PpoByTransfer already exists!"
                    );
                    return responseDTO;
                }

                if (ppoByTransferEntity.FromDate >= ppoByTransferEntity.ToDate)
                {
                    responseDTO.FillDataSource(
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
                    responseDTO.FillDataSource(
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
                    responseDTO.FillDataSource(
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
                    responseDTO.FillDataSource(ppoByTransferEntity, "ByTransferHead not found.");
                    return responseDTO;
                }

                SetCreatedBy(ppoByTransferEntity);

                responseDTO = await _ppoByTransferRepository.CreatePpoByTransfer<T>(
                    ppoByTransferEntity
                );
            }
            catch (Exception ex)
            {
                responseDTO.FillDataSource(
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
                existingEntity = await _ppoByTransferRepository.GetPPOByTransferByIdAsync(id);

                if (existingEntity is null)
                {
                    response.FillDataSource(existingEntity, "PPO By Transfer not found");
                    return response;
                }

                bool isUsed = await _ppoByTransferRepository.IsUsedInOtherTables(
                    existingEntity.BytransferHeadId
                );

                if (isUsed)
                {
                    response.FillDataSource(
                        existingEntity,
                        "PPO By Transfer is used, Cannot delete"
                    );

                    return response;
                }
                response = await _ppoByTransferRepository.RemovePpoByTransferAsync<T>(id);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    existingEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }

            return response;
        }

        //public async Task<List<TResponse>> GetByTransfersByPpoId<TResponse>(
        //    int ppoId,
        //    short financialYear,
        //    string treasuryCode
        //)
        //{
        //    return await _ppoByTransferRepository.GetAllPpoByTransferByPpoIdAsync(
        //        entity => _mapper.Map<TResponse>(entity),
        //        ppoId,
        //        financialYear,
        //        treasuryCode
        //    );
        //}


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
                        tableResponse.FillDataSource(
                            tableResponse.Data,
                            "PPO Bytransfer not found."
                        );
                        return _mapper.Map<T>(tableResponse);
                    }
                }
                catch (Exception ex)
                {
                    tableResponse.FillDataSource(
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
                existingEntity = await _ppoByTransferRepository.GetPPOByTransferByIdAsync(id);

                if (existingEntity is null)
                {
                    response.FillDataSource(existingEntity, "PPO By Transfer not found.");
                    return response;
                }

                bool isUsed = await _ppoByTransferRepository.IsUsedInOtherTables(
                    existingEntity.BytransferHeadId
                );
                if (isUsed)
                {
                    response.FillDataSource(
                        existingEntity,
                        "Cannot update,This PPO By Transfer is used ."
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
                response.FillDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    existingEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            return response;
        }
    }
}
