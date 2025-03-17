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
    public class ComponentRateService : BaseService, IComponentRateService
    {
        private readonly PensionDbContext _context;
        private readonly IComponentRateRepository _pensionRateRepository;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;

        public ComponentRateService(
            IComponentRateRepository pensionRateRepository,
            PensionDbContext context,
            IClaimService claimService,
            IMapper mapper
        )
            : base(claimService)
        {
            _context = context;
            _claimService = claimService;
            _mapper = mapper;
            _pensionRateRepository = pensionRateRepository;
        }

        public async Task<TResponse> CreateComponentRates<TEntry, TResponse>(
            TEntry pensionRateEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            ComponentRate componentRateEntity = new() { Id = 0 };
            TResponse? response = _mapper.Map<TResponse>(componentRateEntity);

            try
            {
                componentRateEntity.FillFrom(pensionRateEntryDTO);
                SetCreatedBy(componentRateEntity);

                // Use AddAsync directly on the DbSet
                await _context.Set<ComponentRate>().AddAsync(componentRateEntity);

                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillDataSource(componentRateEntity, $"Component Rate not saved!");
                    return response;
                }
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    componentRateEntity,
                    $"ServiceException: {ex.InnerException?.Message}"
                );
            }
            finally
            {
                response.FillFrom(componentRateEntity);
            }
            return response;
        }

        public async Task<List<ComponentRateResponseDTO>> ListComponentRates(
            short financialYear,
            string treasuryCode
        )
        {
            return await _context
                .Set<ComponentRate>()
                .Where(entity => entity.ActiveFlag)
                .Select(entity => _mapper.Map<ComponentRateResponseDTO>(entity))
                .ToListAsync();
        }

        public async Task<List<TResponse>> ListComponentRatesByCategoryId<TResponse>(
            long categoryId
        )
        {
            var breakups = await _pensionRateRepository.GetComponentRatesByCategoryId<TResponse>(
                categoryId,
                entity => _mapper.Map<TResponse>(entity)
            );

            return breakups;
        }

        public async Task<List<TResponse>> GetPensionCategoriesWithRates<TResponse>(
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionRateRepository.GetPensionCategoriesWithRatesAsync<TResponse>();
        }
    }
}
