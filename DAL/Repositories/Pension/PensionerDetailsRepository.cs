using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PensionerDetailsRepository : Repository<Pensioner, PensionDbContext>, IPensionerDetailsRepository
    {
        private readonly PensionDbContext _context;
        public PensionerDetailsRepository(PensionDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PensionerResponseDTO>> GetAllPensionerDetailsAsync(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, PensionerResponseDTO>> selectExpression
        )
        {
            var pensioners = await _context.Pensioners
                .Where(
                    entity => entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.BankAccounts)
                .Include(entity => entity.Category)
                .Include(entity => entity.Receipt)
                .Select(selectExpression)
                .ToListAsync();

            return pensioners;
        }

        public async Task<IEnumerable<PensionerListItemDTO>> GetAllNotApprovedPensionerDetailsAsync(
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, PensionerListItemDTO>> selectExpression
        )
        {
            var pensioners = await _context.Pensioners
                .Where(
                    entity => entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.BankAccounts)
                .Include(entity => entity.Category)
                .Include(entity => entity.Receipt)
                .Include(entity => entity.PpoStatusFlags)
                .Where(entity => !entity.PpoStatusFlags
                    .Any(entity => entity.ActiveFlag
                        && entity.StatusFlag == PpoStatus.PpoApproved
                    )
                )
                .Select(selectExpression)
                .ToListAsync();

            return pensioners;
        }

        public async Task<T?> GetPensionerDetailsByPpoIdAsync<T>(
            int ppoId,
            short financialYear,
            string treasuryCode,
            Expression<Func<Pensioner, T>> selectExpression
        )
        {
            T? pensioner = await _context.Pensioners
                .Where(
                    entity => entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.BankAccounts)
                .Include(entity => entity.Category)
                .Include(entity => entity.Receipt)
                .Select(selectExpression)
                .FirstOrDefaultAsync();
            return pensioner;
        }
    }
}