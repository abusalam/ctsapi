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

        public async Task<LifeCertificateListResponseDTO> GetLifeCertificateByPpoId(
            int ppoId,
            string treasuryCode
        )
        {
            List<LifeCertificate>? lifeCertificateList = new();
            LifeCertificateListResponseDTO response = new();
            try {
                lifeCertificateList = await _lifeCertificateRepository.GetLifeCertificateByPpoIdAsync(
                    ppoId,
                    treasuryCode,
                    entity => _mapper.Map<LifeCertificate>(entity)
                );

                response.LifeCertificates = _mapper.Map<List<LifeCertificateResponseDTO>>(lifeCertificateList);
                if (response.LifeCertificateCount == 0)
                {
                    response.FillDataSource(
                        lifeCertificateList,
                        "No Life Certificate has been registered yet."
                    );
                }
                return response;
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    lifeCertificateList,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    lifeCertificateList,
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


    }
}