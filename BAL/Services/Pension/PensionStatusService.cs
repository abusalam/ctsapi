using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DAL.Repositories.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionStatusService : BaseService, IPensionStatusService
    {
        private IClaimService _claimService;
        protected IPensionStatusRepository _pensionStatusRepository;
        protected IMapper _mapper;
        public PensionStatusService(
                IPensionStatusRepository pensionStatusRepository,
                IClaimService claimService,
                IMapper mapper
            )
        {
            _pensionStatusRepository = pensionStatusRepository;
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
            PensionStatusDTO pensionStatusDTO;
            try
            {
                pensionStatusDTO = _mapper.Map<PensionStatusDTO>(
                        await _pensionStatusRepository.GetSingleAysnc(
                            entity => entity.ActiveFlag == true && entity.FinancialYear == financialYear && entity.TreasuryCode == treasuryCode && entity.PpoId == ppoId && entity.StatusFlag == pensionStatusFlag
                        )
                    );
                if(pensionStatusDTO is null) {
                    pensionStatusDTO = new(){
                        StatusFlag = 0,
                    };
                }
            }
            finally {

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
                ppoStatusEntity = await _pensionStatusRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag == true && entity.FinancialYear == financialYear && entity.TreasuryCode == treasuryCode && entity.PpoId == pensionStatusEntryDTO.PpoId && entity.StatusFlag == pensionStatusEntryDTO.StatusFlag
                    );
                if(ppoStatusEntity is null) { 
                    ppoStatusEntity = _mapper.Map<PpoStatusFlag>(pensionStatusEntryDTO);
                    ppoStatusEntity.TreasuryCode = treasuryCode;
                    ppoStatusEntity.FinancialYear = financialYear;
                    SetCreatedBy(ppoStatusEntity);
                    
                    _pensionStatusRepository.Add(ppoStatusEntity);

                    await _pensionStatusRepository.SaveChangesManagedAsync();
                }
            }
            finally {


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
            finally {

            }
            return _mapper.Map<PensionStatusDTO>(ppoStatusEntity);
        }

    }
}