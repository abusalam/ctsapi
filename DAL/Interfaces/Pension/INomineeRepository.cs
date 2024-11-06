using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface INomineeRepository : IRepository<Nominee>
    {
        public Task<T> SaveNomineeDetails<T>(
            Nominee nominee,
            string treasuryCode
        );
        public Task<List<T>?> GetNomineeByPpoIdAsync<T>(
            int ppoId,
            string treasuryCode,
            Expression<Func<Nominee, T>> selectExpression
        );
        public Task<T> UpdateNomineeDetails<T>(
            Nominee nominee,
            string treasuryCode
        );
        public Task<T> DeleteNomineeDetails<T>(
            Nominee nominee,
            string treasuryCode
        );
        public Task<T?> GetNomineeDetailsByIdAsync<T>(
            long nomineeId,
            string treasuryCode,
            Expression<Func<Nominee, T>> selectExpression
        );
    }
}