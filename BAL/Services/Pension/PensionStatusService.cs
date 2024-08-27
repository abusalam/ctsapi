using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PensionStatusService : BaseService, IPensionStatusService
    {
        private readonly IClaimService _claimService;
        private readonly PensionDbContext _pensionDbContext;
        protected IPensionStatusRepository _pensionStatusRepository;
        protected IMapper _mapper;
        public PensionStatusService(
                IPensionStatusRepository pensionStatusRepository,
                IClaimService claimService,
                IMapper mapper
            ) : base(claimService)
        {
            _pensionStatusRepository = pensionStatusRepository;
            _pensionDbContext = (PensionDbContext) pensionStatusRepository.GetDbContext();
            _claimService = claimService;
            _mapper = mapper;
            _userId = _claimService.GetUserId();
        }

        public async Task<PensionStatusDTO> CheckPensionStatusFlag(
                int ppoId,
                int pensionStatusFlag,
                short financialYear,
                string treasuryCode
            )
        {
            PensionStatusDTO pensionStatusDTO = new();
            try
            {
                pensionStatusDTO = _mapper.Map<PensionStatusDTO>(
                        await _pensionStatusRepository.GetSingleAysnc(
                            entity => entity.ActiveFlag
                            && entity.TreasuryCode == treasuryCode
                            && entity.PpoId == ppoId
                            && entity.StatusFlag == pensionStatusFlag
                        )
                    );
                if(pensionStatusDTO is null) {
                    pensionStatusDTO = new(){
                        StatusFlag = 0,
                    };
                }
            }
            catch(DbUpdateException ex) {
                var message = ex.InnerException?.Message ?? ex.Message;
                pensionStatusDTO.FillDataSource(
                    new PpoStatusFlag(),
                    message
                );
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
                Pensioner? pensioner = await _pensionDbContext.Pensioners
                    .Where(
                        entity => entity.ActiveFlag
                        && entity.PpoId == pensionStatusEntryDTO.PpoId
                        && entity.TreasuryCode == treasuryCode
                    ).FirstOrDefaultAsync();
                if(pensioner is null) {
                    pensionStatusEntryDTO.FillDataSource(
                        pensioner,
                        "Pensioner does not exist, please check PPO ID"
                    );
                    return pensionStatusEntryDTO;
                }
                ppoStatusEntity = await _pensionStatusRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag
                        && entity.TreasuryCode == treasuryCode
                        && entity.PpoId == pensionStatusEntryDTO.PpoId
                        && entity.StatusFlag == pensionStatusEntryDTO.StatusFlag
                    );
                    
                if(ppoStatusEntity is null) {

                    ppoStatusEntity = _mapper.Map<PpoStatusFlag>(pensionStatusEntryDTO);
                    ppoStatusEntity.PensionerId = pensioner.Id;
                    ppoStatusEntity.TreasuryCode = treasuryCode;
                    ppoStatusEntity.FinancialYear = financialYear;
                    SetCreatedBy(ppoStatusEntity);
                    
                    _pensionStatusRepository.Add(ppoStatusEntity);

                    await _pensionStatusRepository.SaveChangesManagedAsync();
                }
            }
            catch(DbUpdateException ex) {
                var message = ex.InnerException?.Message ?? ex.Message;
                pensionStatusEntryDTO.FillDataSource(
                    ppoStatusEntity,
                    message
                );
                return pensionStatusEntryDTO;
            }
            return _mapper.Map<PensionStatusEntryDTO>(ppoStatusEntity);
        }

        public async Task<PensionStatusDTO> ClearPensionStatusFlag(
            int ppoId,
            int pensionStatusFlag,
            short financialYear,
            string treasuryCode
            )
        {
            PpoStatusFlag ppoStatusEntity = new();
            try
            {
                ppoStatusEntity = await _pensionStatusRepository.GetSingleAysnc(
                    entity => entity.ActiveFlag == true && entity.FinancialYear == financialYear && entity.TreasuryCode == treasuryCode && entity.PpoId == ppoId && entity.StatusFlag == pensionStatusFlag
                );
                if(ppoStatusEntity is not null) {                    
                    ppoStatusEntity.ActiveFlag = false;
                    SetUpdatedBy(ppoStatusEntity);

                    if(_pensionStatusRepository.Update(ppoStatusEntity)) {
                        await _pensionStatusRepository.SaveChangesManagedAsync();
                    }
                } else {
                    ppoStatusEntity = _mapper.Map<PpoStatusFlag>(new PensionStatusEntryDTO() {
                        StatusFlag = 0
                    });
                }
            }
            catch(DbUpdateException ex) {
                PensionStatusDTO pensionStatusDTO = _mapper.Map<PensionStatusDTO>(ppoStatusEntity);
                var message = ex.InnerException?.Message ?? ex.Message;
                pensionStatusDTO.FillDataSource(
                    ppoStatusEntity,
                    message
                );
                return pensionStatusDTO;
            }
            return _mapper.Map<PensionStatusDTO>(ppoStatusEntity);
        }

    }
}