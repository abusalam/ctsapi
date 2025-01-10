using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IBreakupRepository : IRepository<Breakup>
    {
        public Task<List<T>> GetBreakupsAsync<T>(Expression<Func<Breakup, T>> selectExpression);
    }
}
