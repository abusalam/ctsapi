using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<T> CreateCategory<T>(
            short finYear,
            string treasuryCode,
            Category categoryEntity
        );
        public Task<Category?> GetCategoryById(
            long categoryId,
            short financialYear,
            string treasuryCode
        );
    }
}