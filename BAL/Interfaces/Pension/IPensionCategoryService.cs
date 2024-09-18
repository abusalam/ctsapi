using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionCategoryService : IBaseService
    {
        public Task<TResponse> CreatePensionPrimaryCategory<TEntry, TResponse>(
            TEntry pensionPrimaryCategoryEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<IEnumerable<TResponse>> ListPrimaryCategory<TResponse>(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        );
        public Task<TResponse> CreatePensionSubCategory<TEntry, TResponse>(
            TEntry pensionSubCategoryEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<IEnumerable<TResponse>> ListSubCategory<TResponse>(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        );
        public Task<TResponse> CreatePensionCategory<TEntry, TResponse>(
            TEntry pensionCategoryEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<IEnumerable<TResponse>> ListPensionCategory<TResponse>(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        );

        public Task<T> GetPensionCategoryById<T>(
            long categoryId,
            short financialYear,
            string treasuryCode
        );
    }
}