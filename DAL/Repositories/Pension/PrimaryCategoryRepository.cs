using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PrimaryCategoryRepository
        : Repository<PrimaryCategory, PensionDbContext>,
            IPrimaryCategoryRepository
    {
        private readonly PensionDbContext _context;

        private readonly IMapper _mapper;

        public PrimaryCategoryRepository(IMapper mapper, PensionDbContext context)
            : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PensionPrimaryCategoryResponseDTO>> GetPrimaryCategoriesAsync()
        {
            return await _context
                .PrimaryCategories.Where(entity => entity.ActiveFlag)
                .Select(entity => new PensionPrimaryCategoryResponseDTO
                {
                    Id = entity.Id,
                    AccountHeadId = entity.AccountHeadId,
                    PrimaryCategoryName = entity.PrimaryCategoryName,
                    AccountHead = new AccountHeadResponseDTO
                    {
                        Id = entity.AccountHead.Id,
                        MajorHead = entity.AccountHead.MajorHead,
                        SubmajorHead = entity.AccountHead.SubmajorHead,
                        MinorHead = entity.AccountHead.MinorHead,
                        PlanStatus = entity.AccountHead.PlanStatus,
                        SchemeHead = entity.AccountHead.SchemeHead,
                        DetailHead = entity.AccountHead.DetailHead,
                        SubdetailHead = entity.AccountHead.SubdetailHead,
                        VotedCharged = entity.AccountHead.VotedCharged,
                    },
                })
                .ToListAsync();
        }

        public async Task<List<AccountHeadListItemResponseDTO>> GetAccountHeadsAsync(
            short financialYear,
            string treasuryCode
        )
        {
            return await _context
                .AccountHeads.Where(ah => ah.ActiveFlag)
                .Select(ah => new AccountHeadListItemResponseDTO
                {
                    Id = ah.Id,
                    HeadDetails =
                        $"{ah.MajorHead}-{ah.SubmajorHead}-{ah.MinorHead}-{ah.PlanStatus}-{ah.SchemeHead}-{ah.VotedCharged}-{ah.DetailHead}-{ah.SubdetailHead}",
                })
                .ToListAsync();
        }

        public async Task<PrimaryCategory?> GetPrimaryCategoryWithAccountHeadAsync(long id)
        {
            var primaryCategory = await _context
                .PrimaryCategories.Include(pc => pc.AccountHead)
                .FirstOrDefaultAsync(pc => pc.Id == id && pc.ActiveFlag);

            return primaryCategory;
        }
    }
}
