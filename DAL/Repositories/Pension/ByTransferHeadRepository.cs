using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ByTransferHeadRepository(PensionDbContext context, IMapper mapper)
        : IByTransferHeadRepository
    {
        private readonly PensionDbContext _pensionDbContext = context;
        private readonly IMapper _mapper = mapper;

        public async Task<AccountHead?> GetAccountHeadAsync(long id)
        {
            return await _pensionDbContext
                .AccountHeads.Where(ah => ah.Id == id && ah.ActiveFlag)
                .FirstOrDefaultAsync();
        }

        public async Task<BytransferHead?> GetByTransferHeadMapById(long id)
        {
            return await _pensionDbContext
                .BytransferHeads.Where(entity => entity.Id == id && entity.ActiveFlag)
                .Include(entity => entity.AccountHead)
                .FirstOrDefaultAsync();
        }

        public async Task<T> SaveByTransferHead<T>(BytransferHead byTransferHeadEntity)
        {
            T responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            try
            {
                await _pensionDbContext.BytransferHeads.AddAsync(byTransferHeadEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    responseDTO.FillErrorInDataSource(
                        byTransferHeadEntity,
                        "Failed to save record!"
                    );
                    return responseDTO;
                }

                _pensionDbContext
                    .Entry(byTransferHeadEntity)
                    .Reference(entity => entity.AccountHead)
                    .Load();

                responseDTO = _mapper.Map<T>(byTransferHeadEntity);
            }
            catch (Exception ex)
            {
                responseDTO.FillErrorInDataSource(
                    byTransferHeadEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return responseDTO;
            }

            return responseDTO;
        }

        public async Task<T> UpdateByTransferHead<T>(BytransferHead byTransferHeadEntity)
        {
            T responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            try
            {
                _pensionDbContext.BytransferHeads.Update(byTransferHeadEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    responseDTO.FillErrorInDataSource(
                        byTransferHeadEntity,
                        "Failed to update record!"
                    );
                    return responseDTO;
                }

                _pensionDbContext
                    .Entry(byTransferHeadEntity)
                    .Reference(entity => entity.AccountHead)
                    .Load();

                responseDTO = _mapper.Map<T>(byTransferHeadEntity);
            }
            catch (Exception ex)
            {
                responseDTO.FillErrorInDataSource(
                    byTransferHeadEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return responseDTO;
            }

            return responseDTO;
        }

        public async Task<List<BytransferHead>> GetAllByTransferHeadsAsync()
        {
            return await _pensionDbContext
                .BytransferHeads.Where(entity => entity.ActiveFlag)
                .Include(entity => entity.AccountHead)
                .ToListAsync();
        }

        public async Task<bool> IsUsedInOtherTables(long byTransferHeadId)
        {
            bool isUsed =
                await _pensionDbContext.BillBytransfers.AnyAsync(b =>
                    b.BytransferHeadId == byTransferHeadId
                )
                || await _pensionDbContext.PpoBytransfers.AnyAsync(p =>
                    p.BytransferHeadId == byTransferHeadId
                );

            return isUsed;
        }

        public async Task<T> RemoveByTransferHeadAsync<T>(long id)
        {
            T responseDTO = _mapper.Map<T>(new ByTransferHeadResponseDTO());

            try
            {
                var existingEntity = await _pensionDbContext.BytransferHeads.FindAsync(id);

                if (existingEntity == null)
                {
                    responseDTO.FillErrorInDataSource(existingEntity, "BytransferHead not found!");
                }
                else
                {
                    _pensionDbContext.BytransferHeads.Remove(existingEntity);

                    if (await _pensionDbContext.SaveChangesAsync() == 0)
                    {
                        responseDTO.FillErrorInDataSource(
                            existingEntity,
                            "Failed to delete record!"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                responseDTO.FillErrorInDataSource(
                    responseDTO,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }

        public async Task<BytransferHead?> GetExistingByTransferHeadAsync(
            BytransferHead byTransferHeadEntity
        )
        {
            return await _pensionDbContext.BytransferHeads.FirstOrDefaultAsync(entity =>
                entity.ActiveFlag
                && entity.AccountHeadId == byTransferHeadEntity.AccountHeadId
                && entity.BytransferDescription == byTransferHeadEntity.BytransferDescription
                && entity.BytransferType == byTransferHeadEntity.BytransferType
            );
        }
    }
}
