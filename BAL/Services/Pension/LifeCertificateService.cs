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
    public class LifeCertificateService : BaseService,ILifeCertificateService
    {
        private readonly IMapper _mapper;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
        private readonly IBankBranchRepository _bankBranchRepository;
        private readonly ILifeCertificateRepository _lifeCertificateRepository;

        public LifeCertificateService(
            IMapper mapper,
            IPensionerDetailsRepository pensionerDetailsRepository,
            IBankBranchRepository bankBranchRepository,
            ILifeCertificateRepository lifeCertificateRepository,
            IClaimService claimService
        ) : base(claimService)
        {
            _mapper = mapper;
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _bankBranchRepository = bankBranchRepository;
            _lifeCertificateRepository = lifeCertificateRepository;
        }

        public async Task<T> GetLifeCertificateByPpoId<T>(
            long ppoId,
            string treasuryCode
        )
        {
            T? response = _mapper.Map<T>(new LifeCertificate());
            try {
                LifeCertificate? lifeCertificateDetails = await _lifeCertificateRepository.GetLifeCertificateByPpoIdAsync(
                    ppoId,
                    treasuryCode,
                    entity => _mapper.Map<LifeCertificate>(entity)
                );

                if (lifeCertificateDetails is null)
                {
                    response.FillDataSource(
                        lifeCertificateDetails,
                        "Life Certificate does not exist. Please check PPOId. and try again."
                    );
                    return response;
                }

                response = _mapper.Map<T>(lifeCertificateDetails);
                return response;

            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                    new LifeCertificate(),
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    new LifeCertificate(),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> CreateLifeCertificate<T>(
            LifeCertificateEntryDTO lifeCertificateEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            LifeCertificate lifeCertificateEntity = new();
            T? response = _mapper.Map<T>(lifeCertificateEntryDTO);
            try
            {
                Pensioner? pensioner = await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                    lifeCertificateEntryDTO.PpoId,
                    financialYear,
                    treasuryCode,
                    entity => _mapper.Map<Pensioner>(entity)
                );

                if (pensioner is null)
                {
                    response.FillDataSource(
                        pensioner,
                        "Pensioner not found. Please check PPO Id. and try again."
                    );
                    return response;
                }

                lifeCertificateEntity.FillFrom(lifeCertificateEntryDTO);
                lifeCertificateEntity.TreasuryCode = treasuryCode;
                lifeCertificateEntity.PensionerId = pensioner.Id;
                SetCreatedBy(lifeCertificateEntity);
                return await _lifeCertificateRepository.CreateLifeCertificate<T>(
                    lifeCertificateEntity,
                    treasuryCode
                );
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    lifeCertificateEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    lifeCertificateEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> UpdateLifeCertificateByPpoId<T>(
            long ppoId,
            LifeCertificateEntryDTO lifeCertificateEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            T? response =  _mapper.Map<T>(lifeCertificateEntryDTO);
            LifeCertificate lifeCertificateEntity = new();

            try
            {

                LifeCertificate? lifeCertificateDetails = await _lifeCertificateRepository.GetLifeCertificateByPpoIdAsync(
                    ppoId,
                    treasuryCode,
                    entity => _mapper.Map<LifeCertificate>(entity)
                );

                if (lifeCertificateDetails is null)
                {
                    response.FillDataSource(
                        lifeCertificateDetails,
                        " Life Certificate does not exist. Please check PPO Id. and try again."
                    );
                    return response;
                }

                long branchId = lifeCertificateEntryDTO.BranchId ?? 0;
                if (branchId > 0)
                {
                    Branch? branch = await _bankBranchRepository.GetBranchById(
                        treasuryCode,
                        branchId
                    );
                    if (branch is null)
                    {
                        response.FillDataSource(
                            lifeCertificateEntity,
                            "Branch not found. Please check branch Id. and try again."
                        );
                        return response;
                    }
                }

                lifeCertificateEntity.FillFrom(lifeCertificateEntryDTO);
                lifeCertificateEntity.Id = lifeCertificateDetails.Id;
                lifeCertificateEntity.PensionerId = lifeCertificateDetails.PensionerId;
                lifeCertificateEntity.TreasuryCode = lifeCertificateDetails.TreasuryCode;
                lifeCertificateEntity.PpoId = lifeCertificateDetails.PpoId;
                SetUpdatedBy(lifeCertificateEntity);

                return await _lifeCertificateRepository.UpdateLifeCertificateByPpoId<T>(
                    lifeCertificateEntity,
                    treasuryCode
                );
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                        lifeCertificateEntity,
                        $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                    );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                        lifeCertificateEntity,
                        $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                    );
                return response;
            }
        }

    }
}