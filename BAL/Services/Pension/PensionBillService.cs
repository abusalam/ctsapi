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
        private readonly IPensionerBankAccountRepository _pensionerBankAccountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IComponentRateRepository _componentRateRepository;
        private readonly IPpoComponentRevisionRepository _ppoComponentRevisionRepository;
        private readonly IBreakupRepository _breakupRepository;
        // private readonly IPensionBillRepository _pensionBillRepository;
        private readonly IMapper _mapper;
        public PensionBillService(
                IPensionerDetailsRepository pensionerDetailsRepository,
                IPensionerBankAccountRepository pensionerBankAccountRepository,
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
            _pensionerBankAccountRepository = pensionerBankAccountRepository;
            _categoryRepository = categoryRepository;
            _ppoComponentRevisionRepository = ppoComponentRevisionRepository;
            _componentRateRepository = componentRateRepository;
            _breakupRepository = breakupRepository;
            // _pensionBillRepository = pensionBillRepository;
            _mapper = mapper;
        }

        public async Task<T> GenerateFirstPensionBill<T>(
                InitiateFirstPensionBillDTO initiateFirstPensionBillDTO,
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
                T errResponse = default!;
                errResponse.FillDataSource(
                    pensioner,
                    "No such PPO exists"
                );
                return errResponse;
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

            pensionDbContext.Entry(pensioner)
                .Collection(entity => entity.BankAccounts)
                .Query().Where(entity => entity.ActiveFlag)
                .Load();

            // List<PpoPaymentListItemDTO>? ppoPayments = new();

            // DateOnly calculatedPeriodStartDate = initiateFirstPensionBillDTO.ToDate;
            // long prevBreakupId = 0;
            foreach (ComponentRate componentRate in pensioner.Category.ComponentRates) {
                pensionDbContext.Entry(componentRate)
                .Reference(entity => entity.Breakup)
                .Load();
                // $"{componentRate.Breakup.Id}, {componentRate.Breakup.ComponentName}".PrintOut();
                // if(componentRate.Breakup.Id != prevBreakupId) {
                //     calculatedPeriodStartDate = initiateFirstPensionBillDTO.ToDate;
                // }
                // prevBreakupId = componentRate.Breakup.Id;
                // ppoPayments.Add(new PpoPaymentListItemDTO()
                // {
                //     RateId = componentRate.Id,
                //     ComponentName = componentRate.Breakup.ComponentName,
                //     RateType = componentRate.RateType,
                //     RateAmount = componentRate.RateAmount,
                //     BasicPensionAmount = pensioner.BasicPensionAmount,
                //     BreakupAmount = PensionCalculator.CalculatePerMonthBreakupAmount(
                //         PensionCalculator.CalculateEffectiveRate(
                //             pensioner.Category.ComponentRates.ToList(),
                //             componentRate.Breakup.Id,
                //             calculatedPeriodStartDate      
                //         ),
                //         pensioner.BasicPensionAmount
                //         ),
                //     FromDate = PensionCalculator.CalculatePeriodStartFromDate(
                //             PensionCalculator.CalculateEffectiveRate(
                //                     pensioner.Category.ComponentRates.ToList(),
                //                     componentRate.Breakup.Id,
                //                     calculatedPeriodStartDate.AddDays(-1)
                //                 ).EffectiveFromDate,
                //             pensioner.DateOfCommencement
                //         ),
                //     ToDate = calculatedPeriodStartDate.AddDays(-1),
                // });
                // calculatedPeriodStartDate = componentRate.EffectiveFromDate;
            }

            // PensionCalculator.CalculatePpoPaymentStartFromDate(ref ppoPayments, pensioner.DateOfCommencement);

            // List<ComponentRate> ppoComponentRate = new();
            // List<long> componentBreakupIds = new();
            // componentBreakupIds = PensionCalculator.CalculateBreakups(pensioner.Category.ComponentRates.ToList());
            // componentBreakupIds.ForEach(breakupId => {
            //     ppoComponentRate.Add(PensionCalculator.CalculateEffectiveRate(
            //             pensioner.Category.ComponentRates.ToList(),
            //             breakupId,
            //             initiateFirstPensionBillDTO.ToDate      
            //         ));
            // });

            // dynamic dataSource = new ExpandoObject();
            // dataSource.DateValue = initiateFirstPensionBillDTO.ToDate;
            // dataSource.BreakupIds = componentBreakupIds;
            // dataSource.RateIds = _mapper.Map<List<ComponentRateResponseDTO>>(ppoComponentRate);
            // dataSource.MinDateValue = PensionCalculator.CalculatePeriodStartDate(initiateFirstPensionBillDTO.ToDate);
            // dataSource.MaxDateValue = PensionCalculator.CalculatePeriodEndDate(initiateFirstPensionBillDTO.ToDate);

            // dataSource.Pensioner = _mapper.Map<PensionerResponseDTO>(pensioner);
            // dataSource.PpoPayments = PensionCalculator.CalculatePpoPayments(
            //         pensioner.Category.ComponentRates,
            //         pensioner.DateOfCommencement,
            //         initiateFirstPensionBillDTO.ToDate,
            //         pensioner.BasicPensionAmount
            //     );
            // dataSource.PpoTotalPaymentPerMonth = ppoPayments.Sum(entity => entity.BreakupAmount);
            // dataSource.PpoFirstBillTotalAmount = PensionCalculator.CalculateTotalPensionAmount(
            //     pensioner.Category.ComponentRates.ToList(),
            //     pensioner.BasicPensionAmount,
            //     12
            // );
            List<PpoPaymentListItemDTO>? ppoPayments = PensionCalculator.CalculatePpoPayments(
                        pensioner.Category.ComponentRates,
                        pensioner.DateOfCommencement,
                        initiateFirstPensionBillDTO.ToDate,
                        pensioner.BasicPensionAmount
                    );
            List<PpoBillBreakupResponseDTO> ppoBillBreakups = new();

            foreach (PpoPaymentListItemDTO ppoPayment in ppoPayments) {
                ppoBillBreakups.Add(new PpoBillBreakupResponseDTO()
                {
                    PpoId = pensioner.PpoId,
                    DrawnAmount = pensioner.BasicPensionAmount,
                    DueAmount = (int) ppoPayment.DueAmount,
                    BreakupAmount = (int) ppoPayment.AmountPerMonth * ppoPayment.PeriodInMonths,
                    FromDate = ppoPayment.FromDate,
                    ToDate = ppoPayment.ToDate,
                    Revision = new PpoComponentRevisionResponseDTO() {
                        AmountPerMonth = (int) ppoPayment.AmountPerMonth,
                        FromDate = ppoPayment.FromDate,
                        RateId = ppoPayment.RateId
                    }
                });
            }
            InitiateFirstPensionBillResponseDTO response = new (){
                // DataSource = dataSource,
                // Pensioner = _mapper.Map<PensionerListItemDTO>(pensioner),
                // BankAccount = _mapper.Map<PensionerBankAcResponseDTO>(
                //         await _pensionerBankAccountRepository.GetSingleAysnc(
                //             entity => entity.ActiveFlag
                //             && entity.PpoId==initiateFirstPensionBillDTO.PpoId
                //             && entity.TreasuryCode==treasuryCode
                //         )
                //     ),
                PpoId = initiateFirstPensionBillDTO.PpoId,
                // PensionCategory = _mapper.Map<PensionCategoryResponseDTO>(pensioner.Category),
                // ComponentRates = _mapper.Map<List<ComponentRateResponseDTO>>(pensioner.Category.ComponentRates)
                //     .OrderBy(entity => entity.EffectiveFromDate).ToList(),
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


            // ************************************
            // Save Pensioner Component Revisions
            // ************************************

            // pensioner.PpoComponentRevisions = _mapper.Map<List<PpoComponentRevision>>(response.PensionerPayments.ToList());

            // pensioner.PpoComponentRevisions.ToList().ForEach(entity => {
            //     entity.TreasuryCode = treasuryCode;
            //     entity.ActiveFlag = true;
            //     entity.PpoId = initiateFirstPensionBillDTO.PpoId;
            //     entity.ToDate = (initiateFirstPensionBillDTO.ToDate == entity.ToDate) ? null : entity.ToDate;
            // });

            // pensionDbContext.Pensioners.Update(pensioner);
            // await pensionDbContext.SaveChangesAsync();
            return _mapper.Map<T>(response);
        }
    }
}