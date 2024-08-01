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
    public class PensionRateService : BaseService, IPensionRateService
    {
        private readonly IComponentRateRepository _pensionRateRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        public PensionRateService(
                IComponentRateRepository pensionRateRepository,
                IClaimService claimService,
                IMapper mapper
            )
        {
            _pensionRateRepository = pensionRateRepository;
            _claimService = claimService;
            _mapper = mapper;
        }
        public async Task<TResponse> CreatePensionRates<TEntry, TResponse>(TEntry pensionRateEntryDTO, short financialYear, string treasuryCode)
        {
            ComponentRate componentRateEntity = new() {
                Id = 0
            };
            TResponse? response = _mapper.Map<TResponse>(componentRateEntity);

            try {
                componentRateEntity.FillFrom(pensionRateEntryDTO);
                SetCreatedBy(componentRateEntity);
                
                _pensionRateRepository.Add(componentRateEntity);

                _dataCount=await _pensionRateRepository.SaveChangesManagedAsync();
                if(_dataCount == 0) {
                    response.FillDataSource(
                        componentRateEntity,
                        $"Primary Category not saved!"
                    );
                    return response;
                }
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        componentRateEntity,
                        $"ServiceException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                response.FillFrom(componentRateEntity);
            }
            return response;
        }

        public async Task<IEnumerable<TResponse>> ListRates<TResponse>(short financialYear, string treasuryCode, DynamicListQueryParameters dynamicListQueryParameters)
        {
            _dataCount = _pensionRateRepository.Count();
            return await _pensionRateRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag,
                    entity => _mapper.Map<TResponse>(entity),
                    dynamicListQueryParameters
                );        
        }
    }
}