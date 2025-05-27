using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class ByTransferHeadService(
        IClaimService claimService,
        IMapper mapper,
        IByTransferHeadRepository byTransferHeadRepository,
        ILogger<ByTransferHeadService> logger
    ) : BaseService(claimService), IByTransferHeadService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IByTransferHeadRepository _byTransferHeadRepository =
            byTransferHeadRepository;
        private readonly ILogger<ByTransferHeadService> _logger = logger;

        public async Task<T> SaveByTransferHead<T>(ByTransferHeadEntryDTO byTransferHeadEntryDTO)
        {
            _logger.LogInformation(
                "Received request to save ByTransferHead with data: {Data}",
                byTransferHeadEntryDTO
            );
            BytransferHead byTransferHeadEntity = _mapper.Map<BytransferHead>(
                byTransferHeadEntryDTO
            );
            T responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            try
            {
                var existingByTransferHead =
                    await _byTransferHeadRepository.GetExistingByTransferHeadAsync(
                        byTransferHeadEntity
                    );

                if (existingByTransferHead != null)
                {
                    responseDTO.FillErrorInDataSource(
                        byTransferHeadEntity,
                        "ByTransferHead already exists."
                    );
                    return responseDTO;
                }

                var accountHead = await _byTransferHeadRepository.GetAccountHeadAsync(
                    byTransferHeadEntity.AccountHeadId
                );
                if (accountHead is null)
                {
                    _logger.LogWarning(
                        "AccountHead not found for ID: {AccountHeadId}",
                        byTransferHeadEntity.AccountHeadId
                    );
                    responseDTO.FillErrorInDataSource(
                        byTransferHeadEntity,
                        "AccountHead not found, Please check Account Head ID."
                    );
                    return responseDTO;
                }

                byTransferHeadEntity.AgBytransfer = true;
                SetCreatedBy(byTransferHeadEntity);

                responseDTO = await _byTransferHeadRepository.SaveByTransferHead<T>(
                    byTransferHeadEntity
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while saving ByTransferHead with data: {Data}",
                    byTransferHeadEntryDTO
                );
                responseDTO.FillErrorInDataSource(
                    byTransferHeadEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return responseDTO;
        }

        public async Task<T> UpdateByTransferHead<T>(long id, ByTransferHeadUpdateDTO updateDTO)
        {
            _logger.LogInformation(
                "Received request to update ByTransferHead with ID: {Id} and data: {Data}",
                id,
                updateDTO
            );
            BytransferHead? existingEntity = _mapper.Map<BytransferHead>(updateDTO);
            T? response = _mapper.Map<T>(existingEntity);

            try
            {
                AccountHead? accountHead = await _byTransferHeadRepository.GetAccountHeadAsync(
                    existingEntity.AccountHeadId
                );
                if (accountHead is null)
                {
                    response.FillErrorInDataSource(existingEntity, "Account Head not found.");
                    return response;
                }

                existingEntity = await _byTransferHeadRepository.GetByTransferHeadMapById(id);

                if (existingEntity is null)
                {
                    _logger.LogWarning("ByTransferHead not found for ID: {Id}", id);
                    response.FillErrorInDataSource(existingEntity, "ByTransferHead not found.");
                    return response;
                }

                bool isUsed = await _byTransferHeadRepository.IsUsedInOtherTables(id);
                if (isUsed)
                {
                    _logger.LogWarning("ByTransferHead with ID: {Id} is already in use.", id);
                    response.FillErrorInDataSource(
                        existingEntity,
                        "Updation failed: ByTransferHead is in used."
                    );
                    return response;
                }

                existingEntity.Id = id;
                existingEntity.AgBytransfer = true;
                existingEntity.FillFrom(updateDTO);
                SetUpdatedBy(existingEntity);

                response = await _byTransferHeadRepository.UpdateByTransferHead<T>(existingEntity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while updating ByTransferHead with ID: {Id}",
                    id
                );
                response.FillErrorInDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Service exception occurred while updating ByTransferHead with ID: {Id}",
                    id
                );
                response.FillErrorInDataSource(
                    existingEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            return response;
        }

        public async Task<T> DeleteByTransferHeadMapById<T>(long id)
        {
            _logger.LogInformation("Received request to delete ByTransferHead with ID: {Id}", id);
            T? response = _mapper.Map<T>(new ByTransferHeadResponseDTO());
            BytransferHead? existingEntity = new();

            try
            {
                existingEntity = await _byTransferHeadRepository.GetByTransferHeadMapById(id);

                if (existingEntity is null)
                {
                    _logger.LogWarning("ByTransferHead not found for ID: {Id}", id);
                    response.FillErrorInDataSource(existingEntity, "ByTransferHead not found.");
                    return response;
                }

                bool isUsed = await _byTransferHeadRepository.IsUsedInOtherTables(id);
                if (isUsed)
                {
                    _logger.LogWarning("ByTransferHead with ID: {Id} is already in use.", id);
                    response.FillErrorInDataSource(
                        existingEntity,
                        "Deletion failed: ByTransferHead is already in used."
                    );
                }

                response = await _byTransferHeadRepository.RemoveByTransferHeadAsync<T>(id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while deleting ByTransferHead with ID: {Id}",
                    id
                );
                response.FillErrorInDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Service exception occurred while deleting ByTransferHead with ID: {Id}",
                    id
                );
                response.FillErrorInDataSource(
                    existingEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return response;
        }

        public async Task<T> GetByTransferHeadMapById<T>(long Id)
        {
            _logger.LogInformation("Received request to get ByTransferHead with ID: {Id}", Id);
            BytransferHead bytransferHeadEntity = new() { Id = 0 };
            T response = _mapper.Map<T>(bytransferHeadEntity);

            try
            {
                BytransferHead? byTransferHead =
                    await _byTransferHeadRepository.GetByTransferHeadMapById(Id);

                if (byTransferHead == null)
                {
                    response.FillErrorInDataSource(
                        byTransferHead,
                        "ByTransferHead not found, Please check the ID and try again."
                    );
                    return response;
                }

                response = _mapper.Map<T>(byTransferHead);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while getting ByTransferHead with ID: {Id}",
                    Id
                );
                response.FillErrorInDataSource(
                    bytransferHeadEntity,
                    $"DbException : {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Service exception occurred while getting ByTransferHead with ID: {Id}",
                    Id
                );
                response.FillErrorInDataSource(
                    bytransferHeadEntity,
                    $"ServiceException : {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            return response;
        }

        public async Task<T> GetByTransferHeadMaps<T>()
        {
            _logger.LogInformation("Received request to get all ByTransferHeads.");
            TableResponseDTO<ByTransferHeadResponseDTO> tableResponse = new();

            try
            {
                var byTransferHeads = await _byTransferHeadRepository.GetAllByTransferHeadsAsync();

                tableResponse.Data = _mapper.Map<List<ByTransferHeadResponseDTO>>(byTransferHeads);

                if (tableResponse.Data.Count == 0)
                {
                    tableResponse.FillErrorInDataSource(
                        tableResponse.Data,
                        "No active ByTransferHead records found."
                    );
                    return _mapper.Map<T>(tableResponse);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while getting all ByTransferHeads."
                );
                tableResponse.FillErrorInDataSource(
                    tableResponse.Data,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Service exception occurred while getting all ByTransferHeads."
                );
                tableResponse.FillErrorInDataSource(
                    tableResponse.Data,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return _mapper.Map<T>(tableResponse);
        }
    }
}
