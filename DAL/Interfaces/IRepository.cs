using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace CTS_BE.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> condition);
        Task<ICollection<T>> GetAllByConditionAsync(Expression<Func<T, bool>> condition);

        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();

        Task<ICollection<TResult>> GetSelectedColumnAsync<TResult>(Expression<Func<T, TResult>> selectExpression);
        Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TResult>> selectExpression);
        Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TResult>> selectExpression,
            int pageIndex = 0,
            int pageSize = 10,
            List<FilterParameter> dynamicFilters = null,
            string orderByField = null,
            string orderByOrder = null);
        public Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TResult>> selectExpression,
            DynamicListQueryParameters dynamicListQueryParameters
        );
        public Task<TResult> GetSingleSelectedColumnByConditionAsync<TResult>(Expression<Func<T, bool>> filterExpression,Expression<Func<T, TResult>> selectExpression);

        Task<Dictionary<TKey, List<TResult>>> GetSelectedColumnGroupByConditionAsync<TKey, TResult>(Expression<Func<T, bool>> filterExpression,Expression<Func<T, TKey>> groupByKeySelector,Expression<Func<T, TResult>> selectExpression);
        T GetSingle(Expression<Func<T, bool>> condition);

        Task<T> GetSingleAysnc(Expression<Func<T, bool>> condition);
        int CountWithCondition(Expression<Func<T, bool>> condition, List<FilterParameter> dynamicFilters = null);
        int Count();
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);

        void SaveChangesManaged();
        Task<int> SaveChangesManagedAsync();
        public IExecutionStrategy GetExecutionStrategy();

        public Task<object> ExecuteQuery(string sqlQuery, object parameters);
    }
}
