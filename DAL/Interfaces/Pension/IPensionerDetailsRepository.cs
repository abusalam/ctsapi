using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPensionerDetailsRepository : IRepository<Pensioner>
    {
        public Task<List<PensionerResponseDTO>> GetAllPensionerDetailsAsync(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, PensionerResponseDTO>> selectExpression
        );

        public Task<List<T>> GetPensionerListAsync<T>(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, T>> selectExpression
        );

        public Task<T> UpdatePensionerDetails<T>(Pensioner pensionerEntity, string treasuryCode);

        public Task<T?> GetPensionerDetailsByPpoIdAsync<T>(
            int ppoId,
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, T>> selectExpression
        );

        public Task<List<PensionerListItemDTO>> GetAllNotApprovedPensionerDetailsAsync(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, PensionerListItemDTO>> selectExpression
        );

        public Task<List<PaymentHistoryResponseDTO>> GetPensionerPaymentHistoryByPpoIdAsync(
            int ppoId,
            short financialYear,
            string treasuryCode
        );
    }
}
