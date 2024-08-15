using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoComponentRevisionService : IBaseService
    {
        public Task<TResponse> CreateSinglePpoComponentRevision<TEntry, TResponse>(
            int ppoId,
            TEntry ppoComponentRevisionDTO,
            short financialYear,
            string treasuryCode
        );

        public Task<List<TResponse>> CreatePpoComponentRevisions<TEntry, TResponse>(
            int ppoId,
            List<TEntry> ppoComponentRevisionDTOs,
            short financialYear,
            string treasuryCode
        );

        public Task<IEnumerable<TResponse>> GetPpoComponentRevisionsByPpoId<TResponse>(int ppoId, short financialYear, string treasuryCode);

        public Task<TResponse> UpdatePpoComponentRevisionById<TEntry, TResponse>(
            long revisionId,
            TEntry ppoComponentRevisionUpdateDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<TResponse> DeletePpoComponentRevisionById<TResponse>(
            long revisionId,
            short financialYear,
            string treasuryCode
        );
    }
}