using System.Linq;
using System.Linq.Expressions;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace CTS_BE.DAL.Repositories
{
    public abstract class Repository<T, Tcontext> : IRepository<T>
        where T : class
        where Tcontext : DbContext
    {
        protected readonly Tcontext CTSDbContext = null;

        public Repository(Tcontext context)
        {
            this.CTSDbContext = context;
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public bool IsTransactionRunning()
        {
            return this.CTSDbContext.Database.CurrentTransaction != null;
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        private IDbContextTransaction BeginTran()
        {
            return this.CTSDbContext.Database.BeginTransaction();
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public IExecutionStrategy GetExecutionStrategy()
        {
            return this.CTSDbContext.Database.CreateExecutionStrategy();
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> result = this.CTSDbContext.Set<T>();
            if (condition != null)
            {
                result = result.Where(condition);
            }

            return result;
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public async Task<ICollection<T>> GetAllByConditionAsync(
            Expression<Func<T, bool>> condition
        )
        {
            IQueryable<T> result = this.CTSDbContext.Set<T>();
            if (condition != null)
            {
                result = result.Where(condition);
            }

            return await result.ToListAsync();
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public IQueryable<T> GetAll()
        {
            IQueryable<T> result = this.CTSDbContext.Set<T>();
            return result;
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public async Task<ICollection<T>> GetAllAsync()
        {
            IQueryable<T> result = this.CTSDbContext.Set<T>();
            return await result.ToListAsync();
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public async Task<ICollection<TResult>> GetSelectedColumnAsync<TResult>(
            Expression<Func<T, TResult>> selectExpression
        )
        {
            IQueryable<TResult> result = this.CTSDbContext.Set<T>().Select(selectExpression);
            return await result.ToListAsync();
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public async Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TResult>> selectExpression
        )
        {
            IQueryable<TResult> result = this
                .CTSDbContext.Set<T>()
                .Where(filterExpression)
                .Select(selectExpression);

            return await result.ToListAsync();
        }

        // public async Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(
        //     Expression<Func<T, bool>> filterExpression,
        //     Expression<Func<T, TResult>> selectExpression,
        //     int pageIndex = 0,
        //     int pageSize = 10,
        //     List<FilterParameter> dynamicFilters = null,
        //     string orderByField = null,
        //     string orderByOrder = null
        // )
        // {
        //     IQueryable<T> query = this.CTSDbContext.Set<T>().Where(filterExpression);

        //     if (dynamicFilters != null && dynamicFilters.Any())
        //     {
        //         foreach (var filter in dynamicFilters)
        //         {
        //             var dynimicFilterExpression = ExpressionHelper.GetFilterExpression<T>(filter.Field, filter.Value, filter.Operator);
        //             query = query.Where(dynimicFilterExpression);
        //         }
        //     }
        //     // Dynamic order by expression
        //     if (!string.IsNullOrWhiteSpace(orderByField))
        //     {
        //         var parameter = Expression.Parameter(typeof(T), "x");
        //         var property = Expression.Property(parameter, orderByField);
        //         var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

        //         if (orderByOrder == "ASC")
        //         {
        //             query = query.OrderBy(lambda);
        //         }
        //         else
        //         {
        //             query = query.OrderByDescending(lambda);
        //         }
        //     }
        //     var result = await query.Select(selectExpression).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        //     return result;
        // }
        // public async Task<Dictionary<TKey, List<TResult>>> GetSelectedColumnGroupByConditionAsync<TKey, TResult>(
        //     Expression<Func<T, bool>> filterExpression,
        //     Expression<Func<T, TKey>> groupByKeySelector,
        //     Expression<Func<T, TResult>> selectExpression)
        // {
        //     var data = await this.CTSDbContext.Set<T>()
        //     .Where(filterExpression)
        //     .ToListAsync();
        //     var groupedResult = data
        //         .GroupBy(groupByKeySelector.Compile())
        //         .ToDictionary(group => group.Key, group => group.Select(selectExpression.Compile()).ToList());

        //     return groupedResult;

        // }

        [Obsolete("Use EntityFramework methods instead", true)]
        public async Task<TResult?> GetSingleSelectedColumnByConditionAsync<TResult>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TResult>> selectExpression
        )
        {
            TResult? result = await this
                .CTSDbContext.Set<T>()
                .Where(filterExpression)
                .Select(selectExpression)
                .FirstOrDefaultAsync();

            return result;
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public T GetSingle(Expression<Func<T, bool>> condition)
        {
            return this.CTSDbContext.Set<T>().Where(condition).FirstOrDefault();
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public async Task<T> GetSingleAysnc(Expression<Func<T, bool>> condition)
        {
            var retValue = await this.CTSDbContext.Set<T>().Where(condition).SingleOrDefaultAsync();

            return retValue;
        }

        // public int CountWithCondition(Expression<Func<T, bool>> condition, List<FilterParameter> dynamicFilters = null)
        // {
        //     IQueryable<T> query = this.CTSDbContext.Set<T>();

        //     if (dynamicFilters != null && dynamicFilters.Any())
        //     {
        //         foreach (var filter in dynamicFilters)
        //         {
        //             var dynimicFilterExpression = ExpressionHelper.GetFilterExpression<T>(filter.Field, filter.Value, filter.Operator);
        //             query = query.Where(dynimicFilterExpression);
        //         }
        //     }
        //     //var result = query.Select(selectExpression).ToListAsync();
        //     return query.Count(condition);
        // }

        [Obsolete("Use EntityFramework methods instead", true)]
        public int CountWithCondition(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> query = this.CTSDbContext.Set<T>();
            return query.Count(condition);
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public async Task<int> CountWithConditionAsync(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> query = this.CTSDbContext.Set<T>();
            return await query.CountAsync(condition);
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public int Count()
        {
            return this.CTSDbContext.Set<T>().Count();
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public async Task<object> ExecuteQuery(string sqlQuery, object parameters)
        {
            return await this.CTSDbContext.Database.ExecuteSqlRawAsync(sqlQuery, parameters);
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public bool Add(T entity)
        {
            this.CTSDbContext.Set<T>().Add(entity);
            return true;
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public void AddRange(IEnumerable<T> entities)
        {
            this.CTSDbContext.Set<T>().AddRange(entities);
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public bool Update(T entity)
        {
            this.CTSDbContext.Entry(entity).State = EntityState.Modified;
            return true;
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public bool Delete(T entity)
        {
            this.CTSDbContext.Set<T>().Remove(entity);
            return true;
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public void SaveChangesManaged()
        {
            this.CTSDbContext.SaveChanges();
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public async Task<int> SaveChangesManagedAsync()
        {
            return await this.CTSDbContext.SaveChangesAsync();
        }

        [Obsolete("Use EntityFramework methods instead", true)]
        public DbContext GetDbContext()
        {
            return this.CTSDbContext;
        }
    }
}
