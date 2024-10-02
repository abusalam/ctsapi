using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.BAL.Services.Pension
{
    public class BankBranchService : BaseService, IBankBranchService
    {
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        private readonly IBankBranchRepository _bankBranchRepository;
        public BankBranchService(
            IClaimService claimService,
            IMapper mapper,
            IBankBranchRepository bankBranchRepository
        ) : base(claimService)
        {
            _claimService = claimService;
            _mapper = mapper;
            _bankBranchRepository = bankBranchRepository;

        }

        public Task<BankBranchNameResponseDTO> GetBankBranchNameByBranchId(
            string treasuryCode,
            long branchId
        )
        {
            throw new NotImplementedException();
        }

        public Task<BankBranchNameResponseDTO> GetBankBranchNameByPpoId(
            string treasuryCode,
            long ppoId
        )
        {
            throw new NotImplementedException();
        }

        public async Task<BankListResponseDTO> GetBanks(
            string treasuryCode
        )
        {
            BankListResponseDTO bankListDTO = new ();
            List<Bank>? bankEntityList = new();
            try{
                bankEntityList = await _bankBranchRepository.GetAllBanks(treasuryCode);
                bankListDTO.Banks = _mapper.Map<List<BankResponseDTO>>(bankEntityList);
            }
            catch (Exception ex) {
                bankListDTO.FillDataSource(
                    bankEntityList,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return bankListDTO;
            }
           return bankListDTO;
        }

        public async Task<BranchListResponseDTO> GetBranchesByBankId(
            string treasuryCode,
            long bankId
        )
        {
            BranchListResponseDTO branchListResponseDTO = new ();
            List<Branch>? branchEntityList = new();
            try{
                branchEntityList = await _bankBranchRepository.GetBranchesByBankId(treasuryCode, bankId);
                branchListResponseDTO.Branches = _mapper.Map<List<BranchResponseDTO>>(branchEntityList);
            }
            catch (Exception ex) {
                branchListResponseDTO.FillDataSource(
                    branchEntityList,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return branchListResponseDTO;
            }
           return branchListResponseDTO;
        }
    }
}