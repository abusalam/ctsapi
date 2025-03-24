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
    public class PpoComponentRevisionService(
        IPpoComponentRevisionRepository ppoComponentRevisionRepository,
        IPensionerDetailsRepository pensionerDetailsRepository,
        IMapper mapper,
        PensionDbContext pensionDbContext,
        IClaimService claimService
    ) : BaseService(claimService), IPpoComponentRevisionService
    {
        private readonly IPpoComponentRevisionRepository _ppoComponentRevisionRepository =
            ppoComponentRevisionRepository;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository =
            pensionerDetailsRepository;
        private readonly IMapper _mapper = mapper;
        private readonly PensionDbContext _pensionDbContext = pensionDbContext;

        public async Task<T> GetPposForComponentRevisions<T>(
            short financialYear,
            string treasuryCode
        )
        {
            TableResponseDTO<PpoComponentRevisionPpoListItemDTO> tableResponse = new();
            try
            {
                tableResponse.Data = await _ppoComponentRevisionRepository.GetAllPpos(
                    entity => _mapper.Map<PpoComponentRevisionPpoListItemDTO>(entity),
                    financialYear,
                    treasuryCode
                );

                if (tableResponse.Data.Count == 0)
                {
                    tableResponse.FillErrorInDataSource(
                        tableResponse.Data,
                        "No PPO Revisions found"
                    );
                    return _mapper.Map<T>(tableResponse);
                }

                tableResponse.Data.ForEach(item =>
                {
                    item.BankBranchName =
                        item.Branch?.Bank?.BankName + "-" + item.Branch?.BranchName;
                    item.CategoryDescription = item.Category?.CategoryName ?? "";
                    item.Branch = null;
                    item.Category = null;
                });
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

        public async Task<T> CreateSinglePpoComponentRevision<T>(
            int ppoId,
            PpoComponentRevisionEntryDTO ppoComponentRevisionDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoComponentRevision ppoComponentRevision = _mapper.Map<PpoComponentRevision>(
                ppoComponentRevisionDTO
            );
            ppoComponentRevision.PpoId = ppoId;

            T response = _mapper.Map<T>(ppoComponentRevision);

            try
            {
                ppoComponentRevision.FillFrom(ppoComponentRevisionDTO);
                // var today = DateOnly.FromDateTime(DateTime.UtcNow);

                // if (ppoComponentRevision.FromDate < today)
                // {
                //     response.FillErrorInDataSource(ppoComponentRevision, "Old date not allowed.");
                //     return response;
                // }

                var rateExists = await _ppoComponentRevisionRepository.CheckRateExists(
                    ppoComponentRevision.RateId,
                    financialYear,
                    treasuryCode
                );

                if (!rateExists)
                {
                    response.FillErrorInDataSource(ppoComponentRevision, "Rate not found.");
                    return response;
                }

                var pensionerFound =
                    await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                        ppoId,
                        financialYear,
                        treasuryCode,
                        entity => _mapper.Map<Pensioner>(entity)
                    );

                if (pensionerFound is null)
                {
                    response.FillErrorInDataSource(ppoComponentRevisionDTO, "Pensioner not found.");
                    return response;
                }

                var existingRevisions =
                    await _ppoComponentRevisionRepository.GetRevisionsByPpoIdAndRateId(
                        ppoId,
                        ppoComponentRevision.RateId
                    );

                if (existingRevisions.Any())
                {
                    var latestEntry = existingRevisions.First();

                    if (ppoComponentRevision.FromDate <= latestEntry.FromDate)
                    {
                        response.FillErrorInDataSource(
                            ppoComponentRevision,
                            "Date overlap detected."
                        );
                        return response;
                    }

                    if (
                        latestEntry.ToDate.HasValue
                        && ppoComponentRevision.FromDate != latestEntry.ToDate.Value.AddDays(1)
                    )
                    {
                        response.FillErrorInDataSource(
                            ppoComponentRevision,
                            $"Date gap detected, New FromDate must be {latestEntry.ToDate.Value.AddDays(1)}."
                        );
                        return response;
                    }

                    latestEntry.ToDate = ppoComponentRevision.FromDate.AddDays(-1);
                    await _ppoComponentRevisionRepository.UpdatePpoComponentRevision<T>(
                        latestEntry,
                        financialYear,
                        treasuryCode
                    );
                }

                SetCreatedBy(ppoComponentRevision);
                ppoComponentRevision.PensionerId = pensionerFound.Id;

                response =
                    await _ppoComponentRevisionRepository.CreateSinglePpoComponentRevision<T>(
                        ppoComponentRevision
                    );
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    ppoComponentRevision,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return response;
        }

        public async Task<List<TResponse>> CreatePpoComponentRevisions<TEntry, TResponse>(
            int ppoId,
            List<TEntry> ppoComponentRevisionDTOs,
            short financialYear,
            string treasuryCode
        )
        {
            List<PpoComponentRevision> ppoComponentRevisions = new();
            List<TResponse>? response = new List<TResponse>();

            try
            {
                foreach (TEntry ppoComponentRevisionDTO in ppoComponentRevisionDTOs)
                {
                    PpoComponentRevision ppoComponentRevision = new() { Id = 0, PpoId = ppoId };
                    ppoComponentRevision.FillFrom(ppoComponentRevisionDTO);

                    PpoComponentRevision? ppoComponentRevisionFound =
                        await _pensionDbContext.PpoComponentRevisions.FirstOrDefaultAsync(entity =>
                            entity.ActiveFlag
                            && entity.PpoId == ppoId
                            && entity.RateId == ppoComponentRevision.RateId
                            && entity.FromDate == ppoComponentRevision.FromDate
                        );

                    if (ppoComponentRevisionFound != null)
                    {
                        ppoComponentRevision = ppoComponentRevisionFound;
                        ppoComponentRevisionDTO.FillErrorInDataSource(
                            ppoComponentRevisionFound,
                            $"PPO Component Revision already exists!"
                        );
                        continue;
                    }
                    Pensioner? pensionerFound =
                        await _pensionDbContext.Pensioners.FirstOrDefaultAsync(entity =>
                            entity.ActiveFlag
                            && entity.TreasuryCode == treasuryCode
                            && entity.PpoId == ppoId
                        );
                    if (pensionerFound is null)
                    {
                        ppoComponentRevisionDTO.FillErrorInDataSource(
                            ppoComponentRevision,
                            $"Pensioner not found!"
                        );
                        continue;
                    }
                    SetCreatedBy(ppoComponentRevision);
                    ppoComponentRevision.PensionerId = pensionerFound.Id;
                    ppoComponentRevisions.Add(ppoComponentRevision);
                }
                await _pensionDbContext.PpoComponentRevisions.AddRangeAsync(ppoComponentRevisions);
                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    response.FillErrorInDataSource(
                        ppoComponentRevisions,
                        $"PPO Component Rate not saved!"
                    );
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    ppoComponentRevisions,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return response;
        }

        public async Task<T> UpdatePpoComponentRevisionById<T>(
            long revisionId,
            PpoComponentRevisionUpdateDTO ppoComponentRevisionUpdateDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoComponentRevision? ppoComponentRevision = new();
            T? response = _mapper.Map<T>(ppoComponentRevision);

            try
            {
                ppoComponentRevision =
                    await _ppoComponentRevisionRepository.GetPpoComponentRevisionById(
                        revisionId,
                        financialYear,
                        treasuryCode
                    );

                if (ppoComponentRevision == null)
                {
                    response.FillErrorInDataSource(
                        ppoComponentRevision,
                        $"PPO Component RevisionId({revisionId}) not found!"
                    );
                    return response;
                }

                ppoComponentRevision.FillFrom(ppoComponentRevisionUpdateDTO);
                SetUpdatedBy(ppoComponentRevision);

                return await _ppoComponentRevisionRepository.UpdatePpoComponentRevision<T>(
                    ppoComponentRevision,
                    financialYear,
                    treasuryCode
                );
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    ppoComponentRevision,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return response;
        }

        public async Task<List<T>> GetPpoComponentRevisionsByPpoId<T>(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            var revisions = await _ppoComponentRevisionRepository.GetAllRevisionsByPpoIdAsync(
                ppoId,
                entity => _mapper.Map<T>(entity),
                financialYear,
                treasuryCode
            );
            return revisions;
        }

        public async Task<T> DeletePpoComponentRevisionById<T>(
            long revisionId,
            short financialYear,
            string treasuryCode
        )
        {
            PpoComponentRevision? ppoComponentRevision = new();
            T? response = _mapper.Map<T>(ppoComponentRevision);

            try
            {
                ppoComponentRevision =
                    await _ppoComponentRevisionRepository.GetPpoComponentRevisionById(
                        revisionId,
                        financialYear,
                        treasuryCode
                    );

                if (ppoComponentRevision == null)
                {
                    response.FillErrorInDataSource(
                        ppoComponentRevision,
                        $"PPO Component RevisionId({revisionId}) not found!"
                    );
                    return response;
                }

                ppoComponentRevision.ActiveFlag = false;
                SetUpdatedBy(ppoComponentRevision);

                return await _ppoComponentRevisionRepository.DeletePpoComponentRevisionById<T>(
                    ppoComponentRevision,
                    financialYear,
                    treasuryCode
                );
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    ppoComponentRevision,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return response;
        }
    }
}
