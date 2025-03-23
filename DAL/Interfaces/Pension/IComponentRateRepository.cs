using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IComponentRateRepository
    {
        public Task<List<T>> GetComponentRatesByCategoryId<T>(
            long categoryId,
            Expression<Func<ComponentRate, T>> selectExpression
        );

        public Task<List<T>> GetPensionCategoriesWithRatesAsync<T>();
    }
}
