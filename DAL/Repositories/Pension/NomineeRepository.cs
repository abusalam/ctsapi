using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class NomineeRepository : Repository<Nominee, PensionDbContext>, INomineeRepository
    {
        private readonly IMapper _mapper;
        private readonly PensionDbContext _context;
        public NomineeRepository(
            IMapper mapper,
            PensionDbContext context
        ) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<T>?> GetNomineeByPpoIdAsync<T>(
            int ppoId,
            string treasuryCode,
            Expression<Func<Nominee, T>> selectExpression
        )
        {
            return await _context.Nominees
                .Where(entity => entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Branch)
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<T> SaveNomineeDetails<T>(
            Nominee nominee,
            string treasuryCode
        )
        {

            T? response = _mapper.Map<T>(nominee);
            try {
                nominee.TreasuryCode = treasuryCode;
                _context.Nominees.Add(nominee);
                if(await _context.SaveChangesAsync() == 0) {
                    response.FillDataSource(
                        nominee,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                return _mapper.Map<T>(nominee);
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                    nominee,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    nominee,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> UpdateNomineeDetails<T>(
            Nominee nomineeEntity,
            string treasuryCode
        )
        {

            T? response = _mapper.Map<T>(nomineeEntity);
            try {
                nomineeEntity.TreasuryCode = treasuryCode;
                _context.Nominees.Update(nomineeEntity);
                if(await _context.SaveChangesAsync() == 0) {
                    response.FillDataSource(
                        nomineeEntity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                return _mapper.Map<T>(nomineeEntity);
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                    nomineeEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    nomineeEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> DeleteNomineeDetails<T>(
            Nominee nomineeEntity,
            string treasuryCode
        )
        {

            T? response = _mapper.Map<T>(nomineeEntity);
            try {
                nomineeEntity.TreasuryCode = treasuryCode;
                _context.Nominees.Update(nomineeEntity);
                if(await _context.SaveChangesAsync() == 0) {
                    response.FillDataSource(
                        nomineeEntity,
                        "Failed to delete data. Please try again after sometime."
                    );
                    return response;
                }
                return _mapper.Map<T>(nomineeEntity);
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                    nomineeEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    nomineeEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T?> GetNomineeDetailsByIdAsync<T>(
            long nomineeId,
            string treasuryCode,
            Expression<Func<Nominee, T>> selectExpression
        )
        {
            return await _context.Nominees
                .Where(
                    x => x.Id == nomineeId
                    && x.TreasuryCode == treasuryCode
                )
                .Include(x => x.Branch)
                .Select(selectExpression)
                .FirstOrDefaultAsync();
        }

    }
}