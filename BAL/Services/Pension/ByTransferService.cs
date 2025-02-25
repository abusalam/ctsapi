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
    public class ByTransferService : BaseService, IByTransferService
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;
        private readonly IByTransferRepository _byTransferHeadRepository;
        private readonly ITreasuryRepository _treasuryRepository;

        public ByTransferService(
            PensionDbContext pensionDbContext,
            IClaimService claimService,
            IMapper mapper,
            IByTransferRepository byTransferHeadRepository,
            ITreasuryRepository treasuryRepository
        )
            : base(claimService)
        {
            _pensionDbContext = pensionDbContext;
            _mapper = mapper;
            _byTransferHeadRepository = byTransferHeadRepository;
            _treasuryRepository = treasuryRepository;
        }

        public async Task<T> SaveByTransferHead<T>(ByTransferHeadEntryDTO byTransferHeadEntryDTO)
        {
            BytransferHead byTransferHeadEntity = _mapper.Map<BytransferHead>(
                byTransferHeadEntryDTO
            );
            T responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            try
            {
                AccountHead? accountHead = await _pensionDbContext
                    .AccountHeads.Where(entity =>
                        entity.Id == byTransferHeadEntity.AccountHeadId && entity.ActiveFlag
                    )
                    .FirstOrDefaultAsync();

                if (accountHead == null)
                {
                    responseDTO.FillDataSource(
                        byTransferHeadEntity,
                        "Account head not found! Please check Account Head ID."
                    );
                    return responseDTO;
                }

                BytransferHead? existingByTransferHead = await _pensionDbContext
                    .BytransferHeads.Where(entity =>
                        entity.ActiveFlag
                        && entity.AccountHeadId == byTransferHeadEntity.AccountHeadId
                        && entity.BytransferType == byTransferHeadEntity.BytransferType
                        && entity.BytransferDescription
                            == byTransferHeadEntity.BytransferDescription
                    )
                    .FirstOrDefaultAsync();

                if (existingByTransferHead != null)
                {
                    responseDTO.FillDataSource(
                        existingByTransferHead,
                        "ByTransferHead already exists! Please check the description and type."
                    );
                    return responseDTO;
                }

                SetCreatedBy(byTransferHeadEntity);
                byTransferHeadEntity.UpdatedAt = DateTime.Now;

                responseDTO = await _byTransferHeadRepository.SaveByTransferHead<T>(
                    byTransferHeadEntity
                );
            }
            catch (Exception ex)
            {
                responseDTO.FillDataSource(
                    _mapper.Map<T>(byTransferHeadEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }

        public async Task<T> UpdateByTransferHead<T>(ByTransferHeadUpdateDTO updateDTO)
        {
            try
            {
                ByTransferHeadResponseDTO responseDTO = new();

                var existingEntity = await _byTransferHeadRepository.GetByTransferHeadByIdAsync(
                    updateDTO.Id
                );

                if (existingEntity == null || !existingEntity.ActiveFlag)
                {
                    responseDTO.Message = "ByTransferHead not found or already deleted.";
                    return _mapper.Map<T>(responseDTO);
                }

                bool isUsed = await _byTransferHeadRepository.IsUsedInOtherTables(updateDTO.Id);
                if (isUsed)
                {
                    responseDTO.Message =
                        "Cannot update, as this ByTransferHead is used in other tables.";
                    return _mapper.Map<T>(responseDTO);
                }

                existingEntity.BytransferType = updateDTO.BytransferType;
                existingEntity.AccountHeadId = updateDTO.AccountHeadId;
                existingEntity.BytransferDescription = updateDTO.BytransferDescription;
                existingEntity.AgBytransfer = updateDTO.AgBytransfer;
                existingEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime();

                return await _byTransferHeadRepository.UpdateByTransferHead<T>(existingEntity);
            }
            catch (Exception ex)
            {
                ByTransferHeadResponseDTO errorResponse = new()
                {
                    Message = $"ServiceException: {ex.InnerException?.Message ?? ex.Message}",
                };

                return _mapper.Map<T>(errorResponse);
            }
        }

        //delete


        public async Task<T> DeleteByTransferHead<T>(long id)
        {
            try
            {
                ByTransferHeadResponseDTO responseDTO = new();

                var existingEntity = await _byTransferHeadRepository.GetByTransferHeadByIdAsync(id);

                if (existingEntity == null || !existingEntity.ActiveFlag)
                {
                    responseDTO.Message = "ByTransferHead not found or already deleted.";
                    return _mapper.Map<T>(responseDTO);
                }
                //to check if bytransferhead is used or not

                bool isUsed = await _byTransferHeadRepository.IsUsedInOtherTables(id);
                if (isUsed)
                {
                    responseDTO.Message =
                        "Cannot delete, as this ByTransferHead is used in other tables.";
                    return _mapper.Map<T>(responseDTO);
                }

                existingEntity.ActiveFlag = false;
                existingEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime();

                return await _byTransferHeadRepository.UpdateByTransferHead<T>(existingEntity);
            }
            catch (Exception ex)
            {
                ByTransferHeadResponseDTO errorResponse = new()
                {
                    Message = $"ServiceException: {ex.InnerException?.Message ?? ex.Message}",
                };

                return _mapper.Map<T>(errorResponse);
            }
        }

        public async Task<T> GetByTransferHeadById<T>(long byTransferHeadId)
        {
            T? response = _mapper.Map<T>(new BytransferHead());

            try
            {
                bool isByTransferHeadIdExists = await _pensionDbContext.BytransferHeads.AnyAsync(
                    entity => entity.Id == byTransferHeadId && entity.ActiveFlag
                );

                if (!isByTransferHeadIdExists)
                {
                    response.FillDataSource(
                        new BytransferHead(),
                        "ByTransferHead ID does not exist or is inactive. Please check the ID and try again."
                    );
                    return response;
                }

                var byTransferHead = await _byTransferHeadRepository.GetByTransferHeadByIdAsync(
                    byTransferHeadId
                );

                if (byTransferHead == null)
                {
                    response.FillDataSource(
                        new BytransferHead(),
                        "ByTransferHead not found! Please check the ID and try again."
                    );
                    return response;
                }

                var accountHead = await _pensionDbContext
                    .AccountHeads.Where(entity =>
                        entity.Id == byTransferHead.AccountHeadId && entity.ActiveFlag
                    )
                    .FirstOrDefaultAsync();

                if (accountHead == null)
                {
                    response.FillDataSource(
                        new BytransferHead(),
                        "AccountHead not found or inactive! Please check the AccountHead ID."
                    );
                    return response;
                }

                response = _mapper.Map<T>(byTransferHead);
                return response;
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    new BytransferHead(),
                    $"Database exception occurred: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    new BytransferHead(),
                    $"An unexpected error occurred: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<List<T>> GetAllByTransferHeads<T>()
        {
            List<T> responseList = new List<T>();

            try
            {
                var byTransferHeads = await _byTransferHeadRepository.GetAllByTransferHeadsAsync();

                if (byTransferHeads == null || !byTransferHeads.Any())
                {
                    var emptyResponse = _mapper.Map<T>(new BytransferHead());
                    emptyResponse.FillDataSource(
                        new BytransferHead(),
                        "No active ByTransferHead records found."
                    );
                    responseList.Add(emptyResponse);
                    return responseList;
                }

                foreach (var byTransferHead in byTransferHeads)
                {
                    var responseDTO = _mapper.Map<T>(byTransferHead);
                    responseList.Add(responseDTO);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = _mapper.Map<T>(new BytransferHead());
                errorResponse.FillDataSource(
                    new BytransferHead(),
                    $"An unexpected error occurred: {ex.InnerException?.Message ?? ex.Message}"
                );
                responseList.Clear();
                responseList.Add(errorResponse);
            }

            return responseList;
        }
    }
}
