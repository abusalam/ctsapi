using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;
using CTS_BE.Helper;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionerDetailsService
    {
        public Task<PensionerResponseDTO> CreatePensioner(
            PensionerEntryDTO pensionerEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionerResponseDTO> UpdatePensioner(
            int ppoId,
            PensionerEntryDTO pensionerEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionerResponseDTO> GetPensioner(
            int ppoId,
            short financialYear,
            string treasuryCode
        );
        public Task<IEnumerable<PensionerListDTO>> GetAllPensioners(
            short financialYear, 
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        );
    }
}