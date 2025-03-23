using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionStatusService(
        PensionDbContext pensionDbContext,
        IClaimService claimService,
        IMapper mapper
    ) : BaseService(claimService), IPensionStatusService
    {
        private readonly PensionDbContext _pensionDbContext = pensionDbContext;
        protected IMapper _mapper = mapper;

        public async Task<PensionStatusDTO> CheckPensionStatusFlag(
            int ppoId,
            PensionStatusFlag pensionStatusFlag,
            short financialYear,
            string treasuryCode
        )
        {
            PensionStatusDTO pensionStatusDTO = new();
            try
            {
                var ppoStatusEntity = await _pensionDbContext.PpoStatusFlags.FirstOrDefaultAsync(
                    entity =>
                        entity.ActiveFlag
                        && entity.TreasuryCode == treasuryCode
                        && entity.PpoId == ppoId
                        && entity.StatusFlag == pensionStatusFlag
                );

                pensionStatusDTO = _mapper.Map<PensionStatusDTO>(ppoStatusEntity);
                if (pensionStatusDTO is null)
                {
                    pensionStatusDTO = new() { StatusFlag = pensionStatusFlag };
                    pensionStatusDTO.FillErrorInDataSource(
                        pensionStatusFlag,
                        "Status Flag is not set. Please check PPO ID and Status Flag"
                    );
                    return pensionStatusDTO;
                }
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                pensionStatusDTO.FillErrorInDataSource(new PpoStatusFlag(), message);
                return pensionStatusDTO;
            }
            return pensionStatusDTO;
        }

        public async Task<T> SetPensionStatusFlag<T>(
            PensionStatusEntryDTO pensionStatusEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoStatusFlag? ppoStatusEntity = _mapper.Map<PpoStatusFlag>(pensionStatusEntryDTO);
            T result = _mapper.Map<T>(ppoStatusEntity);
            try
            {
                Pensioner? pensioner = await _pensionDbContext.Pensioners.FirstOrDefaultAsync(
                    entity =>
                        entity.ActiveFlag
                        && entity.PpoId == pensionStatusEntryDTO.PpoId
                        && entity.TreasuryCode == treasuryCode
                );

                if (pensioner is null)
                {
                    result.FillErrorInDataSource(
                        ppoStatusEntity,
                        "Pensioner does not exist, please check PPO ID"
                    );
                    return result;
                }

                PpoStatusFlag? ppoStatusEntityExists =
                    await _pensionDbContext.PpoStatusFlags.FirstOrDefaultAsync(entity =>
                        entity.ActiveFlag
                        && entity.TreasuryCode == treasuryCode
                        && entity.PpoId == pensionStatusEntryDTO.PpoId
                        && entity.StatusFlag == pensionStatusEntryDTO.StatusFlag
                    );

                if (ppoStatusEntityExists is not null)
                {
                    result.FillErrorInDataSource(ppoStatusEntity, "Status Flag is already set");
                    return result;
                }

                ppoStatusEntity.PensionerId = pensioner.Id;
                ppoStatusEntity.TreasuryCode = treasuryCode;
                ppoStatusEntity.FinancialYear = financialYear;
                ppoStatusEntity.StatusWef = DateOnly.FromDateTime(DateTime.Now);
                SetCreatedBy(ppoStatusEntity);

                await _pensionDbContext.PpoStatusFlags.AddAsync(ppoStatusEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    result.FillErrorInDataSource(ppoStatusEntity, "Unable to set Status Flag");
                    return result;
                }
            }
            catch (Exception ex)
            {
                pensionStatusEntryDTO.FillErrorInDataSource(
                    ppoStatusEntity,
                    "ServiceException: " + ex.InnerException?.Message ?? ex.Message
                );
            }
            return _mapper.Map<T>(ppoStatusEntity);
        }

        public async Task<PensionStatusDTO> ClearPensionStatusFlag(
            int ppoId,
            PensionStatusFlag pensionStatusFlag,
            short financialYear,
            string treasuryCode
        )
        {
            PpoStatusFlag? ppoStatusEntity = new();
            try
            {
                ppoStatusEntity = await _pensionDbContext.PpoStatusFlags.FirstOrDefaultAsync(
                    entity =>
                        entity.ActiveFlag
                        && entity.FinancialYear == financialYear
                        && entity.TreasuryCode == treasuryCode
                        && entity.PpoId == ppoId
                        && entity.StatusFlag == pensionStatusFlag
                );

                if (ppoStatusEntity is not null)
                {
                    ppoStatusEntity.ActiveFlag = false;
                    SetUpdatedBy(ppoStatusEntity);
                    _pensionDbContext.PpoStatusFlags.Update(ppoStatusEntity);
                    if (await _pensionDbContext.SaveChangesAsync() == 0)
                    {
                        PensionStatusDTO pensionStatusDTO = _mapper.Map<PensionStatusDTO>(
                            ppoStatusEntity
                        );
                        pensionStatusDTO.FillErrorInDataSource(
                            ppoStatusEntity,
                            "Status Flag is not cleared."
                        );
                        return pensionStatusDTO;
                    }
                }
                else
                {
                    ppoStatusEntity = _mapper.Map<PpoStatusFlag>(
                        new PensionStatusEntryDTO() { StatusFlag = pensionStatusFlag }
                    );
                    ppoStatusEntity.PpoId = ppoId;
                    PensionStatusDTO pensionStatusDTO = _mapper.Map<PensionStatusDTO>(
                        ppoStatusEntity
                    );
                    pensionStatusDTO.FillErrorInDataSource(
                        ppoStatusEntity,
                        "Status Flag is not found. Please check PPO ID and Status Flag"
                    );
                    return pensionStatusDTO;
                }
            }
            catch (DbUpdateException ex)
            {
                PensionStatusDTO pensionStatusDTO = _mapper.Map<PensionStatusDTO>(ppoStatusEntity);
                var message = ex.InnerException?.Message ?? ex.Message;
                pensionStatusDTO.FillErrorInDataSource(ppoStatusEntity, message);
                return pensionStatusDTO;
            }
            return _mapper.Map<PensionStatusDTO>(ppoStatusEntity);
        }
    }
}
