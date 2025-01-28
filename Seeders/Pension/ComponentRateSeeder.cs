using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.Factories.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class ComponentRateSeeder(PensionDbContext context, IMapper mapper) : ISeeder
    {
        public void Seed(int count = 10)
        {
            if (context.ComponentRates.Any())
            {
                return; // Exit if there are already component rates in the database
            }

            new BreakupSeeder(context).Seed(count);
            new CategorySeeder(context, mapper).Seed(count);

            var componentRates = new ComponentRateFactory()
                .Make(count)
                .Select(
                    (rateEntry, id) =>
                        new ComponentRate
                        {
                            Id = id + 1,
                            CategoryId = rateEntry.CategoryId,
                            BreakupId = rateEntry.BreakupId,
                            EffectiveFromDate = rateEntry.EffectiveFromDate,
                            RateAmount = rateEntry.RateAmount,
                            RateType = rateEntry.RateType,
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                )
                .ToList();

            context.ComponentRates.AddRange(componentRates);
            context.SaveChanges();
        }
    }
}
