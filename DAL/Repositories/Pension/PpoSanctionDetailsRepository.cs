using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoSanctionDetailsRepository(
        IMapper mapper,
        PensionDbContext context,
        ILogger<PpoSanctionDetailsRepository> logger
    ) : IPpoSanctionDetailsRepository
    {
        private readonly IMapper _mapper = mapper;
        private readonly PensionDbContext _context = context;
        private readonly ILogger<PpoSanctionDetailsRepository> _logger = logger;

        public async Task<PpoSanctionDetail?> GetSanctionDetailsByPpoIdAsync(
            int ppoId,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Fetching sanction details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                ppoId,
                treasuryCode
            );
            return await _context.PpoSanctionDetails.FirstOrDefaultAsync(x =>
                x.PpoId == ppoId && x.TreasuryCode == treasuryCode
            );
        }

        public async Task<PpoSanctionDetail?> GetSanctionDetailsByIdAsync(
            long ppoSanctionDetailsId,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Fetching sanction details for PPO Sanction Details ID: {PpoSanctionDetailsId} with Treasury Code: {TreasuryCode}",
                ppoSanctionDetailsId,
                treasuryCode
            );
            return await _context.PpoSanctionDetails.FirstOrDefaultAsync(x =>
                x.Id == ppoSanctionDetailsId && x.TreasuryCode == treasuryCode
            );
        }

        public async Task<T> AddNewSanctionDetails<T>(
            PpoSanctionDetail ppoSanctionDetail,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Adding new PPO Sanction Details with Treasury Code: {TreasuryCode}",
                treasuryCode
            );
            var response = _mapper.Map<T>(ppoSanctionDetail);
            try
            {
                ppoSanctionDetail.TreasuryCode = treasuryCode;
                _context.PpoSanctionDetails.Add(ppoSanctionDetail);
                if (await _context.SaveChangesAsync() == 0)
                {
                    _logger.LogError(
                        "Failed to save PPO Sanction Details for PPO ID: {PpoId}",
                        ppoSanctionDetail.PpoId
                    );
                    response.FillErrorInDataSource(
                        ppoSanctionDetail,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                _logger.LogInformation(
                    "Successfully added PPO Sanction Details for PPO ID: {PpoId}",
                    ppoSanctionDetail.PpoId
                );
                return _mapper.Map<T>(ppoSanctionDetail);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while adding PPO Sanction Details for PPO ID: {PpoId}",
                    ppoSanctionDetail.PpoId
                );
                response.FillErrorInDataSource(
                    ppoSanctionDetail,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An exception occurred while adding PPO Sanction Details for PPO ID: {PpoId}",
                    ppoSanctionDetail.PpoId
                );
                response.FillErrorInDataSource(
                    ppoSanctionDetail,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> UpdateSanctionDetails<T>(
            PpoSanctionDetail ppoSanctionDetailEntity,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Updating PPO Sanction Details with ID: {PpoSanctionDetailsId} and Treasury Code: {TreasuryCode}",
                ppoSanctionDetailEntity.Id,
                treasuryCode
            );
            T? response = _mapper.Map<T>(ppoSanctionDetailEntity);
            try
            {
                ppoSanctionDetailEntity.TreasuryCode = treasuryCode;
                _context.PpoSanctionDetails.Update(ppoSanctionDetailEntity);
                if (await _context.SaveChangesAsync() == 0)
                {
                    _logger.LogError(
                        "Failed to update PPO Sanction Details for PPO ID: {PpoId}",
                        ppoSanctionDetailEntity.PpoId
                    );
                    response.FillErrorInDataSource(
                        ppoSanctionDetailEntity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }

                _logger.LogInformation(
                    "Successfully updated PPO Sanction Details for PPO ID: {PpoId}",
                    ppoSanctionDetailEntity.PpoId
                );
                return _mapper.Map<T>(ppoSanctionDetailEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while updating PPO Sanction Details for PPO ID: {PpoId}",
                    ppoSanctionDetailEntity.PpoId
                );
                response.FillErrorInDataSource(
                    ppoSanctionDetailEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An exception occurred while updating PPO Sanction Details for PPO ID: {PpoId}",
                    ppoSanctionDetailEntity.PpoId
                );
                response.FillErrorInDataSource(
                    ppoSanctionDetailEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }
    }
}
