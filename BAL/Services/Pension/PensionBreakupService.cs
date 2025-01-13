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
    public class PensionBreakupService : BaseService, IPensionBreakupService
    {
        private readonly IBreakupRepository _billBreakupRepository;
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;

        public PensionBreakupService(
            IBreakupRepository breakupRepository,
            PensionDbContext pensionDbContext,
            IClaimService claimService,
            IMapper mapper
        )
            : base(claimService)
        {
            _billBreakupRepository = breakupRepository;
            _pensionDbContext = pensionDbContext;
            _claimService = claimService;
            _mapper = mapper;
            _userId = _claimService.GetUserId();
        }

        public async Task<TResponse> CreatePensionBreakup<TEntry, TResponse>(
            TEntry pensionBreakupEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            Breakup breakupEntity = new() { Id = 0 };
            TResponse? response = _mapper.Map<TResponse>(breakupEntity);

            try
            {
                breakupEntity.FillFrom(pensionBreakupEntryDTO);

                var breakup = await _pensionDbContext.Breakups.FirstOrDefaultAsync(entity =>
                    entity.ActiveFlag && entity.ComponentName == breakupEntity.ComponentName
                );

                if (breakup != null)
                {
                    response.FillDataSource(breakupEntity, $"Breakup already exists!");
                    return response;
                }

                SetCreatedBy(breakupEntity);
                await _pensionDbContext.Breakups.AddAsync(breakupEntity);

                if (await _pensionDbContext.SaveChangesAsync() == 0)
                {
                    response.FillDataSource(breakupEntity, $"Breakup not saved!");
                    return response;
                }
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    breakupEntity,
                    $"ServiceException: {ex.InnerException?.Message}"
                );
            }
            finally
            {
                response.FillFrom(breakupEntity);
            }
            return response;
        }

        public async Task<List<PensionBreakupResponseDTO>> ListBreakup(
            short financialYear,
            string treasuryCode
        )
        {
            _dataCount = await _pensionDbContext.Breakups.CountAsync();
            return await _pensionDbContext
                .Breakups.Where(entity => entity.ActiveFlag)
                .Select(entity => _mapper.Map<PensionBreakupResponseDTO>(entity))
                .ToListAsync();
        }

        public async Task<List<TResponse>> GetBreakups<TResponse>(
            short financialYear,
            string treasuryCode
        )
        {
            return await _billBreakupRepository.GetBreakupsAsync(entity =>
                _mapper.Map<TResponse>(entity)
            );
        }
    }
}
