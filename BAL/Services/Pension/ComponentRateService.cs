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
        private readonly ILogger<ComponentRateService> _logger;

        public ComponentRateService(
            IComponentRateRepository pensionRateRepository,
            PensionDbContext context,
            IClaimService claimService,
            IMapper mapper,
            ILogger<ComponentRateService> logger
        )
            : base(claimService)
        {
            _context = context;
            _claimService = claimService;
            _mapper = mapper;
            _pensionRateRepository = pensionRateRepository;
            _logger = logger;
        }

        public async Task<TResponse> CreateComponentRates<TEntry, TResponse>(
            TEntry pensionRateEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            _logger.LogInformation(
                "Creating component rate with data: {PensionRateEntryDTO}, Financial Year: {FinancialYear}, Treasury Code: {TreasuryCode}",
                pensionRateEntryDTO,
                financialYear,
                treasuryCode
            );
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
                    _logger.LogError(
                        "Failed to save component rate entity: {ComponentRateEntity}",
                        componentRateEntity
                    );
                    response.FillErrorInDataSource(
                        componentRateEntity,
                        $"Component Rate not saved!"
                    );
                    return response;
                }
                _logger.LogInformation(
                    "Component rate created successfully with ID: {Id}",
                    componentRateEntity.Id
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database update exception occurred while creating component rate with data: {Data}",
                    pensionRateEntryDTO
                );
                response.FillErrorInDataSource(
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
            _logger.LogInformation(
                "Listing component rates for financial year: {FinancialYear}, Treasury Code: {TreasuryCode}",
                financialYear,
                treasuryCode
            );
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
