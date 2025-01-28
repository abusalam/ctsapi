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

            var desiredCategoryIds = new HashSet<long> { 30, 48, 31, 138 };
            var componentRates = new List<ComponentRate>();
            var factory = new ComponentRateFactory().Make(count);

            // Track existing combinations to avoid duplicates
            var existingCombinations =
                new HashSet<(long CategoryId, long BreakupId, DateOnly EffectiveFromDate)>();

            foreach (var (rateEntry, index) in factory.Select((r, i) => (r, i)))
            {
                foreach (var categoryId in desiredCategoryIds)
                {
                    var combination = (
                        categoryId,
                        rateEntry.BreakupId,
                        rateEntry.EffectiveFromDate
                    );

                    // Skip if combination already exists
                    if (existingCombinations.Contains(combination))
                    {
                        continue;
                    }

                    existingCombinations.Add(combination);

                    componentRates.Add(
                        new ComponentRate
                        {
                            Id = componentRates.Count + 1,
                            CategoryId = categoryId,
                            BreakupId = rateEntry.BreakupId,
                            EffectiveFromDate = rateEntry.EffectiveFromDate,
                            RateAmount = rateEntry.RateAmount,
                            RateType = rateEntry.RateType,
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                    );
                }
            }

            context.ComponentRates.AddRange(componentRates);
            context.SaveChanges();
        }
    }
}
