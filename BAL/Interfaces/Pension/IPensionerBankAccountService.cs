using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionerBankAccountService : IBaseService
    {
        public Task<PensionerBankAcResponseDTO> CreatePensionerBankAccount(
            int ppoId,
            long pensionerId,
            PensionerBankAcEntryDTO pensionerBankAcEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionerBankAcResponseDTO> UpdatePensionerBankAccount(
            int ppoId,
            PensionerBankAcEntryDTO pensionerBankAcEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionerBankAcResponseDTO> GetPensionerBankAccount(
            int ppoId,
            short financialYear,
            string treasuryCode
        );
        public Task<IEnumerable<PensionerBankAcResponseDTO>> GetAllPensionerBankAccounts(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        );
    }
}