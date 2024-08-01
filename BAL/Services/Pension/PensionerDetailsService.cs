using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class PensionerDetailsService : BaseService, IPensionerDetailsService
    {
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
        private readonly IPpoIdSequenceRepository _ppoIdSequenceRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        public PensionerDetailsService(
            IPensionerDetailsRepository pensionerDetailsRepository,
            IPpoIdSequenceRepository ppoIdSequenceRepository,
            IClaimService claimService,
            IMapper mapper)
        {
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _ppoIdSequenceRepository    = ppoIdSequenceRepository;
            _claimService               = claimService;
            _mapper                     = mapper;
            _userId                     = claimService.GetUserId();
        }

        public async Task<PensionerResponseDTO> CreatePensioner(
                PensionerEntryDTO pensionerEntryDTO,
                short financialYear,
                string treasuryCode
            )
        {
            Pensioner pensionerEntity = new() {
                PpoId = 0
            };
            PensionerResponseDTO pensionerResponseDTO = _mapper.Map<PensionerResponseDTO>(pensionerEntity);
            try {
                pensionerEntity = _mapper.Map<Pensioner>(pensionerEntryDTO);
                pensionerEntity.PpoId = await _ppoIdSequenceRepository.GetNextPpoId(
                    financialYear,
                    treasuryCode
                );
                pensionerEntity.FinancialYear = financialYear;
                pensionerEntity.TreasuryCode = treasuryCode;
                
                if(pensionerEntity.PpoId > 0) {
                    SetCreatedBy(pensionerEntity);
                    _pensionerDetailsRepository.Add(pensionerEntity);
                    if(await _pensionerDetailsRepository.SaveChangesManagedAsync() == 0) {
                        pensionerEntity.PpoId = 0;
                    }
                }
            }
            finally {
                if(pensionerEntity.PpoId == 0) {
                    pensionerResponseDTO = _mapper.Map<PensionerResponseDTO>(pensionerEntryDTO);
                } else {
                    pensionerResponseDTO = _mapper.Map<PensionerResponseDTO>(pensionerEntity);
                }
            }
            return pensionerResponseDTO;
        }

        public async Task<T> GetPensioner<T>(int ppoId, short financialYear, string treasuryCode)
        {
            Pensioner pensionerEntity = new() {
                PpoId = 0
            };
            T pensionerResponseDTO = _mapper.Map<T>(pensionerEntity);
            try {
                pensionerResponseDTO = _mapper.Map<T>(
                    await _pensionerDetailsRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.PpoId == ppoId 
                        && entity.TreasuryCode == treasuryCode
                    )
                );
            }
            finally {

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
            Pensioner pensionerEntity  = new() {
                    PpoId = 0
                };
            PensionerResponseDTO pensionerResponseDTO;
            try {

                pensionerEntity = await _pensionerDetailsRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.PpoId == ppoId 
                        && entity.TreasuryCode == treasuryCode
                    );

                pensionerEntity.FillFrom(pensionerEntryDTO);

                if(pensionerEntity.PpoId > 0) {
                    SetUpdatedBy(pensionerEntity);
                    _pensionerDetailsRepository.Update(pensionerEntity);
                    if(await _pensionerDetailsRepository.SaveChangesManagedAsync() == 0) {
                        pensionerEntity.PpoId = 0;
                    }
                } else {
                    pensionerEntity = new() {
                        PpoId = 0
                    };
                }
            }
            finally {
                pensionerResponseDTO = _mapper.Map<PensionerResponseDTO>(pensionerEntity);
            }
            return pensionerResponseDTO; 
        }

        public async Task<IEnumerable<PensionerListDTO>> GetAllPensioners(
                short financialYear, 
                string treasuryCode,
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            _dataCount = _pensionerDetailsRepository.Count();
            return await _pensionerDetailsRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag 
                        && entity.FinancialYear == financialYear 
                        && entity.TreasuryCode == treasuryCode,
                    entity => _mapper.Map<PensionerListDTO>(entity),
                    dynamicListQueryParameters
                );
        }

        public int Add(int a, int b) {
            return a+b;
        }
    }
}