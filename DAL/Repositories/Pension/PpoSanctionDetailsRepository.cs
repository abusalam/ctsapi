using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoSanctionDetailsRepository : Repository<PpoSanctionDetail, PensionDbContext>, IPpoSanctionDetailsRepository
    {
        private readonly IMapper _mapper;
        private readonly PensionDbContext _context;
        public PpoSanctionDetailsRepository(
            IMapper mapper,
            PensionDbContext context
        ) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PpoSanctionDetail?> GetSanctionDetailsByPpoIdAsync(
            int ppoId,
            string treasuryCode
        )
        {
            return await _context.PpoSanctionDetails
                .FirstOrDefaultAsync(
                    x => x.PpoId == ppoId
                    && x.TreasuryCode == treasuryCode
                );
        }

        public async Task<PpoSanctionDetail?> GetSanctionDetailsByIdAsync(
            long ppoSanctionDetailsId,
            string treasuryCode
        )
        {
            return await _context.PpoSanctionDetails
                .FirstOrDefaultAsync(
                    x => x.Id == ppoSanctionDetailsId
                    && x.TreasuryCode == treasuryCode
                );
        }

        public async Task<T> AddNewSanctionDetails<T>(
            PpoSanctionDetail ppoSanctionDetail,
            string treasuryCode
        )
        {

            var response = _mapper.Map<T>(ppoSanctionDetail);
            try {
                ppoSanctionDetail.TreasuryCode = treasuryCode;
                _context.PpoSanctionDetails.Add(ppoSanctionDetail);
                if(await _context.SaveChangesAsync() == 0) {
                    response.FillDataSource(
                        ppoSanctionDetail,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                return _mapper.Map<T>(ppoSanctionDetail);
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                    ppoSanctionDetail,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    ppoSanctionDetail,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> UpdateSanctionDetails<T>(
            PpoSanctionDetail ppoSanctionDetailEntity,
            string treasuryCode
        )
        {

            T? response = _mapper.Map<T>(ppoSanctionDetailEntity);
            try {
                ppoSanctionDetailEntity.TreasuryCode = treasuryCode;
                _context.PpoSanctionDetails.Update(ppoSanctionDetailEntity);
                if(await _context.SaveChangesAsync() == 0) {
                    response.FillDataSource(
                        ppoSanctionDetailEntity,
                        "Failed to save data. Please try again after sometime."
                    );
                    return response;
                }
                return _mapper.Map<T>(ppoSanctionDetailEntity);
            }
            catch (DbUpdateException ex) {
                response.FillDataSource(
                    ppoSanctionDetailEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    ppoSanctionDetailEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }
    }
}