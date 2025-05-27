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
        private readonly ILogger<BankBranchService> _logger;

        public BankBranchService(
            IClaimService claimService,
            IMapper mapper,
            IBankBranchRepository bankBranchRepository,
            ILogger<BankBranchService> logger
        )
            : base(claimService)
        {
            _claimService = claimService;
            _mapper = mapper;
            _bankBranchRepository = bankBranchRepository;
            _logger = logger;
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

        public async Task<BankListResponseDTO> GetBanks(string treasuryCode)
        {
            _logger.LogInformation(
                "Fetching all banks for treasury code: {TreasuryCode}",
                treasuryCode
            );
            BankListResponseDTO bankListDTO = new();
            List<Bank>? bankEntityList = new();
            try
            {
                bankEntityList = await _bankBranchRepository.GetAllBanks(treasuryCode);
                bankListDTO.Banks = _mapper.Map<List<BankResponseDTO>>(bankEntityList);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching banks for treasury code: {TreasuryCode}",
                    treasuryCode
                );
                bankListDTO.FillErrorInDataSource(
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
            _logger.LogInformation(
                "Fetching branches for bank ID: {BankId} in treasury code: {TreasuryCode}",
                bankId,
                treasuryCode
            );
            BranchListResponseDTO branchListResponseDTO = new();
            List<Branch>? branchEntityList = new();
            try
            {
                branchEntityList = await _bankBranchRepository.GetBranchesByBankId(
                    treasuryCode,
                    bankId
                );
                branchListResponseDTO.Branches = _mapper.Map<List<BranchListItemResponseDTO>>(
                    branchEntityList
                );
                branchListResponseDTO.Bank = _mapper.Map<BankResponseDTO>(
                    branchEntityList.FirstOrDefault()?.Bank
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching branches for bank ID: {BankId} in treasury code: {TreasuryCode}",
                    bankId,
                    treasuryCode
                );
                branchListResponseDTO.FillErrorInDataSource(
                    branchEntityList,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return branchListResponseDTO;
            }
            return branchListResponseDTO;
        }
    }
}
