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
    public class PpoSanctionDetailsService : BaseService, IPpoSanctionDetailsService
    {
        private readonly IMapper _mapper;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository;
        private readonly IPpoSanctionDetailsRepository _ppoSanctionDetailsRepository;

        public PpoSanctionDetailsService(
            IMapper mapper,
            IPensionerDetailsRepository pensionerDetailsRepository,
            IPpoSanctionDetailsRepository ppoSanctionDetailsRepository,
            IClaimService claimService
        ) : base(claimService)
        {
            _mapper = mapper;
            _pensionerDetailsRepository = pensionerDetailsRepository;
            _ppoSanctionDetailsRepository = ppoSanctionDetailsRepository;
        }

        public async Task<T> GetSanctionDetailsById<T>(
            long sanctionDetailsId,
            string treasuryCode
        )
        {
            T? response = _mapper.Map<T>(new PpoSanctionDetail());
            try {
                PpoSanctionDetail? sanctionDetails = await _ppoSanctionDetailsRepository.GetSanctionDetailsByIdAsync(
                    sanctionDetailsId,
                    treasuryCode
                );

                if (sanctionDetails is null)
                {
                    response.FillDataSource(
                        sanctionDetails,
                        "Sanction details does not exist. Please check Id. and try again."
                    );
                    return response;
                }

                response = _mapper.Map<T>(sanctionDetails);
                return response;

            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                    new PpoSanctionDetail(),
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    new PpoSanctionDetail(),
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> CreateSanctionDetails<T>(
            PpoSanctionDetailsEntryDTO ppoSanctionDetailsEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            T? response =  _mapper.Map<T>(ppoSanctionDetailsEntryDTO);

            PpoSanctionDetail sanctionDetailsEntity = new();
            try
            {
                Pensioner? pensioner = await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                    ppoSanctionDetailsEntryDTO.PpoId,
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

                PpoSanctionDetail? sanctionDetails = await _ppoSanctionDetailsRepository.GetSanctionDetailsByPpoIdAsync(
                    ppoSanctionDetailsEntryDTO.PpoId,
                    treasuryCode
                );

                if (sanctionDetails is not null)
                {
                    response.FillDataSource(
                        sanctionDetails,
                        "Sanction details already exist. Please check PPO Id. and try again."
                    );
                    return response;
                }
                sanctionDetails = new (){
                    TreasuryCode = treasuryCode
                };
                sanctionDetails.FillFrom(ppoSanctionDetailsEntryDTO);
                sanctionDetails.PensionerId = pensioner.Id;
                SetCreatedBy(sanctionDetails);

                return await _ppoSanctionDetailsRepository.AddNewSanctionDetails<T>(
                    sanctionDetails,
                    treasuryCode
                );
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                        sanctionDetailsEntity,
                        $"DbException: {ex.InnerException?.Message}"
                    );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                        sanctionDetailsEntity,
                        $"ServiceException: {ex.InnerException?.Message}"
                    );
                return response;
            }
        }


        public async Task<T> UpdateSanctionDetailsById<T>(
            long sanctionDetailsId,
            PpoSanctionDetailsEntryDTO ppoSanctionDetailsEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            T? response =  _mapper.Map<T>(ppoSanctionDetailsEntryDTO);

            PpoSanctionDetail sanctionDetailsEntity = new();
            try
            {

                PpoSanctionDetail? sanctionDetails = await _ppoSanctionDetailsRepository.GetSanctionDetailsByIdAsync(
                    sanctionDetailsId,
                    treasuryCode
                );

                if (sanctionDetails is null)
                {
                    response.FillDataSource(
                        sanctionDetails,
                        "Sanction details does not exist. Please check Id. and try again."
                    );
                    return response;
                }

                Pensioner? pensioner = await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                    ppoSanctionDetailsEntryDTO.PpoId,
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

                sanctionDetails.FillFrom(ppoSanctionDetailsEntryDTO);
                sanctionDetails.PensionerId = pensioner.Id;
                SetUpdatedBy(sanctionDetails);

                return await _ppoSanctionDetailsRepository.UpdateSanctionDetails<T>(
                    sanctionDetails,
                    treasuryCode
                );
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                        sanctionDetailsEntity,
                        $"DbException: {ex.InnerException?.Message}"
                    );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                        sanctionDetailsEntity,
                        $"ServiceException: {ex.InnerException?.Message}"
                    );
                return response;
            }
        }
    }
}