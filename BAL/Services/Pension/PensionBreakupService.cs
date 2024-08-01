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
    public class PensionBreakupService : BaseService, IPensionBreakupService
    {
        private readonly IBillBreakupRepository _billBreakupRepository;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;
        public PensionBreakupService(
                IBillBreakupRepository billBreakupRepository,
                IClaimService claimService,
                IMapper mapper
            )
        {
            _billBreakupRepository = billBreakupRepository;
            _claimService = claimService;
            _mapper = mapper;
            _userId = _claimService.GetUserId();
        }
        public async Task<TResponse> CreatePensionBreakup<TEntry, TResponse>(TEntry pensionBreakupEntryDTO, short financialYear, string treasuryCode)
        {
            Component breakupEntity = new() {
                Id = 0
            };
            TResponse? response = _mapper.Map<TResponse>(breakupEntity);

            try {
                breakupEntity.FillFrom(pensionBreakupEntryDTO);
                SetCreatedBy(breakupEntity);
                
                _billBreakupRepository.Add(breakupEntity);

                if(await _billBreakupRepository.SaveChangesManagedAsync() == 0) {
                    response.FillDataSource(
                        breakupEntity,
                        $"Primary Category not saved!"
                    );
                    return response;
                }
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        breakupEntity,
                        $"ServiceException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                response.FillFrom(breakupEntity);
            }
            return response;
        }

        public async Task<IEnumerable<TResponse>> ListBreakup<TResponse>(short financialYear, string treasuryCode, DynamicListQueryParameters dynamicListQueryParameters)
        {
            _dataCount = _billBreakupRepository.Count();
            return await _billBreakupRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag,
                    entity => _mapper.Map<TResponse>(entity),
                    dynamicListQueryParameters
                );
        }
    }
}