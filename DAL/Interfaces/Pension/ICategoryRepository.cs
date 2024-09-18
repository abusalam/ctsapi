using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<Category?> GetCategoryById(
            long categoryId,
            short financialYear,
            string treasuryCode
        );
    }
}