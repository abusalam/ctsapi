using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
        private readonly IBreakupRepository _breakupRepository;
        // private readonly IPensionBillRepository _pensionBillRepository;
        private readonly IMapper _mapper;
        public PensionBillService(
                IPensionerDetailsRepository pensionerDetailsRepository,
                IPensionerBankAccountRepository pensionerBankAccountRepository,
                ICategoryRepository categoryRepository,
                IComponentRateRepository componentRateRepository,
                IBreakupRepository breakupRepository,
                // IPensionBillRepository pensionBillRepository,
                IClaimService claimService,
                IMapper mapper
            ) : base(claimService)
        {
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _pensionerBankAccountRepository = pensionerBankAccountRepository;
            _categoryRepository = categoryRepository;
            _componentRateRepository = componentRateRepository;
            _breakupRepository = breakupRepository;
            // _pensionBillRepository = pensionBillRepository;
            _mapper = mapper;
        }

        public async Task<InitiateFirstPensionBillResponseDTO> GenerateFirstPensionBills(
                InitiateFirstPensionBillDTO initiateFirstPensionBillDTO,
                short financialYear,
                string treasuryCode
            )
        {

            PensionDbContext pensionDbContext = (PensionDbContext) _pensionerDetailsRepository.GetDbContext();
            Pensioner pensioner = await _pensionerDetailsRepository.GetSingleAysnc(
                    entity => entity.ActiveFlag
                    && entity.PpoId==initiateFirstPensionBillDTO.PpoId
                    && entity.TreasuryCode==treasuryCode
                );

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
            InitiateFirstPensionBillResponseDTO response = new (){
                // DataSource = dataSource,
                Pensioner = _mapper.Map<PensionerListItemDTO>(pensioner),
                BankAccount = _mapper.Map<PensionerBankAcDTO>(
                        await _pensionerBankAccountRepository.GetSingleAysnc(
                            entity => entity.ActiveFlag
                            && entity.PpoId==initiateFirstPensionBillDTO.PpoId
                            && entity.TreasuryCode==treasuryCode
                        )
                    ),
                PensionerPayments = PensionCalculator.CalculatePpoPayments(
                        pensioner.Category.ComponentRates,
                        pensioner.DateOfCommencement,
                        initiateFirstPensionBillDTO.ToDate,
                        pensioner.BasicPensionAmount
                    ),
                PensionCategory = _mapper.Map<PensionCategoryResponseDTO>(pensioner.Category),
                // ComponentRates = _mapper.Map<List<ComponentRateResponseDTO>>(pensioner.Category.ComponentRates)
                //     .OrderBy(entity => entity.EffectiveFromDate).ToList(),
                BillGeneratedUptoDate = initiateFirstPensionBillDTO.ToDate
            };

            return response;
        }
    }
}