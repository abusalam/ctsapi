using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionerBankAccountService : IBaseService
    {
        public Task<PensionerBankAcDTO> CreatePensionerBankAccount(
            int ppoId,
            PensionerBankAcDTO pensionerBankAcDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionerBankAcDTO> UpdatePensionerBankAccount(
            int ppoId,
            PensionerBankAcDTO pensionerBankAcDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionerBankAcDTO> GetPensionerBankAccount(
            int ppoId,
            short financialYear,
            string treasuryCode
        );
        public Task<IEnumerable<PensionerBankAcDTO>> GetAllPensionerBankAccounts(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        );
    }
}