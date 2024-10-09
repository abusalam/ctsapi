using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoComponentRevisionRepository :
        Repository<PpoComponentRevision, PensionDbContext>,
        IPpoComponentRevisionRepository
    {
        private readonly PensionDbContext _context;
        public PpoComponentRevisionRepository(
            PensionDbContext context
        ) : base(context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllPpos<T>(
            Expression<Func<Pensioner, T>> selectExpression,
            short financialYear,
            string treasuryCode
        )
        {
            var ppos = await _context.Pensioners
                .Where(
                    entity => entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoComponentRevisions.Count > 0
                )
                .Include(entity => entity.Branch)
                .ThenInclude(entity => entity.Bank)
                .Include(entity => entity.Category)
                .Include(entity => entity.PpoComponentRevisions)
                .Select(selectExpression)
                .ToListAsync();
            return ppos;
        }


        public async Task<List<T>> GetAllRevisionsByPpoIdAsync<T>(
            int ppoId,
            Expression<Func<PpoComponentRevision, T>> selectExpression,
            short financialYear,
            string treasuryCode
        )
        {
            var revisions = await _context.PpoComponentRevisions
                .Where(
                    entity => entity.ActiveFlag
                    && entity.PpoId == ppoId
                )
                .Include(entity => entity.Rate)
                .ThenInclude(entity => entity.Breakup)
                .Select(selectExpression)
                .ToListAsync();
            return revisions;
        }
    }
}