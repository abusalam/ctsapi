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

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionerBankAccountService : IPensionerBankAccountService
    {

        private readonly IPensionerBankAccountRepository _pensionerBankAccountRepository;
        private readonly IMapper _mapper;
        public PensionerBankAccountService(
            IPensionerBankAccountRepository pensionerBankAccountRepository,
            IMapper mapper)
        {
            _pensionerBankAccountRepository = pensionerBankAccountRepository;
            _mapper                         = mapper;
        }

        public async Task<PensionerBankAcDTO> CreatePensionerBankAccount(
                int ppoId,
                PensionerBankAcDTO pensionerBankAcDTO,
                short financialYear,
                string treasuryCode
            )
        {
            BankAccount bankAccountEntity = new() {
                PpoId = 0
            };

            try {
                bankAccountEntity.FillFrom(pensionerBankAcDTO);
                bankAccountEntity.PpoId = ppoId;
                bankAccountEntity.FinancialYear = financialYear;
                bankAccountEntity.TreasuryCode = treasuryCode;
                bankAccountEntity.ActiveFlag = true;
                bankAccountEntity.CreatedAt = DateTime.Now;
                
                _pensionerBankAccountRepository.Add(bankAccountEntity);

                if(await _pensionerBankAccountRepository.SaveChangesManagedAsync() == 0) {
                    dynamic dataSource = new ExpandoObject(){};
                    dataSource.Message = $"Bank Account not saved!";
                    dataSource.BankAccount = bankAccountEntity;
                    pensionerBankAcDTO.DataSource = dataSource;
                }
            }
            finally {

            }
            return pensionerBankAcDTO;
        }

        public Task<IEnumerable<PensionerBankAcDTO>> GetAllPensionerBankAccounts(
                short financialYear,
                string treasuryCode,
                DynamicListQueryParameters dynamicListQueryParameters
            )
        {
            throw new NotImplementedException();
        }

        public async Task<PensionerBankAcDTO> GetPensionerBankAccount(
                int ppoId,
                short financialYear,
                string treasuryCode
            )
        {
            return _mapper.Map<PensionerBankAcDTO>(
                await _pensionerBankAccountRepository.GetSingleAysnc(
                    entity => entity.ActiveFlag 
                    && entity.PpoId == ppoId 
                    && entity.TreasuryCode == treasuryCode
                )
            ); 
        }

        public async Task<PensionerBankAcDTO> UpdatePensionerBankAccount(
                int ppoId,
                PensionerBankAcDTO pensionerBankAcDTO,
                short financialYear,
                string treasuryCode    
            )
        {
            BankAccount bankAccountEntity = new() {
                PpoId = 0
            };

            try {
                bankAccountEntity = await _pensionerBankAccountRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.PpoId == ppoId 
                        && entity.TreasuryCode == treasuryCode
                    );
                bankAccountEntity.FillFrom(pensionerBankAcDTO);
                
                if(bankAccountEntity.PpoId >0 ) {
                    bankAccountEntity.UpdatedAt = DateTime.Now;
                    _pensionerBankAccountRepository.Update(bankAccountEntity);
                    if(await _pensionerBankAccountRepository.SaveChangesManagedAsync() == 0) {
                        dynamic dataSource = new ExpandoObject(){};
                        dataSource.Message = $"Bank Account not saved!";
                        dataSource.BankAccount = bankAccountEntity;
                        pensionerBankAcDTO.DataSource = dataSource;
                    }
                }
            }
            finally {

            }
            return pensionerBankAcDTO;
        }
    }
}