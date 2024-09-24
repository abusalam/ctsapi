using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IComponentRateRepository : IRepository<ComponentRate>
    {
        public Task<IEnumerable<T>> GetComponentRatesByCategoryId<T>(
            long categoryId,
            Expression<Func<ComponentRate, T>> selectExpression
        );
        
    }
}