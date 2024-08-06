using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoComponentRateService : IBaseService
    {
        public Task<TResponse> CreatePpoComponentRate<TEntry, TResponse>(TEntry ppoComponentRateDTO, short financialYear, string treasuryCode);
        public Task<IEnumerable<TResponse>> ListPpoComponentRates<TResponse>(short financialYear, string treasuryCode, DynamicListQueryParameters dynamicListQueryParameters);
    }
}