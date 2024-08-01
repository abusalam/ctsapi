using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionRateService : IBaseService
    {
        public Task<TResponse> CreatePensionRates<TEntry, TResponse>(
            TEntry pensionRateEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<IEnumerable<TResponse>> ListRates<TResponse>(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        );
    }
}