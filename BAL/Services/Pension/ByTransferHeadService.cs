using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
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
                    responseDTO.FillDataSource(
                        byTransferHeadEntity,
                        "ByTransferHead already exists"
                    );
                    return responseDTO;
                }

                var accountHead = await _byTransferHeadRepository.GetAccountHeadAsync(
                    byTransferHeadEntity.AccountHeadId
                );
                if (accountHead is null)
                {
                    responseDTO.FillDataSource(
                        byTransferHeadEntity,
                        "AccountHead not found! Please check Account Head ID."
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
                responseDTO.FillDataSource(
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
                    response.FillDataSource(existingEntity, "Account head not found!");
                    return response;
                }

                existingEntity = await _byTransferHeadRepository.GetByTransferHeadMapById(id);

                if (existingEntity is null)
                {
                    response.FillDataSource(existingEntity, "ByTransferHead not found.");
                    return response;
                }

                bool isUsed = await _byTransferHeadRepository.IsUsedInOtherTables(id);
                if (isUsed)
                {
                    response.FillDataSource(
                        existingEntity,
                        "Cannot update,ByTransferHead is used ."
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
                response.FillDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
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
                    response.FillDataSource(existingEntity, "ByTransferHead not found");
                    return response;
                }

                bool isUsed = await _byTransferHeadRepository.IsUsedInOtherTables(id);
                if (isUsed)
                {
                    response.FillDataSource(
                        existingEntity,
                        "ByTransferHead is already in used, cannot delete."
                    );
                }

                response = await _byTransferHeadRepository.RemoveByTransferHeadAsync<T>(id);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    existingEntity,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            catch (Exception ex)
            {
                response.FillDataSource(
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
                    response.FillDataSource(
                        byTransferHead,
                        "ByTransferHead not found! Please check the ID and try again."
                    );
                    return response;
                }

                response = _mapper.Map<T>(byTransferHead);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    bytransferHeadEntity,
                    $"DbException : {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    bytransferHeadEntity,
                    $"ServiceException : {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            return response;
        }

        public async Task<List<T>> GetByTransferHeadMaps<T>()
        {
            List<T> responseList = new();
            T response = _mapper.Map<T>(new BytransferHead());

            try
            {
                var byTransferHeads = await _byTransferHeadRepository.GetAllByTransferHeadsAsync();

                if (byTransferHeads == null)
                {
                    response.FillDataSource(response, "No active ByTransferHead records found.");

                    return new List<T> { response };
                }

                responseList = _mapper.Map<List<T>>(byTransferHeads);
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    response,
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return new List<T> { response };
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    response,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return new List<T> { response };
            }

            return responseList;
        }
    }
}
