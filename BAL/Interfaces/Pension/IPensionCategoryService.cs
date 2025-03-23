using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionCategoryService
    {
        public Task<TResponse> CreatePensionPrimaryCategory<TEntry, TResponse>(
            TEntry pensionPrimaryCategoryEntryDTO,
            short financialYear,
            string treasuryCode
        );

        public Task<List<T>> GetPrimaryCategories<T>(short financialYear, string treasuryCode);

        public Task<TResponse> CreatePensionSubCategory<TEntry, TResponse>(
            TEntry pensionSubCategoryEntryDTO,
            short financialYear,
            string treasuryCode
        );

        public Task<List<PensionSubCategoryResponseDTO>> ListSubCategory(
            short financialYear,
            string treasuryCode
        );

        public Task<List<TResponse>> GetSubCategories<TResponse>(
            short financialYear,
            string treasuryCode
        );

        public Task<TResponse> CreatePensionCategory<TEntry, TResponse>(
            TEntry pensionCategoryEntryDTO,
            short financialYear,
            string treasuryCode
        );

        public Task<List<PensionCategoryListDTO>> ListPensionCategory(
            short financialYear,
            string treasuryCode
        );

        public Task<List<TResponse>> GetPensionCategories<TResponse>(
            short financialYear,
            string treasuryCode
        );

        public Task<T> GetPensionCategoryById<T>(
            long categoryId,
            short financialYear,
            string treasuryCode
        );

        public Task<List<AccountHeadListItemResponseDTO>> GetListOfAccountHeads<T>(
            short financialYear,
            string treasuryCode
        );
    }
}
