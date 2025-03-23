using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ManualPpoReceiptRepository(IMapper mapper, PensionDbContext context)
        : IManualPpoReceiptRepository
    {
        protected readonly PensionDbContext _context = context;
        protected readonly IMapper _mapper = mapper;

        public async Task<List<T>> GetAllUnusedPpoReceipts<T>(
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoReceipt, T>> selectExpression
        )
        {
            return await _context
                .PpoReceipts.Where(entity =>
                    entity.ActiveFlag
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Pensioners)
                .Where(entity => entity.Pensioners.Count == 0)
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<List<T>> GetPpoReceiptsAsync<T>(
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoReceipt, T>> selectExpression
        )
        {
            return await _context
                .PpoReceipts.Where(entity =>
                    entity.ActiveFlag
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<T> CreatePpoReceiptWithTreasuryReceiptNo<T>(
            short finYear,
            string treasuryCode,
            PpoReceipt ppoReceiptEntity
        )
            where T : BaseDTO
        {
            T result = _mapper.Map<T>(ppoReceiptEntity);
            try
            {
                PpoReceiptSequence? ppoReceiptSequence = _context
                    .PpoReceiptSequences.Where(entity =>
                        entity.ActiveFlag == true
                        && entity.FinancialYear == finYear
                        && entity.TreasuryCode == treasuryCode
                    )
                    .FirstOrDefault();
                if (ppoReceiptSequence == null)
                {
                    ppoReceiptSequence = new()
                    {
                        FinancialYear = finYear,
                        TreasuryCode = treasuryCode,
                        NextSequenceValue = 1,
                    };
                    _context.Add(ppoReceiptSequence);
                }
                else
                {
                    ppoReceiptSequence.NextSequenceValue++;
                    _context.Update(ppoReceiptSequence);
                }
                string paddedNextSequenceValue = $"{ppoReceiptSequence.NextSequenceValue}".PadLeft(
                    6,
                    '0'
                );
                ppoReceiptEntity.TreasuryReceiptNo =
                    $"{treasuryCode}{finYear}{paddedNextSequenceValue}";
                _context.PpoReceipts.Add(ppoReceiptEntity);

                if (await _context.SaveChangesAsync() == 0)
                {
                    result.FillErrorInDataSource(ppoReceiptEntity, "Failed to add PPO Receipt");
                    return result;
                }
                result = _mapper.Map<T>(ppoReceiptEntity);
            }
            catch (Exception ex)
            {
                result.FillErrorInDataSource(
                    ppoReceiptEntity,
                    "Repository Exception: " + ex.InnerException?.Message ?? ex.Message
                );
                return result;
            }
            return result;
        }

        public IQueryable<PpoReceipt> GetQueryablePpoReceipts()
        {
            return _context.PpoReceipts;
        }

        public string GenerateTreasuryReceiptNo(short finYear, string treasuryCode)
        {
            PpoReceiptSequence? ppoReceiptSequence = _context
                .PpoReceiptSequences.Where(entity =>
                    entity.ActiveFlag == true
                    && entity.FinancialYear == finYear
                    && entity.TreasuryCode == treasuryCode
                )
                .FirstOrDefault();
            if (ppoReceiptSequence == null)
            {
                ppoReceiptSequence = new()
                {
                    FinancialYear = finYear,
                    TreasuryCode = treasuryCode,
                    NextSequenceValue = 1,
                };
                _context.Add(ppoReceiptSequence);
            }
            else
            {
                ppoReceiptSequence.NextSequenceValue++;
                _context.Update(ppoReceiptSequence);
            }
            string paddedNextSequenceValue = $"{ppoReceiptSequence.NextSequenceValue}".PadLeft(
                6,
                '0'
            );
            return $"{treasuryCode}{finYear}{paddedNextSequenceValue}";
        }
    }
}
