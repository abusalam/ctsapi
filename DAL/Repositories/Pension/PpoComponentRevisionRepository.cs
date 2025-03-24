using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoComponentRevisionRepository(IMapper mapper, PensionDbContext context)
        : IPpoComponentRevisionRepository
    {
        private readonly PensionDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CheckPpoComponentRevisionExists(
            PpoComponentRevision ppoComponentRevision,
            short financialYear,
            string treasuryCode
        )
        {
            return await _context.PpoComponentRevisions.AnyAsync(entity =>
                entity.ActiveFlag
                // && entity.TreasuryCode == treasuryCode
                && entity.PpoId == ppoComponentRevision.PpoId
                && entity.RateId == ppoComponentRevision.RateId
                && entity.FromDate == ppoComponentRevision.FromDate
            );
        }

        public async Task<bool> CheckRateExists(
            long rateId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _context.ComponentRates.AnyAsync(entity =>
                entity.ActiveFlag && entity.Id == rateId
            );
        }

        public async Task<T> CreatePpoComponentRevision<T>(
            PpoComponentRevision ppoComponentRevision,
            short financialYear,
            string treasuryCode
        )
        {
            T response = _mapper.Map<T>(ppoComponentRevision);
            try
            {
                await _context.PpoComponentRevisions.AddAsync(ppoComponentRevision);

                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillErrorInDataSource(
                        ppoComponentRevision,
                        $"PPO Component Rate not saved!"
                    );
                    return response;
                }

                await _context
                    .Entry(ppoComponentRevision)
                    .Reference(entity => entity.Rate)
                    .LoadAsync();

                await _context
                    .Entry(ppoComponentRevision.Rate)
                    .Reference(entity => entity.Breakup)
                    .LoadAsync();

                return _mapper.Map<T>(ppoComponentRevision);
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    ppoComponentRevision,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> CreateSinglePpoComponentRevision<T>(
            PpoComponentRevision ppoComponentRevisionEntity
        )
        {
            T responseDTO = _mapper.Map<T>(ppoComponentRevisionEntity);

            try
            {
                await _context.PpoComponentRevisions.AddAsync(ppoComponentRevisionEntity);

                if (await _context.SaveChangesAsync() == 0)
                {
                    responseDTO.FillErrorInDataSource(
                        ppoComponentRevisionEntity,
                        "Failed to add PPO Component Revision record!"
                    );
                    return responseDTO;
                }

                // Load Rate and Breakup
                await _context
                    .Entry(ppoComponentRevisionEntity)
                    .Reference(entity => entity.Rate)
                    .LoadAsync();

                if (ppoComponentRevisionEntity.Rate != null)
                {
                    await _context
                        .Entry(ppoComponentRevisionEntity.Rate)
                        .Reference(rate => rate.Breakup)
                        .LoadAsync();
                }

                // Load Pensioner
                await _context
                    .Entry(ppoComponentRevisionEntity)
                    .Reference(entity => entity.Pensioner)
                    .LoadAsync();

                // Load PpoBillBreakups collection
                await _context
                    .Entry(ppoComponentRevisionEntity)
                    .Collection(entity => entity.PpoBillBreakups)
                    .LoadAsync();

                // Map updated entity with all related data to DTO
                responseDTO = _mapper.Map<T>(ppoComponentRevisionEntity);
            }
            catch (Exception ex)
            {
                responseDTO.FillErrorInDataSource(
                    responseDTO,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }

        public async Task<T> DeletePpoComponentRevisionById<T>(
            PpoComponentRevision ppoComponentRevision,
            short financialYear,
            string treasuryCode
        )
        {
            T? response = _mapper.Map<T>(ppoComponentRevision);
            try
            {
                _context.PpoComponentRevisions.Update(ppoComponentRevision);
                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillErrorInDataSource(
                        ppoComponentRevision,
                        $"PPO Component Rate not deleted!"
                    );
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    ppoComponentRevision,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return response;
        }

        public async Task<List<T>> GetAllPpos<T>(
            Expression<Func<Pensioner, T>> selectExpression,
            short financialYear,
            string treasuryCode
        )
        {
            return await _context
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoComponentRevisions.Count > 0
                )
                .Include(entity => entity.Branch)
                .ThenInclude(entity => entity.Bank)
                .Include(entity => entity.Category)
                .Include(entity => entity.PpoComponentRevisions)
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<List<T>> GetAllRevisionsByPpoIdAsync<T>(
            int ppoId,
            Expression<Func<PpoComponentRevision, T>> selectExpression,
            short financialYear,
            string treasuryCode
        )
        {
            var revisions = await _context
                .PpoComponentRevisions.Where(entity => entity.ActiveFlag && entity.PpoId == ppoId)
                .Include(entity => entity.Rate)
                .ThenInclude(entity => entity.Breakup)
                .Select(selectExpression)
                .ToListAsync();
            return revisions;
        }

        public async Task<PpoComponentRevision?> GetPpoComponentRevisionById(
            long revisionId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _context
                .PpoComponentRevisions.Where(entity => entity.ActiveFlag && entity.Id == revisionId)
                .Include(entity => entity.Rate)
                .ThenInclude(entity => entity.Breakup)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PpoComponentRevision>> GetRevisionsByPpoIdAndRateId(
            int ppoId,
            long rateId
        )
        {
            return await _context
                .PpoComponentRevisions.Where(entity =>
                    entity.ActiveFlag && entity.PpoId == ppoId && entity.RateId == rateId
                )
                .OrderByDescending(entity => entity.FromDate)
                .ToListAsync();
        }

        public async Task<T> UpdatePpoComponentRevision<T>(
            PpoComponentRevision ppoComponentRevision,
            short financialYear,
            string treasuryCode
        )
        {
            T? response = _mapper.Map<T>(ppoComponentRevision);
            try
            {
                _context.PpoComponentRevisions.Update(ppoComponentRevision);

                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillErrorInDataSource(
                        ppoComponentRevision,
                        $"PPO Component Rate not saved!"
                    );
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    ppoComponentRevision,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return response;
        }
    }
}
