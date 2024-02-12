using AutoMapper;
using CTS_BE.BAL.Interfaces.billing;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.billing;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Services.billing
{
    public class TpBillService : ITpBillService
    {
        private readonly ITpBillRepository _TpBillRepository;
        private readonly IMapper _mapper;
        public TpBillService(ITpBillRepository TpBillRepository, IMapper mapper)
        {
            _TpBillRepository = TpBillRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BillsListDTO>> Bills()
        {
            IEnumerable<BillsListDTO> result = await _TpBillRepository.GetSelectedColumnAsync(entity=>new BillsListDTO
            {
                BillId = entity.BillId,
                ReferenceNo = entity.ReferenceNo,
                DdoCode = entity.DdoCode,
                DdoDesignation = entity.Designation,
                BillNo = entity.BillNo,
                BillDate = entity.BillDate.ToString(),
            });

            return result;
        }
        public async Task<IEnumerable<BillsListDTO>> NewBills()
        {
            return await _TpBillRepository.AllNewBills("BAA");
        }
        public async Task<int> CountNewBills()
        {
            return await _TpBillRepository.NewBillCount("BAA");
        }
        public async Task<BillDetailsDetailsByRef> BillDetailsByRefNo(string refNo)
        {
            BillDetailsDetailsByRef billDetailsDetailsByRef = await _TpBillRepository.GetSingleSelectedColumnByConditionAsync(bill => bill.ReferenceNo == refNo,
                billData => new BillDetailsDetailsByRef
                {
                    ReferenceNo = billData.ReferenceNo,
                    BillDate = billData.BillDate,
                    BillNo = billData.BillNo,
                    BillType = billData.TrMaster.IsEmployee,
                    BillSubType = billData.TrMaster.WbFormCode,
                    DdoCode = billData.DdoCode,
                    DdoDesignation = billData.Designation,
                    PayeeDepartment = billData.DemandNavigation.Code,
                    HOAChain = new HOAChain
                    {
                        Demand = billData.Demand,
                        MajorHead = billData.MajorHead,
                        SubMajorHead = billData.SubMajorHead,
                        MinorHead = billData.MinorHead,
                        SchemeHead = billData.SchemeHead,
                        VotedCharged = billData.VotedCharged,
                        DetailHead = billData.DetailHead,
                    },
                    SubDeatilsHead = billData.TpSubdetailInfos.Select(subInfo => new SubDeatilsHeadDto
                    {
                        SubDeatils = subInfo.SubDetailHead,
                        Description = subInfo.SubDetailHead,
                        Amount = subInfo.Amount,
                        Allotments = new AllotmentDto
                        {
                            HOAChain = new HOAChain
                            {
                                Demand = subInfo.Demand,
                                MajorHead = subInfo.MajorHead,
                                SubMajorHead = subInfo.SubMajorHead,
                                MinorHead = subInfo.MinorHead,
                                SchemeHead = subInfo.SchemeHead,
                                VotedCharged = subInfo.VotedCharged,
                                DetailHead = subInfo.DetailHead,
                                SubDetailHead = subInfo.SubDetailHead,
                            },
                            AllotmentAmount = subInfo.DdoWallet.CeilingAmount,
                            PreviousBalance = 0,
                            AdjustedAmount = billData.GrossAmount,
                            BalanceAmount = 0,
                            FinalProjectDetails="",
                            OverDrawalAmount= 0,
                            SubDetailHead =  subInfo.SubDetailHead
                        }
                    }).ToList(),
                    GrossAmount = billData.GrossAmount,
                    NetAmount = billData.NetAmount,
                    TransferAmount = billData.BtAmount,
                    //TODO:: Single bill may have multipale BT
                    AgBTAmount = billData.TpBtdetails.FirstOrDefault().AgAmount,
                    TreasuryBTAmount = billData.TpBtdetails.FirstOrDefault().TotalAmount,
                    TotalBTAmount = billData.TpBtdetails.FirstOrDefault().TotalAmount,
                    SanctionNo = billData.WbSanctionNo,
                    SanctionDate =  billData.WbSanctionDate,
                });
            return billDetailsDetailsByRef;
        }
        public async Task<BillDetailsDetailsByRef> BillDetailsByBillId(long billId)
        {
            BillDetailsDetailsByRef billDetailsDetailsByRef = await _TpBillRepository.GetSingleSelectedColumnByConditionAsync(bill => bill.BillId == billId,
                billData => new BillDetailsDetailsByRef
                {
                    ReferenceNo = billData.ReferenceNo,
                    BillDate = billData.BillDate,
                    BillNo = billData.BillNo,
                    BillType = billData.TrMaster.IsEmployee,
                    BillSubType = billData.TrMaster.WbFormCode,
                    DdoCode = billData.DdoCode,
                    DdoDesignation = billData.Designation,
                    PayeeDepartment = billData.DemandNavigation.Code,
                    HOAChain = new HOAChain
                    {
                        Demand = billData.Demand,
                        MajorHead = billData.MajorHead,
                        SubMajorHead = billData.SubMajorHead,
                        MinorHead = billData.MinorHead,
                        SchemeHead = billData.SchemeHead,
                        VotedCharged = billData.VotedCharged,
                        DetailHead = billData.DetailHead,
                    },
                    SubDeatilsHead = billData.TpSubdetailInfos.Select(subInfo => new SubDeatilsHeadDto
                    {
                        SubDeatils = subInfo.SubDetailHead,
                        Description = subInfo.SubDetailHead,
                        Amount = subInfo.Amount,
                        Allotments = new AllotmentDto
                        {
                            HOAChain = new HOAChain
                            {
                                Demand = subInfo.Demand,
                                MajorHead = subInfo.MajorHead,
                                SubMajorHead = subInfo.SubMajorHead,
                                MinorHead = subInfo.MinorHead,
                                SchemeHead = subInfo.SchemeHead,
                                VotedCharged = subInfo.VotedCharged,
                                DetailHead = subInfo.DetailHead,
                                SubDetailHead = subInfo.SubDetailHead,
                            },
                            AllotmentAmount = subInfo.DdoWallet.CeilingAmount,
                            PreviousBalance = 0,
                            AdjustedAmount = billData.GrossAmount,
                            BalanceAmount = 0,
                            FinalProjectDetails="",
                            OverDrawalAmount= 0,
                            SubDetailHead =  subInfo.SubDetailHead
                        }
                    }).ToList(),
                    GrossAmount = billData.GrossAmount,
                    NetAmount = billData.NetAmount,
                    TransferAmount = billData.BtAmount,
                    //TODO:: Single bill may have multipale BT
                    AgBTAmount = billData.TpBtdetails.FirstOrDefault().AgAmount,
                    TreasuryBTAmount = billData.TpBtdetails.FirstOrDefault().TotalAmount,
                    TotalBTAmount = billData.TpBtdetails.FirstOrDefault().TotalAmount,
                    SanctionNo = billData.WbSanctionNo,
                    SanctionDate =  billData.WbSanctionDate,
                });
            return billDetailsDetailsByRef;
        }
        public async Task<int> BillCountByStatus(int statusCode)
        {
            return _TpBillRepository.CountWithCondition(entity=>entity.Status == statusCode );
        }
    }
}   