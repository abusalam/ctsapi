using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class PensionerDetailsService : BaseService, IPensionerDetailsService
    {
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
        private readonly IPpoIdSequenceRepository _ppoIdSequenceRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;

        private readonly PensionDbContext _pensionDbContext;
        
        public PensionerDetailsService(
            IPensionerDetailsRepository pensionerDetailsRepository,
            IPpoIdSequenceRepository ppoIdSequenceRepository,
            IClaimService claimService,
            IMapper mapper) : base(claimService)
        {
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _pensionDbContext           = (PensionDbContext) _pensionerDetailsRepository.GetDbContext();
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

                Category? category = await _pensionDbContext.Categories
                    .Where(
                        entity => entity.ActiveFlag
                        && entity.Id == pensionerEntryDTO.CategoryId
                    )
                    .Include(entity => entity.PrimaryCategory)
                    .FirstOrDefaultAsync();
                if(category == null){
                    PensionerResponseDTO errResponse = _mapper.Map<PensionerResponseDTO>(pensionerEntryDTO);
                    errResponse.FillDataSource(
                        category,
                        "Pension category not found. Please check category Id. and try again."
                    );
                    return errResponse;
                }

                Branch? branch = await _pensionDbContext.Branches
                    .Where(
                        entity => entity.ActiveFlag
                        && entity.Id == pensionerEntryDTO.BranchId
                        && entity.TreasuryCode == treasuryCode
                    )
                    .Include(entity => entity.Bank)
                    .FirstOrDefaultAsync();
                if(branch == null){
                    PensionerResponseDTO errResponse = _mapper.Map<PensionerResponseDTO>(pensionerEntryDTO);
                    errResponse.FillDataSource(
                        branch,
                        "Bank branch not found. Please check branch Id. and try again."
                    );
                    return errResponse;
                }

                PpoReceipt? ppoReceipt = await _pensionDbContext.PpoReceipts
                    .Where(entity => entity.PpoNo == pensionerEntryDTO.PpoNo)
                    .FirstOrDefaultAsync();
                if(ppoReceipt==null){
                    PensionerResponseDTO errResponse = _mapper.Map<PensionerResponseDTO>(pensionerEntryDTO);
                    errResponse.FillDataSource(
                        ppoReceipt,
                        "PPO Receipt not found. Please check PPO No. and try again."
                    );
                    return errResponse;
                }
                pensionerEntity = _mapper.Map<Pensioner>(pensionerEntryDTO);
                if(pensionerEntity.DateOfCommencement != ppoReceipt.DateOfCommencement) {
                    PensionerResponseDTO errResponse = _mapper.Map<PensionerResponseDTO>(pensionerEntryDTO);
                    errResponse.FillDataSource(
                        ppoReceipt,
                        "Date of Commencement does not match with PPO Receipt. Please check PPO No. and try again."
                    );
                    return errResponse;
                }
                pensionerEntity.PpoId = await _ppoIdSequenceRepository.GetNextPpoId(
                    financialYear,
                    treasuryCode
                );
                pensionerEntity.FinancialYear = financialYear;
                pensionerEntity.TreasuryCode = treasuryCode;
                
                if(pensionerEntity.PpoId > 0) {
                    SetCreatedBy(pensionerEntity);
                    ppoReceipt.Pensioners.Add(pensionerEntity);
                    if(await _pensionDbContext.SaveChangesAsync() == 0) {
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
                Pensioner? pensioner = await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                        ppoId,
                        financialYear,
                        treasuryCode,
                        entity => _mapper.Map<Pensioner>(entity)
                    );

                if(pensioner == null) {
                    pensionerResponseDTO.FillDataSource(
                        pensioner,
                        "Pensioner not found. Please check PPO Id. and try again."
                    );
                    return pensionerResponseDTO;
                }
                pensionerResponseDTO = _mapper.Map<T>(pensioner);
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

        public async Task<IEnumerable<PensionerListItemDTO>> GetAllPensioners(
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
                    entity => _mapper.Map<PensionerListItemDTO>(entity),
                    dynamicListQueryParameters
                );
        }
        public async Task<IEnumerable<PensionerListItemDTO>> GetAllNonApprovedPensioners(
            short financialYear, 
            string treasuryCode
        )
        {
            var pensioners = await _pensionerDetailsRepository
                .GetAllNotApprovedPensionerDetailsAsync(
                    financialYear,
                    treasuryCode,
                    entity => _mapper.Map<PensionerListItemDTO>(entity)
                );
            _dataCount = pensioners.Count();
            return pensioners;
        }

        public int Add(int a, int b) {
            return a+b;
        }
    }
}