using CTS_BE.DAL.Entities;
using CTS_BE.DTOs;
namespace CTS_BE.DAL.Interfaces
{
    public interface IBranchRepository: IRepository<Branch>
    {
        public Task<BranchDeatilsDTO?> GetBranchByCode(
            short branchCode
        );
    }
}