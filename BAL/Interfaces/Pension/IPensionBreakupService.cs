using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionBreakupService
    {
        public Task<TResponse> CreatePensionBreakup<TEntry, TResponse>(
            TEntry pensionBreakupEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<IEnumerable<TResponse>> ListBreakup<TResponse>(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        );
    }
}