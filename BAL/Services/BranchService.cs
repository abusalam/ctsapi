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
        public BranchService(IBranchRepository BranchRepository, IMapper mapper) {
            _BranchRepository = BranchRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DropdownDTO>> GetBranchsByBankCode(short bankCode)
        {
            IEnumerable<DropdownDTO> branchDropdownData = (IEnumerable<DropdownDTO>)await _BranchRepository.GetSelectedColumnByConditionAsync(entity=>entity.BankCode==bankCode&& entity.IsActive,
                entity=>new DropdownDTO
                {
                    Name = entity.BranchName,
                    code = entity.BranchCode
                });
            return branchDropdownData;
        }
        public async Task<BranchDeatilsDTO> GetBranchByBranchCode(short branchCode)
        {
            BranchDeatilsDTO branchDetails = (BranchDeatilsDTO)await _BranchRepository.GetSingleSelectedColumnByConditionAsync(entity => entity.BranchCode == branchCode && entity.IsActive,
                entity => new BranchDeatilsDTO
                {
                    BranchName = entity.BranchName,
                    MircCode = entity.MicrCode,
                    BranchAddress = entity.Address,
                });
            return branchDetails;
        }
    }
}