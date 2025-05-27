using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class NomineeRepository(
        IMapper mapper,
        PensionDbContext context,
        ILogger<NomineeRepository> logger
    ) : INomineeRepository
    {
        private readonly IMapper _mapper = mapper;
        private readonly PensionDbContext _context = context;
        private readonly ILogger<NomineeRepository> _logger = logger;

        public async Task<List<T>?> GetNomineeByPpoIdAsync<T>(
            int ppoId,
            string treasuryCode,
            Expression<Func<Nominee, T>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching nominees for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                ppoId,
                treasuryCode
            );
            return await _context
                .Nominees.Where(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(nominee => nominee.Branch)
                .ThenInclude(branch => branch == null ? null : branch.Bank)
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<T> SaveNomineeDetails<T>(Nominee nominee, string treasuryCode)
        {
            _logger.LogInformation(
                "Saving nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                nominee.PpoId,
                treasuryCode
            );
            T? response = _mapper.Map<T>(nominee);
            try
            {
                nominee.TreasuryCode = treasuryCode;
                _context.Nominees.Add(nominee);
                if (await _context.SaveChangesAsync() == 0)
                {
                    _logger.LogError(
                        "Failed to save nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                        nominee.PpoId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        nominee,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                _logger.LogInformation(
                    "Successfully saved nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                    nominee.PpoId,
                    treasuryCode
                );
                return _mapper.Map<T>(nominee);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "DbUpdateException occurred while saving nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                    nominee.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    nominee,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception occurred while saving nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                    nominee.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    nominee,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> UpdateNomineeDetails<T>(Nominee nomineeEntity, string treasuryCode)
        {
            _logger.LogInformation(
                "Updating nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                nomineeEntity.PpoId,
                treasuryCode
            );
            T? response = _mapper.Map<T>(nomineeEntity);
            try
            {
                nomineeEntity.TreasuryCode = treasuryCode;
                _context.Nominees.Update(nomineeEntity);
                if (await _context.SaveChangesAsync() == 0)
                {
                    _logger.LogError(
                        "Failed to update nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                        nomineeEntity.PpoId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        nomineeEntity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                _logger.LogInformation(
                    "Successfully updated nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                    nomineeEntity.PpoId,
                    treasuryCode
                );
                return _mapper.Map<T>(nomineeEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "DbUpdateException occurred while updating nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                    nomineeEntity.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    nomineeEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception occurred while updating nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                    nomineeEntity.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    nomineeEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> DeleteNomineeDetails<T>(Nominee nomineeEntity, string treasuryCode)
        {
            _logger.LogInformation(
                "Deleting nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                nomineeEntity.PpoId,
                treasuryCode
            );
            T? response = _mapper.Map<T>(nomineeEntity);
            try
            {
                nomineeEntity.TreasuryCode = treasuryCode;
                _context.Nominees.Update(nomineeEntity);
                if (await _context.SaveChangesAsync() == 0)
                {
                    _logger.LogError(
                        "Failed to delete nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                        nomineeEntity.PpoId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        nomineeEntity,
                        "Failed to delete data. Please try again after sometime."
                    );
                    return response;
                }
                _logger.LogInformation(
                    "Successfully deleted nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                    nomineeEntity.PpoId,
                    treasuryCode
                );
                return _mapper.Map<T>(nomineeEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "DbUpdateException occurred while deleting nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                    nomineeEntity.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    nomineeEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Exception occurred while deleting nominee details for PPO ID: {PpoId} with Treasury Code: {TreasuryCode}",
                    nomineeEntity.PpoId,
                    treasuryCode
                );
                response.FillErrorInDataSource(
                    nomineeEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T?> GetNomineeDetailsByIdAsync<T>(
            long nomineeId,
            string treasuryCode,
            Expression<Func<Nominee, T>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching nominee details for ID: {NomineeId} with Treasury Code: {TreasuryCode}",
                nomineeId,
                treasuryCode
            );
            return await _context
                .Nominees.Where(x => x.Id == nomineeId && x.TreasuryCode == treasuryCode)
                .Include(nominee => nominee.Branch)
                .ThenInclude(branch => branch == null ? null : branch.Bank)
                .Select(selectExpression)
                .FirstOrDefaultAsync();
        }
    }
}
