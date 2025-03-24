using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoArrearBillRepository(PensionDbContext context, IMapper mapper)
        : PpoBillRepository(context, mapper),
            IPpoArrearBillRepository
    {
        private readonly PensionDbContext _pensionDbContext = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Pensioner?> GetPpoArrearBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Branch)
                .ThenInclude(entity => entity.Bank)
                .Include(entity => entity.PpoStatusFlags.Where(status => status.ActiveFlag))
                .FirstOrDefaultAsync();
        }

        public async Task<List<Pensioner>> GetPensionersForArrearBillGeneration(
            short financialYear,
            string treasuryCode
        )
        {
            return await _pensionDbContext
                .Pensioners.Where(entity =>
                    entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoStatusFlags.Any(entity =>
                        entity.ActiveFlag && entity.StatusFlag == PensionStatusFlag.PpoRunning
                    )
                    && entity.PpoBills.Any(entity => entity.ActiveFlag)
                )
                .Include(entity => entity.PpoStatusFlags.Where(entity => entity.ActiveFlag))
                .Include(entity => entity.PpoBills.Where(entity => entity.ActiveFlag))
                .Include(entity => entity.Branch)
                .ThenInclude(entity => entity.Bank)
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}
