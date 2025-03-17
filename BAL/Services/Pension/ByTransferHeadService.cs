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
        IByTransferHeadRepository byTransferHeadRepository
    ) : BaseService(claimService), IByTransferHeadService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IByTransferHeadRepository _byTransferHeadRepository =
            byTransferHeadRepository;

        public async Task<T> SaveByTransferHead<T>(ByTransferHeadEntryDTO byTransferHeadEntryDTO)
        {
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
                responseDTO.FillErrorInDataSource(
                    byTransferHeadEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return responseDTO;
        }

        public async Task<T> UpdateByTransferHead<T>(long id, ByTransferHeadUpdateDTO updateDTO)
        {
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
                    response.FillErrorInDataSource(existingEntity, "ByTransferHead not found.");
                    return response;
                }

                bool isUsed = await _byTransferHeadRepository.IsUsedInOtherTables(id);
                if (isUsed)
                {
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
                response.FillErrorInDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
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
            T? response = _mapper.Map<T>(new ByTransferHeadResponseDTO());
            BytransferHead? existingEntity = new();

            try
            {
                existingEntity = await _byTransferHeadRepository.GetByTransferHeadMapById(id);

                if (existingEntity is null)
                {
                    response.FillErrorInDataSource(existingEntity, "ByTransferHead not found.");
                    return response;
                }

                bool isUsed = await _byTransferHeadRepository.IsUsedInOtherTables(id);
                if (isUsed)
                {
                    response.FillErrorInDataSource(
                        existingEntity,
                        "Deletion failed: ByTransferHead is already in used."
                    );
                }

                response = await _byTransferHeadRepository.RemoveByTransferHeadAsync<T>(id);
            }
            catch (DbUpdateException ex)
            {
                response.FillErrorInDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    existingEntity,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return response;
        }

        public async Task<T> GetByTransferHeadMapById<T>(long Id)
        {
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
                response.FillErrorInDataSource(
                    bytransferHeadEntity,
                    $"DbException : {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
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
                tableResponse.FillErrorInDataSource(
                    tableResponse.Data,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            catch (Exception ex)
            {
                tableResponse.FillErrorInDataSource(
                    tableResponse.Data,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return _mapper.Map<T>(tableResponse);
        }
    }
}
