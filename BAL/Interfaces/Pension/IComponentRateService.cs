using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IComponentRateService
    {
        public Task<TResponse> CreateComponentRates<TEntry, TResponse>(
            TEntry pensionRateEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<List<ComponentRateResponseDTO>> ListComponentRates(
            short financialYear,
            string treasuryCode
        );

        public Task<List<TResponse>> ListComponentRatesByCategoryId<TResponse>(long categoryId);

        public Task<List<TResponse>> GetPensionCategoriesWithRates<TResponse>(
            short financialYear,
            string treasuryCode
        );
    }
}
