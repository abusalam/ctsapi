using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IComponentRateService : IBaseService
    {
        public Task<TResponse> CreateComponentRates<TEntry, TResponse>(
            TEntry pensionRateEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<List<ComponentRateResponseDTO>> ListComponentRates(
            short financialYear,
            string treasuryCode
        );

        public Task<List<TResponse>> ListComponentRatesByCategoryId<TResponse>(long categoryId);
    }
}
