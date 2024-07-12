using AutoMapper;
using CTS_BE.BAL.Interfaces.billing;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.billing;
using CTS_BE.DTOs;
namespace CTS_BE.BAL.Services.billing
{
    public class DdoAllotmentBookedBillService : IDdoAllotmentBookedBillService
    {
        private readonly IDdoAllotmentBookedBillRepository _DdoAllotmentBookedBillRepository;
        private readonly IMapper _mapper;
        public DdoAllotmentBookedBillService(IDdoAllotmentBookedBillRepository DdoAllotmentBookedBillRepository, IMapper mapper)
        {
            _DdoAllotmentBookedBillRepository = DdoAllotmentBookedBillRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AllotmentDTO>> AllotmentDetailsByBillId(long billId)
        {
            return await _DdoAllotmentBookedBillRepository.GetSelectedColumnByConditionAsync(
                    entity=>entity.BillId == billId, entity => new AllotmentDTO{
                        AllotmentId = (long)entity.AllotmentId,
                        HOA = new HOAChain{
                            Demand =entity.ActiveHoa.DemandNo,
                            DetailHead = entity.ActiveHoa.DetailHead,
                            MajorHead = entity.ActiveHoa.MajorHead,
                            MinorHead = entity.ActiveHoa.MinorHead,
                            SchemeHead = entity.ActiveHoa.SchemeHead,
                            SubDetailHead = entity.ActiveHoa.SubdetailHead,
                            SubMajorHead = entity.ActiveHoa.SubmajorHead,
                            VotedCharged = entity.ActiveHoa.VotedCharged
                        },
                        CeilingAmount = (decimal) entity.AllotmentNavigation.CeilingAmount,
                        ActualBalanceAmount = (decimal)entity.AllotmentNavigation.CeilingAmount - ((decimal)entity.Allotment.ActualReleasedAmount==null?0:(decimal)entity.Allotment.ActualReleasedAmount),
                        BookedAmount = (decimal)entity.Amount
                    }
                );
        }
    }
}