using CTS_BE.DTOs;
namespace CTS_BE.BAL.Interfaces
{
    public interface IBranchService
    {
        public Task<IEnumerable<DropdownDTO>> GetBranchsByBankCode(short bankCode);
        public Task<BranchDeatilsDTO?> GetBranchByBranchCode(short branchCode);
    }
}