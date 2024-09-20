using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;

namespace CTS_BE.BAL
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _BranchRepository;
        private readonly IMapper _mapper;
        public BranchService(
            IBranchRepository BranchRepository,
            IMapper mapper
        )
        {
            _BranchRepository = BranchRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DropdownDTO>> GetBranchsByBankCode(short bankCode)
        {
            IEnumerable<DropdownDTO> branchDropdownData = (IEnumerable<DropdownDTO>)await _BranchRepository.GetSelectedColumnByConditionAsync(entity=>entity.BankCode==bankCode&& entity.IsActive,
                entity=>new DropdownDTO
                {
                    Name = entity.BranchName,
                    Code = entity.BranchCode
                });
            return branchDropdownData;
        }
        public async Task<BranchDeatilsDTO?> GetBranchByBranchCode(short branchCode)
        {
            return await _BranchRepository.GetBranchByCode(branchCode);
        }
    }
}