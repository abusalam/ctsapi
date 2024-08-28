using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPensionerDetailsRepository : IRepository<Pensioner>
    {
        public Task<IEnumerable<PensionerResponseDTO>> GetAllPensionerDetailsAsync(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, PensionerResponseDTO>> selectExpression
        );

        public Task<T?> GetPensionerDetailsByPpoIdAsync<T>(
            int ppoId,
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, T>> selectExpression
        );
    }
}