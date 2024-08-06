using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoComponentRateService : BaseService, IPpoComponentRateService
    {
        private readonly IPpoComponentRateRepository _ppoComponentRateRepository;
        private readonly IMapper _mapper;
        public PpoComponentRateService(
                IPpoComponentRateRepository ppoComponentRateRepository,
                IMapper mapper,
                IClaimService claimService
            ) : base(claimService)
        {
            _mapper = mapper;
            _ppoComponentRateRepository = ppoComponentRateRepository;
        }

        public async Task<TResponse> CreatePpoComponentRate<TEntry, TResponse>(
            TEntry ppoComponentRateDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoComponentRate ppoComponentRate = new() {
                Id = 0
            };
            TResponse? response = _mapper.Map<TResponse>(ppoComponentRate);

            try {
                ppoComponentRate.FillFrom(ppoComponentRateDTO);
                SetCreatedBy(ppoComponentRate);
                ppoComponentRate.TreasuryCode = treasuryCode;
                _ppoComponentRateRepository.Add(ppoComponentRate);

                if(await _ppoComponentRateRepository.SaveChangesManagedAsync() == 0) {
                    response.FillDataSource(
                        ppoComponentRate,
                        $"Primary Category not saved!"
                    );
                    return response;
                }
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        ppoComponentRate,
                        $"ServiceException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                response.FillFrom(ppoComponentRate);
            }
            return response;
        }

        public async Task<IEnumerable<TResponse>> ListPpoComponentRates<TResponse>(
            short financialYear,
            string treasuryCode,
            DynamicListQueryParameters dynamicListQueryParameters
        )
        {
            _dataCount = _ppoComponentRateRepository.Count();
            return await _ppoComponentRateRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag,
                    entity => _mapper.Map<TResponse>(entity),
                    dynamicListQueryParameters
                );
        }
    }
}