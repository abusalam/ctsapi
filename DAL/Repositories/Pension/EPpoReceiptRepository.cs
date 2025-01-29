using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class EPpoReceiptRepository
        : Repository<EppoReceipt, PensionDbContext>,
            IEPpoReceiptRepository
    {
        private readonly IMapper _mapper;
        private readonly PensionDbContext _context;

        public EPpoReceiptRepository(IMapper mapper, PensionDbContext context)
            : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<EppoReceipt?> GetEPpoReceiptById(
            long receiptId,
            string treasuryCode,
            short financialYear
        )
        {
            return await _context
                .EppoReceipts.Include(e => e.PhotoFile)
                .Include(e => e.SignatureFile)
                .Include(e => e.EppoFile)
                .FirstOrDefaultAsync(e =>
                    e.Id == receiptId
                    && e.TreasuryCode == treasuryCode
                    && e.FinancialYear == financialYear
                );
        }

        public async Task<List<T>> GetUnusedEPpoReceipts<T>(
            string treasuryCode,
            short financialYear,
            Expression<Func<EppoReceipt, T>> selectExpression
        )
        {
            return await _context
                .EppoReceipts.Where(e =>
                    e.TreasuryCode == treasuryCode
                    && e.FinancialYear == financialYear
                    && e.PpoId == null
                    && e.Withdrawn == false
                )
                .Include(e => e.PhotoFile)
                .Include(e => e.SignatureFile)
                .Include(e => e.EppoFile)
                .Select(selectExpression)
                .ToListAsync();
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
                eppoReceiptEntity.FinancialYear = financialYear;
                eppoReceiptEntity.Withdrawn = false;
                EppoReceipt? eppoReceiptExists = await _context.EppoReceipts.FirstOrDefaultAsync(
                    e => e.PensionApplnNo == eppoReceiptEntity.PensionApplnNo
                );

                if (eppoReceiptExists != null)
                {
                    response.FillDataSource(
                        eppoReceiptExists,
                        "eppoReceipt already exists for Pension Application No: "
                            + eppoReceiptEntity.PensionApplnNo
                    );
                    return response;
                }

                FileExtensionContentTypeProvider provider = new();
                if (eppoReceiptEntity.EppoFile != null)
                {
                    eppoReceiptEntity.EppoFile.FilePath = treasuryCode + "/" + financialYear + "/";
                    var extension = provider.TryGetContentType(
                        eppoReceiptEntity.EppoFile.FileName,
                        out string? mimeType
                    );
                    eppoReceiptEntity.EppoFile.FileMimeType =
                        mimeType ?? "application/octet-stream";
                }

                if (eppoReceiptEntity.PhotoFile != null)
                {
                    eppoReceiptEntity.PhotoFile.FilePath = treasuryCode + "/" + financialYear + "/";
                    var extension = provider.TryGetContentType(
                        eppoReceiptEntity.PhotoFile.FileName,
                        out string? mimeType
                    );
                    eppoReceiptEntity.PhotoFile.FileMimeType =
                        mimeType ?? "application/octet-stream";
                }

                if (eppoReceiptEntity.SignatureFile != null)
                {
                    eppoReceiptEntity.SignatureFile.FilePath =
                        treasuryCode + "/" + financialYear + "/";
                    var extension = provider.TryGetContentType(
                        eppoReceiptEntity.SignatureFile.FileName,
                        out string? mimeType
                    );
                    eppoReceiptEntity.SignatureFile.FileMimeType =
                        mimeType ?? "application/octet-stream";
                }

                _context.EppoReceipts.Add(eppoReceiptEntity);
                await _context.SaveChangesAsync();

                var dateOfCommencement = eppoReceiptEntity.DateOfRetirement.AddDays(1);
                var ppoReceipt = new PpoReceipt
                {
                    PpoNo = eppoReceiptEntity.PpoNo,
                    ReceiptType = "EPPO",
                    TreasuryReceiptNo = eppoReceiptEntity.PensionApplnNo,
                    PsaCode = 'D',
                    PpoType = eppoReceiptEntity.PpoTypeCode,
                    PensionerName = eppoReceiptEntity.PensionerName,
                    MobileNumber = eppoReceiptEntity.MobileNumber,
                    DateOfCommencement = dateOfCommencement,
                    ReceiptDate = dateOfCommencement.AddDays(1),
                    TreasuryCode = treasuryCode,
                    FinancialYear = financialYear,
                    EppoReceiptId = eppoReceiptEntity.Id,
                    PpoStatus = "EPPO Received",
                    ActiveFlag = true,
                };

                _context.PpoReceipts.Add(ppoReceipt);

                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillDataSource(
                        eppoReceiptEntity,
                        "Failed to save PpoReceipt data. Please try again after sometime."
                    );
                    return response;
                }

                return _mapper.Map<T>(eppoReceiptEntity);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    eppoReceiptEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    eppoReceiptEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return response;
        }

        public async Task<EppoReceipt?> GetEPpoReceiptByPensionApplnNo(
            string pensionApplnNo,
            string treasuryCode,
            short financialYear
        )
        {
            return await _context.EppoReceipts.FirstOrDefaultAsync(e =>
                e.PensionApplnNo == pensionApplnNo
                && e.TreasuryCode == treasuryCode
                && e.FinancialYear == financialYear
            );
        }

        public async Task<EppoReceipt?> GetPpoIdByPpoNo(
            string ppoNo,
            string treasuryCode,
            short financialYear
        )
        {
            return await _context.EppoReceipts.FirstOrDefaultAsync(e =>
                e.PpoNo == ppoNo
                && e.TreasuryCode == treasuryCode
                && e.FinancialYear == financialYear
            );
        }

        public async Task<T> SaveRevisedEPpoReceipt<T>(
            EppoRevision entity,
            Expression<Func<EppoReceipt, T>> selectExpression
        )
        {
            var response = _mapper.Map<T>(entity);

            try
            {
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
                response.FillDataSource(
                    entity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    entity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return response;
        }

        public async Task<T> WithdrawEPpoReceipt<T>(
            string pensionApplnNo,
            EppoReceipt entity,
            string treasuryCode,
            short financialYear,
            string? reason,
            char flag,
            Expression<Func<EppoReceipt, T>> selectExpression
        )
        {
            T? response = _mapper.Map<T>(entity);
            try
            {
                _context.EppoReceipts.Update(entity);

                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillDataSource(
                        entity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }

                return _mapper.Map<T>(entity);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    entity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    entity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }
    }
}
