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
    public class ManualPpoReceiptRepository : 
        Repository<PpoReceipt, PensionDbContext>,
        IManualPpoReceiptRepository
    {
        protected readonly PensionDbContext _context;
        public ManualPpoReceiptRepository(PensionDbContext context) : 
            base(context)
        {
            _context = context;
        }

        public async Task<List<ListAllPpoReceiptsResponseDTO>> GetAllUnusedPpoReceipts(
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoReceipt, ListAllPpoReceiptsResponseDTO>> selectExpression
        )
        {
            var result = await _context.PpoReceipts
                .Where(
                    entity => entity.ActiveFlag
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Pensioners)
                .Where(entity => entity.Pensioners.Count == 0)
                .Select(selectExpression)
                .ToListAsync();
            return result;
        }

    }
}