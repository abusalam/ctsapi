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
        IClaimService claimService,
        ILogger<PpoComponentRevisionService> logger
    ) : BaseService(claimService), IPpoComponentRevisionService
    {
        private readonly IPpoComponentRevisionRepository _ppoComponentRevisionRepository =
            ppoComponentRevisionRepository;
        private readonly IPensionerDetailsRepository _pensionerDetailsRepository =
            pensionerDetailsRepository;
        private readonly IMapper _mapper = mapper;
        private readonly PensionDbContext _pensionDbContext = pensionDbContext;
        private readonly ILogger<PpoComponentRevisionService> _logger = logger;

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
                    _logger.LogWarning(
                        "No PPO Revisions found for financial year: {FinancialYear} and treasury code: {TreasuryCode}",
                        financialYear,
                        treasuryCode
                    );

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
                _logger.LogError(
                    ex,
                    "Error occurred while fetching PPOs for component revisions with financial year: {FinancialYear} and treasury code: {TreasuryCode}",
                    financialYear,
                    treasuryCode
                );
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
            T? response = _mapper.Map<T>(ppoComponentRevision);

            try
            {
                ppoComponentRevision.FillFrom(ppoComponentRevisionDTO);

                if (
                    await _ppoComponentRevisionRepository.CheckPpoComponentRevisionExists(
                        ppoComponentRevision,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(
                        ppoComponentRevision,
                        $"PPO Component Revision already exists!"
                    );
                    return response;
                }

                if (
                    !await _ppoComponentRevisionRepository.CheckRateExists(
                        ppoComponentRevision.RateId,
                        financialYear,
                        treasuryCode
                    )
                )
                {
                    response.FillErrorInDataSource(ppoComponentRevision, $"Rate not found!");
                    return response;
                }

                Pensioner? pensionerFound =
                    await _pensionerDetailsRepository.GetPensionerDetailsByPpoIdAsync(
                        ppoId,
                        financialYear,
                        treasuryCode,
                        entity => _mapper.Map<Pensioner>(entity)
                    );

                if (pensionerFound is null)
                {
                    _logger.LogWarning(
                        "Pensioner not found for PPO ID: {PpoId} with treasury code: {TreasuryCode}",
                        ppoId,
                        treasuryCode
                    );
                    response.FillErrorInDataSource(
                        ppoComponentRevisionDTO,
                        $"Pensioner not found!"
                    );
                    return response;
                }

                SetCreatedBy(ppoComponentRevision);
                // ppoComponentRevision.TreasuryCode = treasuryCode;
                ppoComponentRevision.PensionerId = pensionerFound.Id;
                await _pensionDbContext.Set<PpoComponentRevision>().AddAsync(ppoComponentRevision);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    // If no rows were affected, fill the error in the response
                    _logger.LogError(
                        "Failed to save PPO Component Revision for PPO ID: {PpoId}",
                        ppoId
                    );
                    response.FillErrorInDataSource(
                        ppoComponentRevision,
                        $"PPO Component Rate not saved!"
                    );
                    return response;
                }
                await _pensionDbContext
                    .Entry(ppoComponentRevision)
                    .Reference(entity => entity.Rate)
                    .LoadAsync();

                await _pensionDbContext
                    .Entry(ppoComponentRevision.Rate)
                    .Reference(entity => entity.Breakup)
                    .LoadAsync();

                response = _mapper.Map<T>(ppoComponentRevision);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating PPO Component Revision for PPO ID: {PpoId} with data: {Data}",
                    ppoId,
                    ppoComponentRevisionDTO
                );
                response.FillErrorInDataSource(
                    ppoComponentRevision,
                    $"DbUpdateException: {ex.InnerException?.Message}"
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
            _logger.LogInformation(
                "Creating multiple PPO Component Revisions for PPO ID: {PpoId} with data count: {Count}",
                ppoId,
                ppoComponentRevisionDTOs.Count
            );
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
                        _logger.LogWarning(
                            "PPO Component Revision already exists for PPO ID: {PpoId} with Rate ID: {RateId} and From Date: {FromDate}",
                            ppoId,
                            ppoComponentRevision.RateId,
                            ppoComponentRevision.FromDate
                        );
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
                        _logger.LogWarning(
                            "Pensioner not found for PPO ID: {PpoId} with treasury code: {TreasuryCode}",
                            ppoId,
                            treasuryCode
                        );
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
                    _logger.LogError(
                        "Failed to save PPO Component Revisions for PPO ID: {PpoId}",
                        ppoId
                    );
                    response.FillErrorInDataSource(
                        ppoComponentRevisions,
                        $"PPO Component Rate not saved!"
                    );
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating multiple PPO Component Revisions for PPO ID: {PpoId} with data count: {Count}",
                    ppoId,
                    ppoComponentRevisionDTOs.Count
                );
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
            _logger.LogInformation(
                "Updating PPO Component Revision with ID: {RevisionId} for financial year: {FinancialYear} and treasury code: {TreasuryCode}",
                revisionId,
                financialYear,
                treasuryCode
            );
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
                    _logger.LogWarning(
                        "PPO Component Revision not found for ID: {RevisionId}",
                        revisionId
                    );
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
                _logger.LogError(
                    ex,
                    "Error occurred while updating PPO Component Revision with ID: {RevisionId} and data: {Data}",
                    revisionId,
                    ppoComponentRevisionUpdateDTO
                );
                response.FillErrorInDataSource(
                    ppoComponentRevision,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            _logger.LogError(
                "Failed to update PPO Component Revision with ID: {RevisionId}",
                revisionId
            );
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
            _logger.LogInformation(
                "Deleting PPO Component Revision with ID: {RevisionId} for financial year: {FinancialYear} and treasury code: {TreasuryCode}",
                revisionId,
                financialYear,
                treasuryCode
            );
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
                    _logger.LogWarning(
                        "PPO Component Revision not found for ID: {RevisionId}",
                        revisionId
                    );
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
                _logger.LogError(
                    ex,
                    "Error occurred while deleting PPO Component Revision with ID: {RevisionId}",
                    revisionId
                );
                response.FillErrorInDataSource(
                    ppoComponentRevision,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            _logger.LogError(
                "Failed to delete PPO Component Revision with ID: {RevisionId}",
                revisionId
            );
            return response;
        }
    }
}
