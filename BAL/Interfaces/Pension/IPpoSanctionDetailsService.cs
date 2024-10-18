using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoSanctionDetailsService : IBaseService
    {
        public Task<T> GetSanctionDetailsById<T>(
            long sanctionDetailsId,
            string treasuryCode
        );
        public Task<T> CreateSanctionDetails<T>(
            PpoSanctionDetailsEntryDTO ppoSanctionDetailsEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<T> UpdateSanctionDetailsById<T>(
            long sanctionDetailsId,
            PpoSanctionDetailsEntryDTO ppoSanctionDetailsEntryDTO,
            short financialYear,
            string treasuryCode
        );
    }
}