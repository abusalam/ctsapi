using System;
using System.Collections.Generic;
using System.Dynamic;
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
    public class PpoComponentRevisionService : BaseService, IPpoComponentRevisionService
    {
        private readonly IPpoComponentRevisionRepository _ppoComponentRevisionRepository;
        private readonly IMapper _mapper;
        public PpoComponentRevisionService(
                IPpoComponentRevisionRepository ppoComponentRevisionRepository,
                IMapper mapper,
                IClaimService claimService
            ) : base(claimService)
        {
            _mapper = mapper;
            _ppoComponentRevisionRepository = ppoComponentRevisionRepository;
        }

        public async Task<TResponse> CreatePpoComponentRevision<TEntry, TResponse>(
            TEntry ppoComponentRevisionDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoComponentRevision ppoComponentRevision = new() {
                Id = 0
            };
            TResponse? response = _mapper.Map<TResponse>(ppoComponentRevision);

            try {
                ppoComponentRevision.FillFrom(ppoComponentRevisionDTO);

                PpoComponentRevision ppoComponentRevisionFound = await _ppoComponentRevisionRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.TreasuryCode == treasuryCode
                        && entity.PpoId == ppoComponentRevision.PpoId
                        && entity.BreakupId == ppoComponentRevision.BreakupId
                        && entity.FromDate == ppoComponentRevision.FromDate
                    );

                if(ppoComponentRevisionFound != null)
                {
                    ppoComponentRevision = ppoComponentRevisionFound;
                    response.FillDataSource(
                        ppoComponentRevisionFound,
                        $"PPO Component Revision already exists!"
                    );
                    return response;
                }
                SetCreatedBy(ppoComponentRevision);
                ppoComponentRevision.TreasuryCode = treasuryCode;
                _ppoComponentRevisionRepository.Add(ppoComponentRevision);

                if(await _ppoComponentRevisionRepository.SaveChangesManagedAsync() == 0) {
                    response.FillDataSource(
                        ppoComponentRevision,
                        $"PPO Component Rate not saved!"
                    );
                    return response;
                }
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        ppoComponentRevision,
                        $"DbUpdateException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                response.FillFrom(ppoComponentRevision);
            }
            return response;
        }

        public async Task<TResponse> UpdatePpoComponentRevisionById<TEntry, TResponse>(
            long revisionId,
            TEntry ppoComponentRevisionUpdateDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoComponentRevision ppoComponentRevision = new() {
                Id = 0
            };
            TResponse? response = _mapper.Map<TResponse>(ppoComponentRevision);

            try {

                ppoComponentRevision = await _ppoComponentRevisionRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.Id == revisionId
                    );

                if(ppoComponentRevision == null) {
                    response.FillDataSource(
                        ppoComponentRevision,
                        $"PPO Component RevisionId({revisionId}) not found!"
                    );
                    return response;
                }

                ppoComponentRevision.FillFrom(ppoComponentRevisionUpdateDTO);
                SetUpdatedBy(ppoComponentRevision);
                _ppoComponentRevisionRepository.Update(ppoComponentRevision);

                if(await _ppoComponentRevisionRepository.SaveChangesManagedAsync() == 0) {
                    response.FillDataSource(
                        ppoComponentRevision,
                        $"PPO Component Rate not saved!"
                    );
                    return response;
                }
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        ppoComponentRevision,
                        $"DbUpdateException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                response.FillFrom(ppoComponentRevision);
            }
            return response;
        }

        public async Task<IEnumerable<TResponse>> GetPpoComponentRevisionsByPpoId<TResponse>(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            // _dataCount = _ppoComponentRevisionRepository.Count();
            return await _ppoComponentRevisionRepository
                .GetSelectedColumnByConditionAsync(
                    entity => entity.ActiveFlag && entity.TreasuryCode == treasuryCode && entity.PpoId == ppoId,
                    entity => _mapper.Map<TResponse>(entity),
                    new DynamicListQueryParameters(){}
                );
        }

        public async Task<TResponse> DeletePpoComponentRevisionById<TResponse>(
            long revisionId,
            short financialYear,
            string treasuryCode
        )
        {
            PpoComponentRevision ppoComponentRevision = new() {
                Id = 0
            };
            TResponse? response = _mapper.Map<TResponse>(ppoComponentRevision);

            try {

                ppoComponentRevision = await _ppoComponentRevisionRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.Id == revisionId
                    );

                if(ppoComponentRevision == null) {
                    response.FillDataSource(
                        ppoComponentRevision,
                        $"PPO Component RevisionId({revisionId}) not found!"
                    );
                    return response;
                }

                ppoComponentRevision.ActiveFlag = false;
                SetUpdatedBy(ppoComponentRevision);
                _ppoComponentRevisionRepository.Update(ppoComponentRevision);

                if(await _ppoComponentRevisionRepository.SaveChangesManagedAsync() == 0) {
                    response.FillDataSource(
                        ppoComponentRevision,
                        $"PPO Component Rate not deleted!"
                    );
                    return response;
                }
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        ppoComponentRevision,
                        $"DbUpdateException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                response.FillFrom(ppoComponentRevision);
            }
            return response;
        }
    }
}