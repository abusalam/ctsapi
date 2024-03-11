using CTS_BE.BAL.Interfaces.paymandate;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Services.paymandate
{
    public class PaymandateService :IPaymandateService
    {
        private readonly ITokenRepository _tokenRepository;

        public PaymandateService(ITokenRepository TokenRepository) 
        {
            _tokenRepository = TokenRepository;
        }
        public async Task<IEnumerable<PayMandateShortListDTO>> TokenForShortList()
        {
            IEnumerable<PayMandateShortListDTO> payMandateShortListDTO = (IEnumerable<PayMandateShortListDTO>) await _tokenRepository.GetSelectedColumnByConditionAsync(entity => entity.TokenFlow.StatusId == (int)Enum.TokenStatus.FrowardbyTreasuryOfficer,

                entity => new PayMandateShortListDTO
                {
                    TokenId = entity.Id,
                    TokenDate = entity.TokenDate,
                    BillNo = entity. Bill.BillNo,
                    BillDate = entity.Bill.BillDate,
                    TRFormats = entity.Bill.TrMaster.WbFormCode,
                    BillTypes = "",
                    BillModule = "",
                    BillPeriod = "",
                    //NoOfBeneficiarie = 0,
                    NeAmount = entity.Bill.NetAmount,
                    //ECSAmount = 0,
                    //ChequeAmount = 0,
                    DetailHead = entity.Bill.DetailHead.ToString(),
                    HeadOfAccounts = new HOAChain
                    {
                        Demand = entity.Bill.Demand,
                        DetailHead = entity.Bill.DetailHead,
                        MajorHead = entity.Bill.MajorHead,
                        MinorHead = entity.Bill.MinorHead,
                        SchemeHead = entity.Bill.SchemeHead,
                        SubMajorHead = entity.Bill.SubMajorHead,
                        VotedCharged = entity.Bill.VotedCharged,
                    },
                });
            return payMandateShortListDTO;
        }
    }
}
