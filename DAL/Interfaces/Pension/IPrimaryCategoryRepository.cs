using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPrimaryCategoryRepository : IRepository<PrimaryCategory>
    {
        public Task<List<PensionPrimaryCategoryResponseDTO>> GetPrimaryCategoriesAsync();
        public Task<List<AccountHeadListItemResponseDTO>> GetAccountHeadsAsync(
            short financialYear,
            string treasuryCode
        );
        public Task<PrimaryCategory?> GetPrimaryCategoryWithAccountHeadAsync(long id);
    }
}
