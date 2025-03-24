using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoComponentRevisionRepository
    {
        public Task<List<T>> GetAllPpos<T>(
            Expression<Func<Pensioner, T>> selectExpression,
            short financialYear,
            string treasuryCode
        );

        public Task<PpoComponentRevision?> GetPpoComponentRevisionById(
            long revisionId,
            short financialYear,
            string treasuryCode
        );

        public Task<List<T>> GetAllRevisionsByPpoIdAsync<T>(
            int ppoId,
            Expression<Func<PpoComponentRevision, T>> selectExpression,
            short financialYear,
            string treasuryCode
        );

        public Task<List<PpoComponentRevision>> GetRevisionsByPpoIdAndRateId(
            int ppoId,
            long rateId
        );

        public Task<T> DeletePpoComponentRevisionById<T>(
            PpoComponentRevision ppoComponentRevision,
            short financialYear,
            string treasuryCode
        );

        public Task<bool> CheckPpoComponentRevisionExists(
            PpoComponentRevision ppoComponentRevision,
            short financialYear,
            string treasuryCode
        );

        public Task<bool> CheckRateExists(long rateId, short financialYear, string treasuryCode);

        public Task<T> UpdatePpoComponentRevision<T>(
            PpoComponentRevision ppoComponentRevision,
            short financialYear,
            string treasuryCode
        );

        public Task<T> CreatePpoComponentRevision<T>(
            PpoComponentRevision ppoComponentRevision,
            short financialYear,
            string treasuryCode
        );
        public Task<T> CreateSinglePpoComponentRevision<T>(
            PpoComponentRevision ppoComponentRevisionEntity
        );
    }
}
