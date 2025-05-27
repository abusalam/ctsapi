using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ManualPpoReceiptRepository(
        IMapper mapper,
        PensionDbContext context,
        ILogger<ManualPpoReceiptRepository> logger
    ) : IManualPpoReceiptRepository
    {
        protected readonly PensionDbContext _context = context;
        protected readonly IMapper _mapper = mapper;
        protected readonly ILogger<ManualPpoReceiptRepository> _logger = logger;

        public async Task<List<T>> GetAllUnusedPpoReceipts<T>(
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoReceipt, T>> selectExpression
        )
        {
            _logger.LogInformation(
                "Fetching all unused PPO receipts for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );
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
            _logger.LogInformation(
                "Fetching PPO receipts for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );
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
            _logger.LogInformation(
                "Creating PPO receipt with treasury receipt number for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                finYear,
                treasuryCode
            );
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
                    _logger.LogError(
                        "Failed to save PPO receipt with treasury receipt number for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                        finYear,
                        treasuryCode
                    );
                    result.FillErrorInDataSource(ppoReceiptEntity, "Failed to add PPO Receipt");
                    return result;
                }

                result = _mapper.Map<T>(ppoReceiptEntity);
                _logger.LogInformation(
                    "Successfully created PPO receipt with treasury receipt number for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                    finYear,
                    treasuryCode
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating PPO receipt with treasury receipt number for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                    finYear,
                    treasuryCode
                );
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
            _logger.LogInformation(
                "Generating treasury receipt number for financial year: {FinancialYear}, treasury code: {TreasuryCode}",
                finYear,
                treasuryCode
            );
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
