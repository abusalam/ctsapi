using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
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

        public Task<T> GetEPpoReceiptByApplicationNo<T>(
            string applicationNo,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        )
        {
            throw new NotImplementedException();
        }

        public Task<T> GetPpoIdByPpoNo<T>(
            string ppoNo,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        )
        {
            throw new NotImplementedException();
        }

        public async Task<T> SaveEPpoReceipt<T>(
            EppoReceipt eppoReceiptEntity,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        )
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

        public Task<T> SaveRevisedEPpoReceipt<T>(
            EppoRevision entity,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        )
        {
            throw new NotImplementedException();
        }

        public Task<T> WithdrawEPpoReceipt<T>(
            string applicationNo,
            EppoReceipt entity,
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        )
        {
            throw new NotImplementedException();
        }
    }
}