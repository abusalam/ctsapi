using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class LifeCertificateRepository(
        IMapper mapper,
        PensionDbContext context,
        ILogger<LifeCertificateRepository> logger
    ) : ILifeCertificateRepository
    {
        private readonly IMapper _mapper = mapper;
        private readonly PensionDbContext _context = context;
        private readonly ILogger<LifeCertificateRepository> _logger = logger;

        public async Task<T?> GetLifeCertificateByPpoIdAsync<T>(
            long ppoId,
            string treasuryCode,
            Expression<Func<LifeCertificate, T>> selectExpression
        )
        {
            return await _context
                .LifeCertificates.Where(x => x.PpoId == ppoId && x.TreasuryCode == treasuryCode)
                .Select(selectExpression)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Pensioner>> GetPensionersWithLifeCertificatesByBranchId(
            long branchId,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Fetching pensioners with life certificates for branchId: {BranchId}, financialYear: {FinancialYear}, treasuryCode: {TreasuryCode}",
                branchId,
                financialYear,
                treasuryCode
            );
            return await _context
                .Pensioners.Include(p => p.LifeCertificates)
                .Where(p =>
                    p.Branch.Id == branchId
                    && p.TreasuryCode == treasuryCode
                    && p.FinancialYear == financialYear
                )
                .ToListAsync();
        }

        public async Task<T> CreateLifeCertificate<T>(
            LifeCertificate lifeCertificate,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Creating life certificate for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                lifeCertificate.PpoId,
                treasuryCode
            );
            T? response = _mapper.Map<T>(lifeCertificate);
            try
            {
                lifeCertificate.TreasuryCode = treasuryCode;
                lifeCertificate.DigitalMode = false;
                _context.LifeCertificates.Add(lifeCertificate);
                if (await _context.SaveChangesAsync() == 0)
                {
                    _logger.LogError(
                        "Failed to save life certificate for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                        lifeCertificate.PpoId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        lifeCertificate,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                _logger.LogInformation(
                    "Life certificate created successfully for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                    lifeCertificate.PpoId,
                    treasuryCode
                );
                return _mapper.Map<T>(lifeCertificate);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "DbUpdateException occurred while creating life certificate for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                    lifeCertificate.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    lifeCertificate,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception occurred while creating life certificate for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                    lifeCertificate.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    lifeCertificate,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> UpdateLifeCertificateByPpoId<T>(
            LifeCertificate lifeCertificateDetailEntity,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Updating life certificate for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                lifeCertificateDetailEntity.PpoId,
                treasuryCode
            );
            T? response = _mapper.Map<T>(lifeCertificateDetailEntity);
            try
            {
                // First, retrieve the existing entity
                var existingEntity = await _context.LifeCertificates.FirstOrDefaultAsync(x =>
                    x.PpoId == lifeCertificateDetailEntity.PpoId && x.TreasuryCode == treasuryCode
                );

                if (existingEntity == null)
                {
                    _logger.LogWarning(
                        "Life Certificate not found for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                        lifeCertificateDetailEntity.PpoId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        lifeCertificateDetailEntity,
                        "Life Certificate not found for update."
                    );

                    return response;
                }

                // Update the properties of the existing entity
                _context
                    .Entry(existingEntity)
                    .CurrentValues.SetValues(lifeCertificateDetailEntity);

                // Or alternatively, manually update specific properties:
                // existingEntity.AccountHolderName = lifeCertificateDetailEntity.AccountHolderName;
                // ... update other properties as needed

                if (await _context.SaveChangesAsync() == 0)
                {
                    _logger.LogError(
                        "Failed to update life certificate for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                        lifeCertificateDetailEntity.PpoId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        lifeCertificateDetailEntity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                _logger.LogInformation(
                    "Life certificate updated successfully for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                    lifeCertificateDetailEntity.PpoId,
                    treasuryCode
                );
                return _mapper.Map<T>(existingEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "DbUpdateException occurred while updating life certificate for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                    lifeCertificateDetailEntity.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    lifeCertificateDetailEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception occurred while updating life certificate for PPO ID: {PpoId}, Treasury Code: {TreasuryCode}",
                    lifeCertificateDetailEntity.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    lifeCertificateDetailEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }
    }
}
