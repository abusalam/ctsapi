using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoComponentRevisionRepository : IRepository<PpoComponentRevision>
    {

        public Task<List<T>> GetAllPpos<T>(
            Expression<Func<Pensioner, T>> selectExpression,
            short financialYear,
            string treasuryCode
        );
        public Task<List<T>> GetAllRevisionsByPpoIdAsync<T>(
            int ppoId,
            Expression<Func<PpoComponentRevision, T>> selectExpression,
            short financialYear,
            string treasuryCode
        );
    }
}