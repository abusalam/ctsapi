using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PensionerDetailsRepository(
        PensionDbContext context,
        IMapper mapper,
        ILogger<PensionerDetailsRepository> logger
    ) : IPensionerDetailsRepository
    {
        private readonly PensionDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<PensionerDetailsRepository> _logger = logger;

        public async Task<List<PensionerResponseDTO>> GetAllPensionerDetailsAsync(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, PensionerResponseDTO>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching all pensioner details for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );
            return await _context
                .Pensioners.Where(entity =>
                    entity.ActiveFlag && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Category)
                .Include(entity => entity.Receipt)
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<List<T>> GetPensionerListAsync<T>(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, T>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching pensioner list for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );
            return await _context
                .Pensioners.Where(entity =>
                    entity.ActiveFlag && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Category)
                .Include(entity => entity.Receipt)
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<List<PensionerListItemDTO>> GetAllNotApprovedPensionerDetailsAsync(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, PensionerListItemDTO>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching not approved pensioner details for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );
            return await _context
                .Pensioners.Where(entity =>
                    entity.ActiveFlag && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Category)
                .Include(entity => entity.Receipt)
                .Include(entity => entity.PpoStatusFlags)
                .Where(entity =>
                    !entity.PpoStatusFlags.Any(entity =>
                        entity.ActiveFlag && entity.StatusFlag == PensionStatusFlag.PpoApproved
                    )
                )
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<T?> GetPensionerDetailsByPpoIdAsync<T>(
            int ppoId,
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, T>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching pensioner details for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                ppoId,
                financialYear,
                treasuryCode
            );
            return await _context
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Category)
                .ThenInclude(entity => entity.PrimaryCategory)
                .Include(entity => entity.Category)
                .ThenInclude(entity => entity.SubCategory)
                .Include(entity => entity.Receipt)
                .Include(entity => entity.Branch)
                .ThenInclude(entity => entity.Bank)
                .Include(entity => entity.PpoSanctionDetails)
                .Include(entity => entity.PpoStatusFlags)
                .Select(selectExpression)
                .FirstOrDefaultAsync();
        }

        public async Task<T> UpdatePensionerDetails<T>(
            Pensioner pensionerEntity,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Updating pensioner details for PPO ID: {PpoId}, treasury code: {TreasuryCode}",
                pensionerEntity.PpoId,
                treasuryCode
            );
            T? response = _mapper.Map<T>(pensionerEntity);
            try
            {
                pensionerEntity.TreasuryCode = treasuryCode;
                _context.Pensioners.Update(pensionerEntity);
                if (await _context.SaveChangesAsync() == 0)
                {
                    _logger.LogError(
                        "Failed to save pensioner details for PPO ID: {PpoId}",
                        pensionerEntity.PpoId
                    );
                    response.FillErrorInDataSource(
                        pensionerEntity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                _logger.LogInformation(
                    "Successfully updated pensioner details for PPO ID: {PpoId}",
                    pensionerEntity.PpoId
                );
                return _mapper.Map<T>(pensionerEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while updating pensioner details for PPO ID: {PpoId}",
                    pensionerEntity.PpoId
                );
                response.FillErrorInDataSource(
                    pensionerEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating pensioner details for PPO ID: {PpoId}",
                    pensionerEntity.PpoId
                );
                response.FillErrorInDataSource(
                    pensionerEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<
            List<PpoPaymentHistoryResponseDTO>
        > GetPensionerPaymentHistoryByPpoIdAsync(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Fetching payment history for PPO ID: {PpoId}, financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                ppoId,
                financialYear,
                treasuryCode
            );
            return await _context
                .PpoBillBreakups.Include(b => b.Revision.Rate.Breakup)
                .Where(b =>
                    b.PpoId == ppoId
                    // && b.FinancialYear == financialYear
                    && b.TreasuryCode == treasuryCode
                )
                .Select(b => new PpoPaymentHistoryResponseDTO
                {
                    ComponentId = b.Revision.Rate.Breakup.Id,
                    ComponentName = b.Revision.Rate.Breakup.ComponentName,
                    BreakupAmount = b.BreakupAmount,
                    FromDate = b.FromDate,
                    ToDate = b.ToDate,
                })
                .ToListAsync();
        }
    }
}
