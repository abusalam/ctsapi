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
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
        private readonly IMapper _mapper;
        public PpoComponentRevisionService(
                IPpoComponentRevisionRepository ppoComponentRevisionRepository,
                IPensionerDetailsRepository pensionerDetailsRepository,
                IMapper mapper,
                IClaimService claimService
            ) : base(claimService)
        {
            _mapper = mapper;
            _ppoComponentRevisionRepository = ppoComponentRevisionRepository;
            _pensionerDetailsRepository = pensionerDetailsRepository;
        }

        public async Task<TResponse> CreateSinglePpoComponentRevision<TEntry, TResponse>(
            int ppoId,
            TEntry ppoComponentRevisionDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoComponentRevision ppoComponentRevision = new() {
                Id = 0,
                PpoId = ppoId
            };
            TResponse? response = _mapper.Map<TResponse>(ppoComponentRevision);

            try {
                ppoComponentRevision.FillFrom(ppoComponentRevisionDTO);

                PpoComponentRevision ppoComponentRevisionFound = await _ppoComponentRevisionRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.TreasuryCode == treasuryCode
                        && entity.PpoId == ppoComponentRevision.PpoId
                        && entity.RateId == ppoComponentRevision.RateId
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
                
                Pensioner pensionerFound = await _pensionerDetailsRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.TreasuryCode == treasuryCode
                        && entity.PpoId == ppoId
                    );
                if(pensionerFound is null)
                {
                    response.FillDataSource(
                        ppoComponentRevisionDTO,
                        $"Pensioner not found!"
                    );
                    return response;
                }

                SetCreatedBy(ppoComponentRevision);
                ppoComponentRevision.TreasuryCode = treasuryCode;
                ppoComponentRevision.PensionerId = pensionerFound.Id;
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

        public async Task<List<TResponse>> CreatePpoComponentRevisions<TEntry, TResponse>(
            int ppoId,
            List<TEntry> ppoComponentRevisionDTOs,
            short financialYear,
            string treasuryCode
        )
        {
            List<PpoComponentRevision> ppoComponentRevisions = new();
            List<TResponse>? response = _mapper.Map<List<TResponse>>(ppoComponentRevisions);

            try {

                foreach(TEntry ppoComponentRevisionDTO in ppoComponentRevisionDTOs) {
                    PpoComponentRevision ppoComponentRevision = new(){
                        Id = 0,
                        PpoId = ppoId
                    };
                    ppoComponentRevision.FillFrom(ppoComponentRevisionDTO);

                    PpoComponentRevision ppoComponentRevisionFound = await _ppoComponentRevisionRepository.GetSingleAysnc(
                            entity => entity.ActiveFlag 
                            && entity.TreasuryCode == treasuryCode
                            && entity.PpoId == ppoId
                            && entity.RateId == ppoComponentRevision.RateId
                            && entity.FromDate == ppoComponentRevision.FromDate
                        );

                    if(ppoComponentRevisionFound != null)
                    {
                        ppoComponentRevision = ppoComponentRevisionFound;
                        ppoComponentRevisionDTO.FillDataSource(
                            ppoComponentRevisionFound,
                            $"PPO Component Revision already exists!"
                        );
                        continue;
                    }
                    Pensioner pensionerFound = await _pensionerDetailsRepository.GetSingleAysnc(
                        entity => entity.ActiveFlag 
                        && entity.TreasuryCode == treasuryCode
                        && entity.PpoId == ppoId
                    );
                    if(pensionerFound is null)
                    {
                        ppoComponentRevisionDTO.FillDataSource(
                            ppoComponentRevision,
                            $"Pensioner not found!"
                        );
                        continue;
                    }
                    SetCreatedBy(ppoComponentRevision);
                    ppoComponentRevision.TreasuryCode = treasuryCode;
                    ppoComponentRevision.PensionerId = pensionerFound.Id;
                    ppoComponentRevisions.Add(ppoComponentRevision);
                }
                await _ppoComponentRevisionRepository.GetDbContext().AddRangeAsync(ppoComponentRevisions);
                if(await _ppoComponentRevisionRepository.SaveChangesManagedAsync() == 0) {
                    response.FillDataSource(
                        ppoComponentRevisions,
                        $"PPO Component Rate not saved!"
                    );
                    return response;
                }

            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                        ppoComponentRevisions,
                        $"DbUpdateException: {ex.InnerException?.Message}"
                    );
            }
            finally {
                //TODO: Implement DataSource Channel for List<TResponse> 
                response = _mapper.Map<List<TResponse>>(ppoComponentRevisions);
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