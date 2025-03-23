using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface ICategoryRepository
    {
        public Task<bool> CategoryExists(Category categoryEntity);
        public Task<List<T>> GetPensionCategoriesAsync<T>();
        public Task<T> CreateCategory<T>(Category categoryEntity);
        public Task<Category?> GetCategoryById(
            long categoryId,
            short financialYear,
            string treasuryCode
        );
    }
}
