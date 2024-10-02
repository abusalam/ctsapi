using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IBankBranchRepository : IRepository<Branch>
    {
        public Task<List<Bank>> GetAllBanks(string treasuryCode);
        public Task<Bank?> GetBankById(string treasuryCode, long bankId);
        public Task<Branch?> GetBranchById(string treasuryCode, long branchId);
        public Task<List<Branch>> GetBranchesByBankId(string treasuryCode, long bankId);
        public Task<string> GetBankBranchName(string treasuryCode, long branchId);
        public Task<string> GetBankBranchNameByPpoId(string treasuryCode, int ppoId);
    }
}