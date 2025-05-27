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
    public class PpoSanctionDetailsService(
        IMapper mapper,
        IPensionerDetailsRepository pensionerDetailsRepository,
        IPpoSanctionDetailsRepository ppoSanctionDetailsRepository,
        IClaimService claimService,
        ILogger<PpoSanctionDetailsService> logger
    ) : BaseService(claimService), IPpoSanctionDetailsService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository =
            pensionerDetailsRepository;
        private readonly IPpoSanctionDetailsRepository _ppoSanctionDetailsRepository =
            ppoSanctionDetailsRepository;
        private readonly ILogger<PpoSanctionDetailsService> _logger = logger;

        public async Task<T> GetSanctionDetailsById<T>(long sanctionDetailsId, string treasuryCode)
        {
            PpoSanctionDetail? sanctionDetails = new();
            T? response = _mapper.Map<T>(sanctionDetails);
            try
            {
                sanctionDetails = await _ppoSanctionDetailsRepository.GetSanctionDetailsByIdAsync(
                    sanctionDetailsId,
                    treasuryCode
                );

                if (sanctionDetails is null)
                {
                    _logger.LogWarning(
                        "Sanction details with ID {SanctionDetailsId} not found in treasury {TreasuryCode}.",
                        sanctionDetailsId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        sanctionDetails,
                        "Sanction details does not exist. Please check Id. and try again."
                    );
                    return response;
                }

                response = _mapper.Map<T>(sanctionDetails);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching sanction details with ID {SanctionDetailsId} in treasury {TreasuryCode}.",
                    sanctionDetailsId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    sanctionDetails,
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
            _logger.LogInformation(
                "Creating sanction details for PPO ID {PpoId} in financial year {FinancialYear} and treasury {TreasuryCode}.",
                ppoSanctionDetailsEntryDTO.PpoId,
                financialYear,
                treasuryCode
            );
            PpoSanctionDetail sanctionDetailsEntity = _mapper.Map<PpoSanctionDetail>(
                ppoSanctionDetailsEntryDTO
            );
            T? response = _mapper.Map<T>(sanctionDetailsEntity);
            try
            {
                Pensioner? pensioner =
                    await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                        ppoSanctionDetailsEntryDTO.PpoId,
                        financialYear,
                        treasuryCode,
                        entity => _mapper.Map<Pensioner>(entity)
                    );

                if (pensioner is null)
                {
                    _logger.LogWarning(
                        "Pensioner with PPO ID {PpoId} not found in financial year {FinancialYear} and treasury {TreasuryCode}.",
                        ppoSanctionDetailsEntryDTO.PpoId,
                        financialYear,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        pensioner,
                        "Pensioner not found. Please check PPO Id. and try again."
                    );
                    return response;
                }

                PpoSanctionDetail? sanctionDetails =
                    await _ppoSanctionDetailsRepository.GetSanctionDetailsByPpoIdAsync(
                        ppoSanctionDetailsEntryDTO.PpoId,
                        treasuryCode
                    );

                if (sanctionDetails is not null)
                {
                    _logger.LogWarning(
                        "Sanction details for PPO ID {PpoId} already exist in treasury {TreasuryCode}.",
                        ppoSanctionDetailsEntryDTO.PpoId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        sanctionDetails,
                        "Sanction details already exist. Please check PPO Id. and try again."
                    );
                    return response;
                }
                sanctionDetails = new() { TreasuryCode = treasuryCode };
                sanctionDetails.FillFrom(ppoSanctionDetailsEntryDTO);
                sanctionDetails.PensionerId = pensioner.Id;
                SetCreatedBy(sanctionDetails);

                return await _ppoSanctionDetailsRepository.AddNewSanctionDetails<T>(
                    sanctionDetails,
                    treasuryCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating sanction details for PPO ID {PpoId} in financial year {FinancialYear} and treasury {TreasuryCode}.",
                    ppoSanctionDetailsEntryDTO.PpoId,
                    financialYear,
                    treasuryCode
                );
                response.FillErrorInDataSource(
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
            _logger.LogInformation(
                "Updating sanction details with ID {SanctionDetailsId} for PPO ID {PpoId} in financial year {FinancialYear} and treasury {TreasuryCode}.",
                sanctionDetailsId,
                ppoSanctionDetailsEntryDTO.PpoId,
                financialYear,
                treasuryCode
            );
            PpoSanctionDetail sanctionDetailsEntity = _mapper.Map<PpoSanctionDetail>(
                ppoSanctionDetailsEntryDTO
            );
            T? response = _mapper.Map<T>(sanctionDetailsEntity);
            try
            {
                PpoSanctionDetail? sanctionDetails =
                    await _ppoSanctionDetailsRepository.GetSanctionDetailsByIdAsync(
                        sanctionDetailsId,
                        treasuryCode
                    );

                if (sanctionDetails is null)
                {
                    _logger.LogWarning(
                        "Sanction details with ID {SanctionDetailsId} not found in treasury {TreasuryCode}.",
                        sanctionDetailsId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        sanctionDetails,
                        "Sanction details does not exist. Please check Id. and try again."
                    );
                    return response;
                }

                Pensioner? pensioner =
                    await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                        ppoSanctionDetailsEntryDTO.PpoId,
                        financialYear,
                        treasuryCode,
                        entity => _mapper.Map<Pensioner>(entity)
                    );

                if (pensioner is null)
                {
                    response.FillErrorInDataSource(
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
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating sanction details with ID {SanctionDetailsId} for PPO ID {PpoId} in financial year {FinancialYear} and treasury {TreasuryCode}.",
                    sanctionDetailsId,
                    ppoSanctionDetailsEntryDTO.PpoId,
                    financialYear,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    sanctionDetailsEntity,
                    $"ServiceException: {ex.InnerException?.Message}"
                );
                return response;
            }
        }
    }
}
