using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Factories.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class PpoSanctionDetailsSeeder(
        PensionDbContext context,
        IMapper mapper,
        IManualPpoReceiptRepository _manualPpoReceiptRepository,
        IPpoIdSequenceRepository _ppoIdSequenceRepository
    ) : BaseSeeder, ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            if (context.PpoSanctionDetails.Any())
            {
                return; // Exit if there are already ppo sanction details in the database
            }
            new CategorySeeder(context, mapper).Seed(count);
            new BranchSeeder(context).Seed(count);
            var pensionerSeeder = new PensionerSeeder(
                context,
                mapper,
                _manualPpoReceiptRepository,
                _ppoIdSequenceRepository
            );
            pensionerSeeder.Seed(count);
            var pensioners = await context.Pensioners.ToListAsync();

            var sanctionEntities = new PpoSanctionDetailsFactory()
                .Make(count)
                .Select(
                    (dto, i) =>
                    {
                        var sanctionEntity = SetCreatedBy(mapper.Map<PpoSanctionDetail>(dto));
                        if (i < pensioners.Count)
                        {
                            sanctionEntity.Pensioner = pensioners[i];
                            sanctionEntity.PpoId = pensioners[i].PpoId;
                        }

                        return sanctionEntity;
                    }
                )
                .ToList();

            context.PpoSanctionDetails.AddRange(sanctionEntities);
            await context.SaveChangesAsync();
        }

        public void Seed(int count = 1)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }
    }
}
