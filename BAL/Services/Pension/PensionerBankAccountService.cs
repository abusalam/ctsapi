using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionerBankAccountService : BaseService, IPensionerBankAccountService
    {

        private readonly IPensionerBankAccountRepository _pensionerBankAccountRepository;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        public PensionerBankAccountService(
                IPensionerBankAccountRepository pensionerBankAccountRepository,
                IPensionerDetailsRepository pensionerDetailsRepository,
                IClaimService claimService,
                IMapper mapper
            ) : base(claimService)
        {
            _pensionerBankAccountRepository = pensionerBankAccountRepository;
            _pensionerDetailsRepository     = pensionerDetailsRepository;
            _claimService                   = claimService;
            _mapper                         = mapper;
            _userId                         = _claimService.GetUserId();
        }

        public async Task<PensionerBankAcResponseDTO> CreatePensionerBankAccount(
                int ppoId,
                long pensionerId,
                PensionerBankAcEntryDTO pensionerBankAcEntryDTO,
                short financialYear,
                string treasuryCode
            )
        {
            BankAccount bankAccountEntity = new() {
                PpoId = 0
            };
            PensionerBankAcResponseDTO pensionerBankAcResponseDTO = new();

            try {
                bankAccountEntity.FillFrom(pensionerBankAcEntryDTO);
                bankAccountEntity.PpoId = ppoId;
                bankAccountEntity.PensionerId = pensionerId;
                bankAccountEntity.FinancialYear = financialYear;
                bankAccountEntity.TreasuryCode = treasuryCode;
                SetCreatedBy(bankAccountEntity);
                
                var bankAccountExists = await _pensionerBankAccountRepository.GetSingleAysnc(
                    entity => entity.ActiveFlag 
                    && entity.PpoId == ppoId 
                    && entity.TreasuryCode == treasuryCode
                );

                if(bankAccountExists != null) {
                    pensionerBankAcResponseDTO.FillDataSource(bankAccountExists, "Bank Account already exists!");
                    return pensionerBankAcResponseDTO;
                } else {
                    _pensionerBankAccountRepository.Add(bankAccountEntity);
                }

                if(await _pensionerBankAccountRepository.SaveChangesManagedAsync() == 0) {
                    pensionerBankAcResponseDTO.FillDataSource(bankAccountEntity, "Bank Account not saved!");
                }
            }
            finally {

            }
            return _mapper.Map<PensionerBankAcResponseDTO>(bankAccountEntity);
        }

        public Task<IEnumerable<PensionerBankAcResponseDTO>> GetAllPensionerBankAccounts(
                short financialYear,
                string treasuryCode,
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            throw new NotImplementedException();
        }

        public async Task<PensionerBankAcResponseDTO> GetPensionerBankAccount(
                int ppoId,
                short financialYear,
                string treasuryCode
            )
        {
            return _mapper.Map<PensionerBankAcResponseDTO>(
                await _pensionerBankAccountRepository.GetSingleAysnc(
                    entity => entity.ActiveFlag 
                    && entity.PpoId == ppoId 
                    && entity.TreasuryCode == treasuryCode
                )
            ); 
        }

        public async Task<PensionerBankAcResponseDTO> UpdatePensionerBankAccount(
                int ppoId,
                PensionerBankAcEntryDTO pensionerBankAcEntryDTO,
                short financialYear,
                string treasuryCode    
            )
        {
            BankAccount bankAccountEntity = new() {
                PpoId = 0
            };
            PensionerBankAcResponseDTO pensionerBankAcResponseDTO = new();
            try {
                bankAccountEntity = await _pensionerBankAccountRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.PpoId == ppoId 
                        && entity.TreasuryCode == treasuryCode
                    );

                if(bankAccountEntity == null) {
                    pensionerBankAcResponseDTO.FillDataSource(bankAccountEntity, "Bank Account not found!");
                    return pensionerBankAcResponseDTO;
                }
                bankAccountEntity.FillFrom(pensionerBankAcEntryDTO);
                
                if(bankAccountEntity.PpoId > 0 ) {
                    SetUpdatedBy(bankAccountEntity);
                    _pensionerBankAccountRepository.Update(bankAccountEntity);
                    if(await _pensionerBankAccountRepository.SaveChangesManagedAsync() == 0) {
                        pensionerBankAcResponseDTO.FillDataSource(bankAccountEntity, "Bank Account not saved!");
                        return pensionerBankAcResponseDTO;
                    }
                }
            }
            finally {

            }
            return _mapper.Map<PensionerBankAcResponseDTO>(bankAccountEntity);
        }
    }
}