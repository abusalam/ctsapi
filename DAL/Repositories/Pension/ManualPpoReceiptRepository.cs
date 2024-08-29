using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ManualPpoReceiptRepository : 
        Repository<PpoReceipt, PensionDbContext>,
        IManualPpoReceiptRepository
    {
        protected readonly PensionDbContext _context;
        protected readonly IMapper _mapper;
        public ManualPpoReceiptRepository(
            IMapper mapper,
            PensionDbContext context
        ) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<ListAllPpoReceiptsResponseDTO>> GetAllUnusedPpoReceipts(
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoReceipt, ListAllPpoReceiptsResponseDTO>> selectExpression
        )
        {
            List<ListAllPpoReceiptsResponseDTO>? result = await _context.PpoReceipts
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


        public async Task<T> CreatePpoReceiptWithTreasuryReceiptNo<T>(
            short finYear,
            string treasuryCode,
            PpoReceipt ppoReceiptEntity
        ) where T : BaseDTO
        {
            T result = _mapper.Map<T>(ppoReceiptEntity);
            try {
                PpoReceiptSequence ppoReceiptSequence = _context.PpoReceiptSequences
                .Where(
                    entity => entity.ActiveFlag == true
                    && entity.FinancialYear == finYear
                    && entity.TreasuryCode == treasuryCode
                )
                .First();
                if(ppoReceiptSequence.NextSequenceValue > 0) {
                    ppoReceiptSequence.NextSequenceValue++;
                    _context.Update(ppoReceiptSequence);                   
                } else {
                    ppoReceiptSequence = new () {
                            FinancialYear = finYear,
                            TreasuryCode = treasuryCode,
                            NextSequenceValue = 1
                        };
                    _context.Add(ppoReceiptSequence);
                }
                string paddedNextSequenceValue = $"{ppoReceiptSequence.NextSequenceValue}".PadLeft(6,'0');
                ppoReceiptEntity.TreasuryReceiptNo = $"{treasuryCode}{finYear}{paddedNextSequenceValue}";
                _context.PpoReceipts.Add(ppoReceiptEntity);
                
                if(_context.SaveChanges() == 0) {
                    result.FillDataSource(
                        ppoReceiptEntity,
                        "Failed to add PPO Receipt"
                    );
                    return await Task.FromResult(result);
                }
                result = _mapper.Map<T>(ppoReceiptEntity);
            }
            catch (Exception ex) {
                result.FillDataSource(
                    ppoReceiptEntity,
                    ex.InnerException?.Message ?? ex.Message
                );
                return await Task.FromResult(result);
            }
            return await Task.FromResult(result);
        }

    }
}