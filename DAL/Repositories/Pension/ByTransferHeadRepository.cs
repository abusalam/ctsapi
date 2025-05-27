using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ByTransferHeadRepository(
        PensionDbContext context,
        IMapper mapper,
        ILogger<ByTransferHeadRepository> logger
    ) : IByTransferHeadRepository
    {
        private readonly PensionDbContext _pensionDbContext = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ByTransferHeadRepository> _logger = logger;

        public async Task<AccountHead?> GetAccountHeadAsync(long id)
        {
            _logger.LogInformation("Fetching AccountHead with ID: {Id}", id);
            return await _pensionDbContext
                .AccountHeads.Where(ah => ah.Id == id && ah.ActiveFlag)
                .FirstOrDefaultAsync();
        }

        public async Task<BytransferHead?> GetByTransferHeadMapById(long id)
        {
            _logger.LogInformation("Fetching BytransferHead with ID: {Id}", id);
            return await _pensionDbContext
                .BytransferHeads.Where(entity => entity.Id == id && entity.ActiveFlag)
                .Include(entity => entity.AccountHead)
                .FirstOrDefaultAsync();
        }

        public async Task<T> SaveByTransferHead<T>(BytransferHead byTransferHeadEntity)
        {
            _logger.LogInformation(
                "Attempting to save BytransferHead with data: {Data}",
                byTransferHeadEntity
            );

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
                _logger.LogError(
                    ex,
                    "Error occurred while saving BytransferHead with data: {Data}",
                    byTransferHeadEntity
                );
                responseDTO.FillErrorInDataSource(
                    byTransferHeadEntity,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return responseDTO;
            }
            _logger.LogInformation(
                "BytransferHead with ID: {Id} saved successfully.",
                byTransferHeadEntity.Id
            );

            return responseDTO;
        }

        public async Task<T> UpdateByTransferHead<T>(BytransferHead byTransferHeadEntity)
        {
            _logger.LogInformation(
                "Attempting to update BytransferHead with data: {Data}",
                byTransferHeadEntity
            );
            T responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            try
            {
                _pensionDbContext.BytransferHeads.Update(byTransferHeadEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    _logger.LogWarning(
                        "Failed to update BytransferHead with ID: {Id}",
                        byTransferHeadEntity.Id
                    );
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
                _logger.LogInformation(
                    "BytransferHead with ID: {Id} updated successfully.",
                    byTransferHeadEntity.Id
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating BytransferHead with data: {Data}",
                    byTransferHeadEntity
                );
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
            _logger.LogInformation("Fetching all active BytransferHeads from the database.");
            return await _pensionDbContext
                .BytransferHeads.Where(entity => entity.ActiveFlag)
                .Include(entity => entity.AccountHead)
                .ToListAsync();
        }

        public async Task<bool> IsUsedInOtherTables(long byTransferHeadId)
        {
            _logger.LogInformation(
                "Checking if BytransferHead with ID: {ByTransferHeadId} is used in other tables.",
                byTransferHeadId
            );
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
            _logger.LogInformation("Attempting to delete BytransferHead with ID: {Id}", id);
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
                        _logger.LogWarning("Failed to delete BytransferHead with ID: {Id}", id);
                        responseDTO.FillErrorInDataSource(
                            existingEntity,
                            "Failed to delete record!"
                        );
                    }
                }
                _logger.LogInformation("BytransferHead with ID: {Id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while deleting BytransferHead with ID: {Id}",
                    id
                );
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
            _logger.LogInformation(
                "Checking for existing BytransferHead with AccountHeadId: {AccountHeadId}, BytransferDescription: {BytransferDescription}, BytransferType: {BytransferType}",
                byTransferHeadEntity.AccountHeadId,
                byTransferHeadEntity.BytransferDescription,
                byTransferHeadEntity.BytransferType
            );
            return await _pensionDbContext.BytransferHeads.FirstOrDefaultAsync(entity =>
                entity.ActiveFlag
                && entity.AccountHeadId == byTransferHeadEntity.AccountHeadId
                && entity.BytransferDescription == byTransferHeadEntity.BytransferDescription
                && entity.BytransferType == byTransferHeadEntity.BytransferType
            );
        }
    }
}
