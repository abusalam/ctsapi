using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PrimaryCategoryRepository(IMapper mapper, PensionDbContext context)
        : IPrimaryCategoryRepository
    {
        private readonly PensionDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<PrimaryCategory?> GetPrimaryCategoryById(long primaryCategoryId)
        {
            return await _context
                .PrimaryCategories.Where(entity =>
                    entity.ActiveFlag && entity.Id == primaryCategoryId
                )
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PrimaryCategoryExists(string primaryCategoryName)
        {
            return await _context.PrimaryCategories.AnyAsync(entity =>
                entity.ActiveFlag && entity.PrimaryCategoryName == primaryCategoryName
            );
        }

        public async Task<T> SavePrimaryCategoryAsync<T>(PrimaryCategory primaryCategory)
        {
            T? response = _mapper.Map<T>(primaryCategory);
            try
            {
                _context.PrimaryCategories.Add(primaryCategory);
                if (await _context.SaveChangesAsync() == 0)
                {
                    response.FillDataSource(primaryCategory, "Failed to add Primary Category!");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    primaryCategory,
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            return response;
        }

        public async Task<List<PrimaryCategory>> GetPrimaryCategoriesAsync()
        {
            return await _context
                .PrimaryCategories.Where(entity => entity.ActiveFlag)
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
