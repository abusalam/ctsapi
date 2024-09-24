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
        public Task<IEnumerable<TResponse>> ListComponentRates<TResponse>(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        );

        public Task<IEnumerable<TResponse>> ListComponentRatesByCategoryId<TResponse>(
            long categoryId
        );
    }
}