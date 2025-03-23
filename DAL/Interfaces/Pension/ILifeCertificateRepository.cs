using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface ILifeCertificateRepository
    {
        public Task<T> CreateLifeCertificate<T>(
            LifeCertificate lifeCertificate,
            string treasuryCode
        );
        public Task<T?> GetLifeCertificateByPpoIdAsync<T>(
            long ppoId,
            string treasuryCode,
            Expression<Func<LifeCertificate, T>> selectExpression
        );
        public Task<List<Pensioner>> GetPensionersWithLifeCertificatesByBranchId(
            long branchId,
            short financialYear,
            string treasuryCode
        );
        public Task<T> UpdateLifeCertificateByPpoId<T>(
            LifeCertificate lifeCertificate,
            string treasuryCode
        );
    }
}
