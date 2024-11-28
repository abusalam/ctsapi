using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using CTS_BE.DTOs; 
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class EPpoReceiptRepository :
        Repository<EppoReceipt, PensionDbContext>,
        IEPpoReceiptRepository
    {
        private readonly IMapper _mapper;
        private readonly PensionDbContext _context;

        public EPpoReceiptRepository(
            IMapper mapper,
            PensionDbContext context
        ) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<T> SaveEPpoReceipt<T>(
            EppoReceipt eppoReceiptEntity,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression)
        {
            var response = _mapper.Map<T>(eppoReceiptEntity);
            try
            {
                eppoReceiptEntity.TreasuryCode = treasuryCode;
                eppoReceiptEntity.FinancialYear = financialYear;
                _context.EppoReceipts.Add(eppoReceiptEntity);
                
                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillDataSource(
                        eppoReceiptEntity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                
                return _mapper.Map<T>(eppoReceiptEntity);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(eppoReceiptEntity, $"DbException: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                response.FillDataSource(eppoReceiptEntity, $"ServiceException: {ex.InnerException?.Message ?? ex.Message}");
            }
            return response;
        }

        public async Task<T> GetEPpoReceiptByApplicationNo<T>(
            string applicationNo,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression)
        {
            var query = _context.EppoReceipts
                .Where(e => 
                    e.PensionApplnNo == applicationNo && 
                    e.TreasuryCode == treasuryCode && 
                    e.FinancialYear == financialYear);

            return await query.Select(selectExpression).FirstOrDefaultAsync();
        }

        public async Task<T> GetPpoIdByPpoNo<T>(
            string ppoNo,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression)
        {
            var query = _context.EppoReceipts
                .Where(e =>
                    e.PpoNo == ppoNo &&
                    e.TreasuryCode == treasuryCode &&
                    e.FinancialYear == financialYear);
            
            var firstOrDefault = await query.Select(selectExpression).FirstOrDefaultAsync();
            
            if (firstOrDefault == null)
            {
                System.Diagnostics.Debug.WriteLine($"No record found for PPO: {ppoNo}, Treasury: {treasuryCode}, FY: {financialYear}");
            }
            
            return firstOrDefault;
        }

        public async Task<T> SaveRevisedEPpoReceipt<T>(
            EppoRevision entity,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression)
        {
            var response = _mapper.Map<T>(entity);

            try 
            {
                entity.TreasuryCode = treasuryCode;
                entity.FinancialYear = financialYear;

                _context.EppoRevisions.Add(entity);
                
                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillDataSource(
                        entity, 
                        "Failed to save revised PPO. Please try again."
                    );
                    return response;
                }

                return _mapper.Map<T>(entity);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(entity, $"DbException: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                response.FillDataSource(entity, $"ServiceException: {ex.InnerException?.Message ?? ex.Message}");
            }

            return response;
        }

        public async Task<T> WithdrawEPpoReceipt<T>(
            string applicationNo,
            EppoReceipt entity,
            string treasuryCode,
            short financialYear,
            string reason,
            string flag,
            Expression<Func<EppoReceipt, T>> selectExpression)
        {
            try 
            {
                entity.PpoStatus = "Withdrawn";
                entity.WithdrawDate = DateOnly.FromDateTime(DateTime.UtcNow);
                
                _context.EppoReceipts.Update(entity);
                
                if (await _context.SaveChangesAsync() == 0)
                {
                    return (T)(object)new EPpoReceiptWithdrawlResponseDTO
                    {
                        ApplicationNo = applicationNo,
                        Status = "F",
                        ErrorCode = "Failed to withdraw PPO receipt. Please try again.",
                        Reason = reason,    
                        Flag = flag  
                    };
                }
                
                return (T)(object)new EPpoReceiptWithdrawlResponseDTO
                {
                    ApplicationNo = applicationNo,
                    Status = "E",  
                    ErrorCode = "-1", 
                    Reason = reason,    
                    Flag = flag 
                };
            }
            catch (DbUpdateException ex)
            {
                return (T)(object)new EPpoReceiptWithdrawlResponseDTO
                {
                    ApplicationNo = applicationNo,
                    Status = "F",
                    ErrorCode = $"DbException: {ex.InnerException?.Message ?? ex.Message}",
                    Reason = reason,    
                    Flag = flag 
                };
            }
            catch (Exception ex)
            {
                return (T)(object)new EPpoReceiptWithdrawlResponseDTO
                {
                    ApplicationNo = applicationNo,
                    Status = "F",
                    ErrorCode = $"ServiceException: {ex.InnerException?.Message ?? ex.Message}",
                    Reason = reason,   
                    Flag = flag 
                };
            }
        }
    }
}