using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionBreakupService
    {
        public Task<TResponse> CreatePensionBreakup<TEntry, TResponse>(
            TEntry pensionBreakupEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<List<PensionBreakupResponseDTO>> ListBreakup(
            short financialYear,
            string treasuryCode
        );
        public Task<List<TResponse>> GetBreakups<TResponse>(
            short financialYear,
            string treasuryCode
        );
    }
}
