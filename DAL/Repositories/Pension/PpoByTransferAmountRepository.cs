using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoByTransferAmountRepository(
        PensionDbContext context,
        IMapper mapper,
        ILogger<PpoByTransferAmountRepository> logger
    ) : IPpoByTransferAmountRepository
    {
        private readonly PensionDbContext _pensionDbContext = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<PpoByTransferAmountRepository> _logger = logger;

        public async Task<PpoBytransfer?> GetPpoByTransferByIdAsync(long id)
        {
            _logger.LogInformation("Fetching PpoByTransfer with ID: {Id}", id);
            var ppoByTransfer = await _pensionDbContext
                .PpoBytransfers.Where(entity => entity.Id == id && entity.ActiveFlag)
                .FirstOrDefaultAsync();

            return ppoByTransfer;
        }

        public async Task<T> CreatePpoByTransfer<T>(PpoBytransfer ppobyTransferHeadEntity)
        {
            _logger.LogInformation(
                "Creating PpoByTransfer with details: {PpoByTransfer}",
                ppobyTransferHeadEntity
            );
            T responseDTO = _mapper.Map<T>(ppobyTransferHeadEntity);

            try
            {
                await _pensionDbContext.PpoBytransfers.AddAsync(ppobyTransferHeadEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    responseDTO.FillErrorInDataSource(
                        ppobyTransferHeadEntity,
                        "Failed to add record!"
                    );
                }

                _pensionDbContext
                    .Entry(ppobyTransferHeadEntity)
                    .Reference(entity => entity.BytransferHead)
                    .Load();

                responseDTO = _mapper.Map<T>(ppobyTransferHeadEntity);
                _logger.LogInformation(
                    "PpoByTransfer created successfully with ID: {Id}",
                    ppobyTransferHeadEntity.Id
                );
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

        public async Task<T> UpdatePpoByTransfer<T>(PpoBytransfer ppoByTransferEntity)
        {
            _logger.LogInformation("Updating PpoByTransfer with ID: {Id}", ppoByTransferEntity.Id);
            T responseDTO = _mapper.Map<T>(ppoByTransferEntity);

            try
            {
                _pensionDbContext.PpoBytransfers.Update(ppoByTransferEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    responseDTO.FillErrorInDataSource(
                        ppoByTransferEntity,
                        "Failed to Update record!"
                    );
                }

                _pensionDbContext
                    .Entry(ppoByTransferEntity)
                    .Reference(entity => entity.BytransferHead)
                    .Load();

                responseDTO = _mapper.Map<T>(ppoByTransferEntity);
                _logger.LogInformation(
                    "PpoByTransfer updated successfully with ID: {Id}",
                    ppoByTransferEntity.Id
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating PpoByTransfer with ID: {Id}",
                    ppoByTransferEntity.Id
                );
                responseDTO.FillErrorInDataSource(
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
            _logger.LogInformation(
                "Fetching all PpoByTransfer records for PPO ID: {PpoId}, Financial Year: {FinancialYear}, Treasury Code: {TreasuryCode}",
                ppoId,
                financialYear,
                treasuryCode
            );
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
            _logger.LogInformation(
                "Checking if ByTransferHead ID: {ByTransferHeadId} is used in other tables",
                byTransferHeadId
            );
            bool isUsed = await _pensionDbContext.BillBytransfers.AnyAsync(b =>
                b.BytransferHeadId == byTransferHeadId
            );

            return isUsed;
        }

        public async Task<T> RemovePpoByTransferAsync<T>(long id)
        {
            _logger.LogInformation("Removing PpoByTransfer with ID: {Id}", id);
            T responseDTO = _mapper.Map<T>(new PpoByTransferAmountResponseDTO());

            try
            {
                var existingEntity = await _pensionDbContext.PpoBytransfers.FindAsync(id);

                if (existingEntity == null)
                {
                    _logger.LogWarning("PpoByTransfer with ID: {Id} not found", id);
                    responseDTO.FillErrorInDataSource(existingEntity, "PpoByTransfer not found!");
                }
                else
                {
                    _pensionDbContext.PpoBytransfers.Remove(existingEntity);
                    if (await _pensionDbContext.SaveChangesAsync() == 0)
                    {
                        _logger.LogError("Failed to delete PpoByTransfer with ID: {Id}", id);
                        responseDTO.FillErrorInDataSource(
                            existingEntity,
                            "Failed to Delete record!"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while removing PpoByTransfer with ID: {Id}",
                    id
                );
                responseDTO.FillErrorInDataSource(
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
            _logger.LogInformation(
                "Validating existing PpoByTransfer for Pensioner ID: {PensionerId}, Bytransfer Head ID: {BytransferHeadId}, From Date: {FromDate}, To Date: {ToDate}",
                ppoByTransferEntity.PensionerId,
                ppoByTransferEntity.BytransferHeadId,
                ppoByTransferEntity.FromDate,
                ppoByTransferEntity.ToDate
            );

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
            _logger.LogInformation(
                "Validating PpoByTransfer overlap for PPO ID: {PpoId}, From Date: {FromDate}, To Date: {ToDate}",
                ppoId,
                ppoByTransferEntity.FromDate,
                ppoByTransferEntity.ToDate
            );
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
