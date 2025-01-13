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
    public class PensionStatusService : BaseService, IPensionStatusService
    {
        private readonly IClaimService _claimService;
        private readonly PensionDbContext _pensionDbContext;
        protected IPensionStatusRepository _pensionStatusRepository;
        protected IMapper _mapper;

        public PensionStatusService(
            PensionDbContext pensionDbContext,
            IPensionStatusRepository pensionStatusRepository,
            IClaimService claimService,
            IMapper mapper
        )
            : base(claimService)
        {
            _pensionStatusRepository = pensionStatusRepository;
            _pensionDbContext = pensionDbContext;
            _claimService = claimService;
            _mapper = mapper;
            _userId = _claimService.GetUserId();
        }

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
                    pensionStatusDTO.FillDataSource(
                        pensionStatusFlag,
                        "Status Flag is not set. Please check PPO ID and Status Flag"
                    );
                    return pensionStatusDTO;
                }
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                pensionStatusDTO.FillDataSource(new PpoStatusFlag(), message);
                return pensionStatusDTO;
            }
            return pensionStatusDTO;
        }

        public async Task<PensionStatusEntryDTO> SetPensionStatusFlag(
            PensionStatusEntryDTO pensionStatusEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoStatusFlag ppoStatusEntity = new();
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
                    pensionStatusEntryDTO.FillDataSource(
                        pensioner,
                        "Pensioner does not exist, please check PPO ID"
                    );
                    return pensionStatusEntryDTO;
                }

                ppoStatusEntity = await _pensionDbContext.PpoStatusFlags.FirstOrDefaultAsync(
                    entity =>
                        entity.ActiveFlag
                        && entity.TreasuryCode == treasuryCode
                        && entity.PpoId == pensionStatusEntryDTO.PpoId
                        && entity.StatusFlag == pensionStatusEntryDTO.StatusFlag
                );

                if (ppoStatusEntity is null)
                {
                    ppoStatusEntity = _mapper.Map<PpoStatusFlag>(pensionStatusEntryDTO);
                    ppoStatusEntity.PensionerId = pensioner.Id;
                    ppoStatusEntity.TreasuryCode = treasuryCode;
                    ppoStatusEntity.FinancialYear = financialYear;
                    SetCreatedBy(ppoStatusEntity);

                    await _pensionDbContext.PpoStatusFlags.AddAsync(ppoStatusEntity);
                    await _pensionDbContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                pensionStatusEntryDTO.FillDataSource(ppoStatusEntity, message);
                return pensionStatusEntryDTO;
            }
            return _mapper.Map<PensionStatusEntryDTO>(ppoStatusEntity);
        }

        public async Task<PensionStatusDTO> ClearPensionStatusFlag(
            int ppoId,
            PensionStatusFlag pensionStatusFlag,
            short financialYear,
            string treasuryCode
        )
        {
            PpoStatusFlag ppoStatusEntity = new();
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
                        pensionStatusDTO.FillDataSource(
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
                    pensionStatusDTO.FillDataSource(
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
                pensionStatusDTO.FillDataSource(ppoStatusEntity, message);
                return pensionStatusDTO;
            }
            return _mapper.Map<PensionStatusDTO>(ppoStatusEntity);
        }
    }
}
