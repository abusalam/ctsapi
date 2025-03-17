using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoByTransferAmountRepository(PensionDbContext context, IMapper mapper)
        : Repository<PpoBytransfer, PensionDbContext>(context),
            IPpoByTransferAmountRepository
    {
        private readonly PensionDbContext _pensionDbContext = context;
        private readonly IMapper _mapper = mapper;

        public async Task<PpoBytransfer?> GetPPOByTransferByIdAsync(long id)
        {
            var ppoByTransfer = await _pensionDbContext
                .PpoBytransfers.Where(entity => entity.Id == id && entity.ActiveFlag)
                .FirstOrDefaultAsync();

            return ppoByTransfer;
        }

        public async Task<T> CreatePpoByTransfer<T>(PpoBytransfer ppobyTransferHeadEntity)
        {
            T responseDTO = _mapper.Map<T>(ppobyTransferHeadEntity);

            try
            {
                await _pensionDbContext.PpoBytransfers.AddAsync(ppobyTransferHeadEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    responseDTO.FillDataSource(ppobyTransferHeadEntity, "Failed to add record!");
                }

                _pensionDbContext
                    .Entry(ppobyTransferHeadEntity)
                    .Reference(entity => entity.BytransferHead)
                    .Load();

                responseDTO = _mapper.Map<T>(ppobyTransferHeadEntity);
            }
            catch (Exception ex)
            {
                responseDTO.FillDataSource(
                    responseDTO,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }

        public async Task<T> UpdatePpoByTransfer<T>(PpoBytransfer ppoByTransferEntity)
        {
            T responseDTO = _mapper.Map<T>(ppoByTransferEntity);

            try
            {
                _pensionDbContext.PpoBytransfers.Update(ppoByTransferEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    responseDTO.FillDataSource(ppoByTransferEntity, "Failed to Update record!");
                }

                _pensionDbContext
                    .Entry(ppoByTransferEntity)
                    .Reference(entity => entity.BytransferHead)
                    .Load();

                responseDTO = _mapper.Map<T>(ppoByTransferEntity);
            }
            catch (Exception ex)
            {
                responseDTO.FillDataSource(
                    responseDTO,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }

        public async Task<List<T>> GetAllPpoByTransferByPpoIdAsync<T>(
            Expression<Func<PpoBytransfer, T>> selectExpression,
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .PpoBytransfers.Where(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Pensioner)
                .Include(entity => entity.BytransferHead)
                .Select(selectExpression)
                .ToListAsync();
        }

        public async Task<bool> IsUsedInOtherTables(long byTransferHeadId)
        {
            bool isUsed = await _pensionDbContext.BillBytransfers.AnyAsync(b =>
                b.BytransferHeadId == byTransferHeadId
            );

            return isUsed;
        }

        public async Task<T> RemovePpoByTransferAsync<T>(long id)
        {
            T responseDTO = _mapper.Map<T>(new PpoByTransferAmountResponseDTO());

            try
            {
                var existingEntity = await _pensionDbContext.PpoBytransfers.FindAsync(id);

                if (existingEntity == null)
                {
                    responseDTO.FillDataSource(existingEntity, "PpoByTransfer not found!");
                }
                else
                {
                    _pensionDbContext.PpoBytransfers.Remove(existingEntity);
                    if (await _pensionDbContext.SaveChangesAsync() == 0)
                    {
                        responseDTO.FillDataSource(existingEntity, "Failed to Delete record!");
                    }
                }
            }
            catch (Exception ex)
            {
                responseDTO.FillDataSource(
                    responseDTO,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }

        public async Task<PpoBytransfer?> GetExistingPpoByTransferAsync(
            PpoBytransfer ppoByTransferEntity
        )
        {
            return await _pensionDbContext.PpoBytransfers.FirstOrDefaultAsync(entity =>
                entity.ActiveFlag
                && entity.PensionerId == ppoByTransferEntity.PensionerId
                && entity.BytransferHeadId == ppoByTransferEntity.BytransferHeadId
                && entity.FromDate == ppoByTransferEntity.FromDate
                && entity.ToDate == ppoByTransferEntity.ToDate
            );
        }

        public async Task<PpoBytransfer?> ValidatePpoByTransferOverlapAsync(
            PpoBytransfer ppoByTransferEntity,
            int ppoId
        )
        {
            return await _pensionDbContext.PpoBytransfers.FirstOrDefaultAsync(entity =>
                entity.ActiveFlag
                && entity.PpoId == ppoId
                && (
                    (
                        entity.FromDate <= ppoByTransferEntity.FromDate
                        && entity.ToDate >= ppoByTransferEntity.FromDate
                    )
                    || (
                        entity.FromDate <= ppoByTransferEntity.ToDate
                        && entity.ToDate >= ppoByTransferEntity.ToDate
                    )
                    || (
                        entity.FromDate >= ppoByTransferEntity.FromDate
                        && entity.ToDate <= ppoByTransferEntity.ToDate
                    )
                )
            );
        }
    }
}
