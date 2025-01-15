using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.Helper.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class ConvertToFamilyPensionService : BaseService, IConvertToFamilyPensionService
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;

        public ConvertToFamilyPensionService(
            PensionDbContext pensionDbContext,
            IClaimService claimService,
            IMapper mapper
        )
            : base(claimService)
        {
            _pensionDbContext = pensionDbContext;
            _mapper = mapper;
        }

        public async Task<List<T>> GetPensioners<T>(string treasuryCode)
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoType != 'F'
                )
                .Select(entity => _mapper.Map<T>(entity))
                .ToListAsync();
        }
    }
}
