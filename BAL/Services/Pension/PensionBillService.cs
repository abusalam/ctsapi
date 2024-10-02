using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PensionBillService : BaseService, IPensionBillService
    {
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;

        private readonly ICategoryRepository _categoryRepository;
        private readonly IComponentRateRepository _componentRateRepository;
        private readonly IPpoComponentRevisionRepository _ppoComponentRevisionRepository;
        private readonly IBreakupRepository _breakupRepository;
        // private readonly IPensionBillRepository _pensionBillRepository;
        private readonly IMapper _mapper;
        public PensionBillService(
                IPensionerDetailsRepository pensionerDetailsRepository,
                ICategoryRepository categoryRepository,
                IComponentRateRepository componentRateRepository,
                IPpoComponentRevisionRepository ppoComponentRevisionRepository,
                IBreakupRepository breakupRepository,
                // IPensionBillRepository pensionBillRepository,
                IClaimService claimService,
                IMapper mapper
            ) : base(claimService)
        {
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _categoryRepository = categoryRepository;
            _ppoComponentRevisionRepository = ppoComponentRevisionRepository;
            _componentRateRepository = componentRateRepository;
            _breakupRepository = breakupRepository;
            // _pensionBillRepository = pensionBillRepository;
            _mapper = mapper;
        }

        public async Task<T> GenerateFirstPensionBill<T>(
            InitiateFirstPensionBillDTO initiateFirstPensionBillDTO,
            char billType,
            short financialYear,
            string treasuryCode
        ) where T : PensionerFirstBillResponseDTO
        {

            PensionDbContext pensionDbContext = (PensionDbContext) _pensionerDetailsRepository.GetDbContext();
            Pensioner pensioner = await _pensionerDetailsRepository.GetSingleAysnc(
                    entity => entity.ActiveFlag
                    && entity.PpoId==initiateFirstPensionBillDTO.PpoId
                    && entity.TreasuryCode==treasuryCode
                );

            if(pensioner == null) {
                InitiateFirstPensionBillResponseDTO errResponse = new ();
                errResponse.FillDataSource(
                    pensioner,
                    "Pensioner not found!"
                );
                return _mapper.Map<T>(errResponse);
            }

            pensionDbContext.Entry(pensioner)
                .Reference(entity => entity.Category)
                .Load();
            pensionDbContext.Entry(pensioner.Category)
                .Reference(entity => entity.PrimaryCategory)
                .Load();
            pensionDbContext.Entry(pensioner.Category)
                .Reference(entity => entity.SubCategory)
                .Load();
            pensionDbContext.Entry(pensioner)
                .Reference(entity => entity.Receipt)
                .Load();
            pensionDbContext.Entry(pensioner.Category)
                .Collection(entity => entity.ComponentRates)
                .Query().Where(entity => entity.ActiveFlag)
                .Load();

            foreach (ComponentRate componentRate in pensioner.Category.ComponentRates) {
                pensionDbContext.Entry(componentRate)
                .Reference(entity => entity.Breakup)
                .Load();
            }
            List<PpoPaymentListItemDTO>? ppoPayments = PensionCalculator.CalculatePpoPayments(
                        pensioner.Category.ComponentRates,
                        billType == BillType.FirstBill ? pensioner.DateOfCommencement 
                        : PensionCalculator.CalculatePeriodStartDate(initiateFirstPensionBillDTO.ToDate),
                        billType == BillType.FirstBill ? initiateFirstPensionBillDTO.ToDate 
                        : PensionCalculator.CalculatePeriodEndDate(initiateFirstPensionBillDTO.ToDate),
                        pensioner.BasicPensionAmount,
                        pensioner.CommutedPensionAmount    
                    );
            List<PpoBillBreakupResponseDTO> ppoBillBreakups = new();

            foreach (PpoPaymentListItemDTO ppoPayment in ppoPayments) {
                ppoBillBreakups.Add(new PpoBillBreakupResponseDTO()
                {
                    PpoId = pensioner.PpoId,
                    DrawnAmount = ppoPayment.DrawnAmount,
                    DueAmount = ppoPayment.DueAmount,
                    ComponentName = ppoPayment.ComponentName,
                    ComponentType = ppoPayment.ComponentType,
                    AmountPerMonth = ppoPayment.AmountPerMonth,
                    BaseAmount = ppoPayment.BaseAmount,
                    BreakupAmount = ppoPayment.AmountPerMonth * ppoPayment.PeriodInMonths,
                    FromDate = ppoPayment.FromDate,
                    ToDate = ppoPayment.ToDate,
                    Revision = new PpoComponentRevisionResponseDTO() {
                        AmountPerMonth = ppoPayment.AmountPerMonth,
                        FromDate = ppoPayment.FromDate,
                        RateId = ppoPayment.RateId
                    }
                });
            }
            InitiateFirstPensionBillResponseDTO response = new (){
                PpoId = initiateFirstPensionBillDTO.PpoId,
                BillType = billType,
                FromDate = pensioner.DateOfCommencement,
                BranchId = pensioner.BranchId,
                BillGeneratedUptoDate = initiateFirstPensionBillDTO.ToDate,
                TreasuryVoucherNo = "N/A",
                BillDate = initiateFirstPensionBillDTO.ToDate,
                Id = 0,
                GrossAmount = 0,
                NetAmount = 0,
            };

            // The instance of entity type cannot be tracked because another instance with the same key value for Pensioner is already being tracked
            if(typeof(T) == typeof(InitiateFirstPensionBillResponseDTO)) {
                response.Pensioner = _mapper.Map<PensionerResponseDTO>(pensioner);
                response.PensionerPayments = ppoPayments;
                response.GrossAmount = response.PensionerPayments.ToList().Sum(entity => entity.DueAmount);
                response.NetAmount = response.PensionerPayments.ToList().Sum(entity => entity.NetAmount);
            } else {
                response.PpoBillBreakups = ppoBillBreakups;
                response.GrossAmount = response.PpoBillBreakups.ToList().Sum(entity => entity.DueAmount);
                response.NetAmount = response.PpoBillBreakups.ToList().Sum(entity => entity.NetAmount);
            }
            return _mapper.Map<T>(response);
        }
    }
}