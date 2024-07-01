using AutoMapper;
using CTS_BE.BAL.Interfaces.stamp;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using CTS_BE.DTOs;
using CTS_BE.Enum;
using CTS_BE.Helper.Authentication;

namespace CTS_BE.BAL.Services.stamp
{
    public class StampWalletService : IStampWalletService
    {
        private readonly IStampWalletRepository _stampWalletRepo;
        private readonly IMapper _mapper;
        private readonly IClaimService _auth;

        public StampWalletService(
            IStampWalletRepository stampWalletRepo,
            IMapper mapper,
            IClaimService claim)
        {
            _stampWalletRepo = stampWalletRepo;
            _mapper = mapper;
            _auth = claim;
        }

        public async Task<bool> CreateOrUpdateStampWallet(StampWalletInsertDTO stampWallet)
        {
            if (stampWallet != null && (await _stampWalletRepo.WalletRefil(stampWallet.TreasuryCode, stampWallet.ClearBalance)))
            {
              
                    return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<short> GetWalletBalanceByTreasuryCode(string TreasuryCode)
        {
            var data = await _stampWalletRepo.GetSingleSelectedColumnByConditionAsync(
                    e => e.TreasuryCode == TreasuryCode,
                    e => new StampWalletBalanceDTO
                        {
                            ClearBalance = e.ClearBalance
                        }
                );
            if (data != null)
            {
                return data.ClearBalance;
            }
            return 0;
        }
    }
}