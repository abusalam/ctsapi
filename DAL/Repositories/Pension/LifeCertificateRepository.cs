using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class LifeCertificateRepository
        : Repository<LifeCertificate, PensionDbContext>,
            ILifeCertificateRepository
    {
        private readonly IMapper _mapper;
        private readonly PensionDbContext _context;

        public LifeCertificateRepository(IMapper mapper, PensionDbContext context)
            : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<T?> GetLifeCertificateByPpoIdAsync<T>(
            long ppoId,
            string treasuryCode,
            Expression<Func<LifeCertificate, T>> selectExpression
        )
        {
            return await _context
                .LifeCertificates.Where(x => x.PpoId == ppoId && x.TreasuryCode == treasuryCode)
                .Select(selectExpression)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Pensioner>> GetPensionersWithLifeCertificatesByBranchId(
            long branchId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _context
                .Pensioners.Include(p => p.LifeCertificates)
                .Where(p =>
                    p.Branch.Id == branchId
                    && p.TreasuryCode == treasuryCode
                    && p.FinancialYear == financialYear
                )
                .ToListAsync();
        }

        public async Task<T> CreateLifeCertificate<T>(
            LifeCertificate lifeCertificate,
            string treasuryCode
        )
        {
            T? response = _mapper.Map<T>(lifeCertificate);
            try
            {
                lifeCertificate.TreasuryCode = treasuryCode;
                lifeCertificate.DigitalMode = false;
                _context.LifeCertificates.Add(lifeCertificate);
                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillDataSource(
                        lifeCertificate,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                return _mapper.Map<T>(lifeCertificate);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    lifeCertificate,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    lifeCertificate,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> UpdateLifeCertificateByPpoId<T>(
            LifeCertificate lifeCertificateDetailEntity,
            string treasuryCode
        )
        {
            T? response = _mapper.Map<T>(lifeCertificateDetailEntity);
            try
            {
                // First, retrieve the existing entity
                var existingEntity = await _context.LifeCertificates.FirstOrDefaultAsync(x =>
                    x.PpoId == lifeCertificateDetailEntity.PpoId && x.TreasuryCode == treasuryCode
                );

                if (existingEntity == null)
                {
                    response.FillDataSource(
                        lifeCertificateDetailEntity,
                        "Life Certificate not found for update."
                    );
                    return response;
                }

                // Update the properties of the existing entity
                _context
                    .Entry(existingEntity)
                    .CurrentValues.SetValues(lifeCertificateDetailEntity);

                // Or alternatively, manually update specific properties:
                // existingEntity.AccountHolderName = lifeCertificateDetailEntity.AccountHolderName;
                // ... update other properties as needed

                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillDataSource(
                        lifeCertificateDetailEntity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                return _mapper.Map<T>(existingEntity);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    lifeCertificateDetailEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    lifeCertificateDetailEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }
    }
}
