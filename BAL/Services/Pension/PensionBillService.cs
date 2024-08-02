using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;

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
                IMapper mapper
            )
        {
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _pensionerBankAccountRepository = pensionerBankAccountRepository;
            _categoryRepository = categoryRepository;
            _componentRateRepository = componentRateRepository;
            _breakupRepository = breakupRepository;
            // _pensionBillRepository = pensionBillRepository;
            _mapper = mapper;
        }

        public async Task<TResponse> GenerateFirstPensionBills<TResponse>(
                InitiateFirstPensionBillDTO initiateFirstPensionBillDTO,
                short financialYear,
                string treasuryCode
            )
        {
            Pensioner pensioner = await _pensionerDetailsRepository.GetSingleAysnc(
                    entity => entity.ActiveFlag
                    && entity.PpoId==initiateFirstPensionBillDTO.PpoId
                    && entity.TreasuryCode==treasuryCode
                    && entity.FinancialYear==financialYear
                );
                // Category category = await _categoryRepository.GetSingleAysnc(
                //     entity => entity.ActiveFlag
                //     && entity.Id==pensioner.PpoCategoryId
                //     && entity.TreasuryCode==treasuryCode
                //     && entity.FinancialYear==financialYear
                // )
            TResponse response = _mapper.Map<TResponse>(pensioner);
            return response;
        }
    }
}