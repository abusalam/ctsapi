using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class BankBranchRepository : Repository<Branch, PensionDbContext>, IBankBranchRepository
    {
        private readonly PensionDbContext _context;
        public BankBranchRepository(
            PensionDbContext context
        ) : base(context)
        {
            _context = context;
        }

        public async Task<Bank?> GetBankById(string treasuryCode, long bankId)
        {
            return await _context.Banks
            .Where(
                entity => entity.ActiveFlag
                && entity.Id == bankId
                && entity.TreasuryCode == treasuryCode
            )
            .FirstOrDefaultAsync();
        }

        public async Task<Branch?> GetBranchById(string treasuryCode, long branchId)
        {
            return await _context.Branches
            .Where(
                entity => entity.ActiveFlag
                && entity.Id == branchId
                && entity.TreasuryCode == treasuryCode
            )
            .Include(entity => entity.Bank)
            .FirstOrDefaultAsync();
        }

        public async Task<List<Bank>> GetAllBanks(string treasuryCode)
        {
            return await _context.Banks
            .Where(
                entity => entity.ActiveFlag
                && entity.TreasuryCode == treasuryCode
            )
            .ToListAsync();
        }

        public async Task<List<Branch>> GetBranchesByBankId(string treasuryCode, long bankId)
        {
            return await _context.Branches
            .Where(
                entity => entity.ActiveFlag
                && entity.BankId == bankId
                && entity.TreasuryCode == treasuryCode
            )
            .Include(entity => entity.Bank)
            .ToListAsync();
        }

        public async Task<string> GetBankBranchName(string treasuryCode, long branchId)
        {
            var bankBranchName = await _context.Branches.Where(
                entity => entity.Id == branchId
                && entity.TreasuryCode == treasuryCode
            )
            .Include(entity => entity.Bank)
            .Select(entity => entity.Bank.BankName + " - " + entity.BranchName)
            .FirstOrDefaultAsync() ?? "";
            return bankBranchName;
        }

        public async Task<string> GetBankBranchNameByPpoId(string treasuryCode, int ppoId)
        {
            var bankBranchName = await _context.Pensioners.Where(
                entity => entity.ActiveFlag
                && entity.PpoId == ppoId
                && entity.TreasuryCode == treasuryCode
            )
            .Include(entity => entity.Branch)
            .ThenInclude(entity => entity.Bank)
            .Select(entity => entity.Branch.Bank.BankName + " - " + entity.Branch.BranchName)
            .FirstOrDefaultAsync() ?? "";
            return bankBranchName;
        }
    }
}