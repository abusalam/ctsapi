using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using Microsoft.AspNetCore.Builder.Extensions;
namespace CTS_BE.BAL
{
    public class EcsNeftDetailService : IEcsNeftDetailService
    {
        private readonly IEcsNeftDetailRepository _EcsNeftDetailRepository;
        private readonly IMapper _mapper;
        public EcsNeftDetailService(IEcsNeftDetailRepository EcsNeftDetailRepository, IMapper mapper)
        {
            _EcsNeftDetailRepository = EcsNeftDetailRepository;
            _mapper = mapper;
        }
        public async Task<ECSNEFT> ECSByBillId(long billId)
        {
            return await _EcsNeftDetailRepository.GetSingleSelectedColumnByConditionAsync(
                 entity => entity.BillId == billId, entity => new ECSNEFT
                 {
                     BillNo = entity.Bill.BillNo,
                     BillDate = entity.Bill.BillDate.ToString("dd/MM/yyyy"),
                     GrossAmount = entity.Bill.GrossAmount,
                     NetAmount = entity.Bill.NetAmount,
                     ChequeAmount = 0,
                     PayMode = entity.Bill.PaymentMode == 1 ? "ECS/NEFT" : "BOTH",
                     Beneficiarys = new List<BeneficiaryDetailsDTO>
                     {
                        new BeneficiaryDetailsDTO
                        {
                            BeneficiaryName = entity.PayeeName,
                            BeneficiaryCode = "",
                            Amount = entity.Amount,
                            IFSCCode = entity.IfscCode,
                            BankName = entity.BankName,
                            AccountNumber = entity.BankAccountNumber,
                            ContactNo = entity.ContactNumber,
                            EMail = entity.Email,
                            PayeeType = "",
                        }
                     }
                 }
             );
        }
        public async Task<int> countBeneficiariesByBillId(long billId)
        {
            return await  _EcsNeftDetailRepository.CountWithConditionAsync(entity => entity.BillId == billId);
        }
    }
}
