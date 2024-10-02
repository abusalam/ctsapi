using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IBankBranchService : IBaseService
    {
        public Task<BankListResponseDTO> GetBanks(string treasuryCode);
        public Task<BranchListResponseDTO> GetBranchesByBankId(string treasuryCode, long bankId);
        public Task<BankBranchNameResponseDTO> GetBankBranchNameByPpoId(string treasuryCode, long ppoId);
        public Task<BankBranchNameResponseDTO> GetBankBranchNameByBranchId(string treasuryCode, long branchId);
    }
}