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
    public class PensionerDetailsRepository(PensionDbContext context, IMapper mapper)
        : IPensionerDetailsRepository
    {
        private readonly PensionDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<PensionerResponseDTO>> GetAllPensionerDetailsAsync(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, PensionerResponseDTO>> selectExpression
        )
        {
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
            T? response = _mapper.Map<T>(pensionerEntity);
            try
            {
                pensionerEntity.TreasuryCode = treasuryCode;
                _context.Pensioners.Update(pensionerEntity);
                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillErrorInDataSource(
                        pensionerEntity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                return _mapper.Map<T>(pensionerEntity);
            }
            catch (DbUpdateException ex)
            {
                response.FillErrorInDataSource(
                    pensionerEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
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
