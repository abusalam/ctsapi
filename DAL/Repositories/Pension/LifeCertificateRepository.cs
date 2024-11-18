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
    public class LifeCertificateRepository : Repository<LifeCertificate, PensionDbContext>, ILifeCertificateRepository
    {
        private readonly IMapper _mapper;
        private readonly PensionDbContext _context;
        public LifeCertificateRepository(
            IMapper mapper,
            PensionDbContext context
        ) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<T>?> GetLifeCertificateByPpoIdAsync<T>(
            int ppoId,
            string treasuryCode,
            Expression<Func<LifeCertificate, T>> selectExpression
        )
        {
            return await _context.LifeCertificates
                .Where(entity => entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                )
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<T> CreateLifeCertificate<T>(
            LifeCertificate lifeCertificate,
            string treasuryCode
        )
        {

            T? response = _mapper.Map<T>(lifeCertificate);
            try {
                lifeCertificate.TreasuryCode = treasuryCode;
                _context.LifeCertificates.Add(lifeCertificate);
                if(await _context.SaveChangesAsync() == 0) {
                    response.FillDataSource(
                        lifeCertificate,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                return _mapper.Map<T>(lifeCertificate);
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                    lifeCertificate,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    lifeCertificate,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }
    }
}