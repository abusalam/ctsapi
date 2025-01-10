using System.Linq.Expressions;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CTS_BE.DAL.Interfaces
{
    public interface IRepository<T>
    {
        // [Obsolete ("Use EntityFramework methods instead", true)]
        IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> condition);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        Task<ICollection<T>> GetAllByConditionAsync(Expression<Func<T, bool>> condition);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        IQueryable<T> GetAll();

        // [Obsolete ("Use EntityFramework methods instead", true)]
        Task<ICollection<T>> GetAllAsync();

        // [Obsolete ("Use EntityFramework methods instead", true)]
        Task<ICollection<TResult>> GetSelectedColumnAsync<TResult>(
            Expression<Func<T, TResult>> selectExpression
        );

        // [Obsolete ("Use EntityFramework methods instead", true)]
        Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TResult>> selectExpression
        );

        // Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(
        //     Expression<Func<T, bool>> filterExpression,
        //     Expression<Func<T, TResult>> selectExpression,
        //     int pageIndex = 0,
        //     int pageSize = 10,
        //     List<FilterParameter> dynamicFilters = null,
        //     string orderByField = null,
        //     string orderByOrder = null);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        public Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TResult>> selectExpression,
            DynamicListQueryParameters dynamicListQueryParameters
        );

        // public Task<TResult> GetSingleSelectedColumnByConditionAsync<TResult>(Expression<Func<T, bool>> filterExpression,Expression<Func<T, TResult>> selectExpression);

        // Task<Dictionary<TKey, List<TResult>>> GetSelectedColumnGroupByConditionAsync<TKey, TResult>(Expression<Func<T, bool>> filterExpression,Expression<Func<T, TKey>> groupByKeySelector,Expression<Func<T, TResult>> selectExpression);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        T GetSingle(Expression<Func<T, bool>> condition);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        Task<T> GetSingleAysnc(Expression<Func<T, bool>> condition);

        // int CountWithCondition(Expression<Func<T, bool>> condition, List<FilterParameter> dynamicFilters = null);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        int CountWithCondition(Expression<Func<T, bool>> condition);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        Task<int> CountWithConditionAsync(Expression<Func<T, bool>> condition);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        int Count();

        // [Obsolete ("Use EntityFramework methods instead", true)]
        bool Add(T entity);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        bool Update(T entity);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        bool Delete(T entity);

        // [Obsolete ("Use EntityFramework methods instead", true)]
        void SaveChangesManaged();

        // [Obsolete ("Use EntityFramework methods instead", true)]
        public DbContext GetDbContext();

        // [Obsolete ("Use EntityFramework methods instead", true)]
        Task<int> SaveChangesManagedAsync();

        // [Obsolete ("Use EntityFramework methods instead", true)]
        public IExecutionStrategy GetExecutionStrategy();

        // [Obsolete ("Use EntityFramework methods instead", true)]
        public Task<object> ExecuteQuery(string sqlQuery, object parameters);
    }
}
