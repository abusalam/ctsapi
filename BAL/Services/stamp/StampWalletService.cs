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
            if (stampWallet != null && (await _stampWalletRepo.WalletRefil(stampWallet.TreasuryCode, stampWallet.CombinationId, stampWallet.AddSheet, stampWallet.AddLabel)))
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<StampWalletBalanceDTO> GetWalletBalanceByTreasuryCode(string TreasuryCode, long combinationId)
        {
            var data = await _stampWalletRepo.GetSingleSelectedColumnByConditionAsync(
                    e => e.TreasuryCode == TreasuryCode && e.Combination.StampCombinationId == combinationId,
                    e => new StampWalletBalanceDTO
                        {
                            SheetLedgerBalance = e.SheetLedgerBalance,
                            LabelLedgerBalance = e.LabelLedgerBalance,
                            Category = e.Combination.StampCategory.StampCategory1,
                            Denomination = e.Combination.StampType.Denomination
                        }
                );
            if (data != null)
            {
                return data;
            }
            return new StampWalletBalanceDTO
            {
                SheetLedgerBalance = 0,
                LabelLedgerBalance = 0
            };
        }
    }
}