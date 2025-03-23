using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface ISubCategoryRepository
    {
        public Task<SubCategory?> GetSubCategoryById(long subCategoryId);
        public Task<List<T>> GetSubCategoriesAsync<T>();
    }
}
