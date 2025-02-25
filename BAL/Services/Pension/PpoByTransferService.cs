using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DAL.Repositories.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoByTransferService : BaseService, IPpoByTransferService
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;
        private readonly IPpoByTransferRepository _ppoByTransferRepository;
        private readonly ITreasuryRepository _treasuryRepository;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
        private readonly IByTransferRepository _byTransferHeadRepository;

        public PpoByTransferService(
            PensionDbContext pensionDbContext,
            IClaimService claimService,
            IMapper mapper,
            IPpoByTransferRepository ppoByTransferRepository,
            ITreasuryRepository treasuryRepository,
            IPensionerDetailsRepository pensionerDetailsRepository,
            IByTransferRepository byTransferHeadRepository
        )
            : base(claimService)
        {
            _pensionDbContext = pensionDbContext;
            _mapper = mapper;
            _ppoByTransferRepository = ppoByTransferRepository;
            _treasuryRepository = treasuryRepository;
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _byTransferHeadRepository = byTransferHeadRepository;
        }

        public async Task<T> SavePpoByTransferHead<T>(
            PpoByTransferEntryDTO ppoByTransferEntryDTO,
            short financialYear,
            string treasuryCode
        )
            where T : BaseDTO, new()
        {
            PpoBytransfer ppoByTransferEntity = _mapper.Map<PpoBytransfer>(ppoByTransferEntryDTO);
            ppoByTransferEntity.FinancialYear = financialYear;
            ppoByTransferEntity.TreasuryCode = treasuryCode;
            ppoByTransferEntity.PensionerId = ppoByTransferEntity.PpoId;
            T responseDTO = _mapper.Map<T>(ppoByTransferEntity);

            try
            {
                var pensioner = await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                    ppoByTransferEntity.PpoId,
                    financialYear,
                    treasuryCode,
                    entity => entity
                );

                if (pensioner == null)
                {
                    responseDTO.FillDataSource(
                        pensioner,
                        "Pensioner not found. Please check PPO Id and try again."
                    );
                    return responseDTO;
                }

                var bytransferhead = await _byTransferHeadRepository.GetByTransferHeadByIdAsync(
                    ppoByTransferEntryDTO.BytransferHeadId
                );

                if (bytransferhead == null || !bytransferhead.ActiveFlag)
                {
                    responseDTO.FillDataSource(
                        bytransferhead,
                        "ByTransferHead not found or already deleted."
                    );
                    return responseDTO;
                }

                var overlappingRecords = await _pensionDbContext
                    .PpoBytransfers.Where(entity =>
                        entity.ActiveFlag
                        && entity.PpoId == ppoByTransferEntity.PpoId
                        && (
                            (
                                entity.FromDate <= ppoByTransferEntity.FromDate
                                && entity.ToDate >= ppoByTransferEntity.FromDate
                            )
                            || (
                                entity.FromDate <= ppoByTransferEntity.ToDate
                                && entity.ToDate >= ppoByTransferEntity.ToDate
                            )
                            || (
                                entity.FromDate >= ppoByTransferEntity.FromDate
                                && entity.ToDate <= ppoByTransferEntity.ToDate
                            )
                        )
                    )
                    .ToListAsync();

                if (overlappingRecords.Any())
                {
                    responseDTO.FillDataSource(
                        overlappingRecords,
                        "Date range overlaps with existing records for the same PPO Id."
                    );
                    return responseDTO;
                }

                var existingPpoByTransfer = await _pensionDbContext
                    .PpoBytransfers.Where(entity =>
                        entity.ActiveFlag
                        && entity.PensionerId == ppoByTransferEntity.PensionerId
                        && entity.BytransferHeadId == ppoByTransferEntity.BytransferHeadId
                        && entity.FromDate == ppoByTransferEntity.FromDate
                        && entity.ToDate == ppoByTransferEntity.ToDate
                    )
                    .FirstOrDefaultAsync();

                if (existingPpoByTransfer != null)
                {
                    responseDTO.FillDataSource(
                        existingPpoByTransfer,
                        "PpoByTransfer already exists!"
                    );
                    return responseDTO;
                }

                SetCreatedBy(ppoByTransferEntity);
                ppoByTransferEntity.UpdatedAt = DateTime.Now;

                if (ppoByTransferEntity != null)
                {
                    responseDTO = await _ppoByTransferRepository.SavePpoByTransferHead<T>(
                        ppoByTransferEntity
                    );
                }
            }
            catch (Exception ex)
            {
                responseDTO.FillDataSource(
                    _mapper.Map<T>(ppoByTransferEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }

        public async Task<T> DeletePPOByTransfer<T>(long id)
        {
            try
            {
                PpoByTransferHeadResponseDTO responseDTO = new();

                var existingEntity = await _ppoByTransferRepository.GetPPOByTransferByIdAsync(id);

                if (existingEntity == null || !existingEntity.ActiveFlag)
                {
                    responseDTO.Message = "PPO By Transfer not found or already deleted.";
                    return _mapper.Map<T>(responseDTO);
                }

                bool isUsed = await _ppoByTransferRepository.IsUsedInOtherTables(
                    existingEntity.BytransferHeadId
                );
                if (isUsed)
                {
                    responseDTO.Message =
                        "Cannot delete, as this PPO By Transfer is used in other tables.";
                    return _mapper.Map<T>(responseDTO);
                }

                existingEntity.ActiveFlag = false;
                existingEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime();

                return await _ppoByTransferRepository.UpdatePPOByTransfer<T>(existingEntity);
            }
            catch (Exception ex)
            {
                PpoByTransferHeadResponseDTO errorResponse = new()
                {
                    Message = $"ServiceException: {ex.InnerException?.Message ?? ex.Message}",
                };

                return _mapper.Map<T>(errorResponse);
            }
        }

        public async Task<T> GetAllPpoByTransferById<T>(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
            where T : BaseDTO, new()
        {
            T ppoByTransferResponseDTO = new T();

            try
            {
                Expression<Func<PpoBytransfer, PpoByTransferHeadResponseList>> selectExpression =
                    entity => new PpoByTransferHeadResponseList
                    {
                        Id = entity.Id,
                        PpoNo = entity.Pensioner.PpoNo,
                        PensionerName = entity.Pensioner.PensionerName,
                        FromDate = entity.FromDate,
                        ToDate = entity.ToDate,
                        BytransferHeadId = entity.BytransferHeadId,
                        BytransferAmount = entity.BytransferAmount,
                        Remarks = entity.Remarks,
                    };

                var ppobytransferList =
                    await _ppoByTransferRepository.GetAllPpoByTransferByPpoIdAsync(
                        ppoId,
                        financialYear,
                        treasuryCode,
                        selectExpression
                    );

                if (ppobytransferList == null || !ppobytransferList.Any())
                {
                    ppoByTransferResponseDTO.FillDataSource(
                        new List<PpoByTransferHeadResponseList>(),
                        "No PpoByTransfer records found for the given PPO Id."
                    );
                    return ppoByTransferResponseDTO;
                }

                if (ppoByTransferResponseDTO is PpoByTransferHeadResponseDTO responseDTO)
                {
                    responseDTO.Result = ppobytransferList;
                }
                else
                {
                    Console.WriteLine("Invalid DTO type.");
                }
            }
            catch (Exception ex)
            {
                ppoByTransferResponseDTO.FillDataSource(
                    new List<PpoByTransferHeadResponseList>(),
                    ex.Message
                );
            }

            return ppoByTransferResponseDTO;
        }

        public async Task<T> UpdatePPOByTransfer<T>(PpoByTransferUpdateDTO updateDTO)
        {
            try
            {
                PpoByTransferHeadResponseDTO responseDTO = new();

                var existingEntity = await _ppoByTransferRepository.GetPPOByTransferByIdAsync(
                    updateDTO.Id
                );

                if (existingEntity == null || !existingEntity.ActiveFlag)
                {
                    responseDTO.Message = "PPO By Transfer record not found or already deleted.";
                    return _mapper.Map<T>(responseDTO);
                }

                bool isUsed = await _ppoByTransferRepository.IsUsedInOtherTables(
                    existingEntity.BytransferHeadId
                );
                if (isUsed)
                {
                    responseDTO.Message =
                        "Cannot update, as this PPO By Transfer is used in other tables.";
                    return _mapper.Map<T>(responseDTO);
                }

                //existingEntity.PpoId = updateDTO.PpoId;
                //existingEntity.BytransferHeadId = updateDTO.BytransferHeadId;
                //existingEntity.FromDate = updateDTO.FromDate;
                //existingEntity.ToDate = updateDTO.ToDate;-
                existingEntity.BytransferAmount = updateDTO.BytransferAmount;
                existingEntity.Remarks = updateDTO.Remarks;
                existingEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime();

                return await _ppoByTransferRepository.UpdatePPOByTransfer<T>(existingEntity);
            }
            catch (Exception ex)
            {
                PpoByTransferHeadResponseDTO errorResponse = new()
                {
                    Message = $"ServiceException: {ex.InnerException?.Message ?? ex.Message}",
                };

                return _mapper.Map<T>(errorResponse);
            }
        }
    }
}
