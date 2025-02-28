using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.Factories.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class ComponentRateSeeder : ISeeder
    {
        private readonly PensionDbContext context;
        private readonly IMapper mapper;

        public ComponentRateSeeder(PensionDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Seed(int count = 10)
        {
            if (context.ComponentRates.Any())
            {
                return; // Exit if there are already component rates in the database
            }

            new BreakupSeeder(context).Seed();
            new CategorySeeder(context, mapper).Seed(count);

            var desiredCategoryIds = new HashSet<long> { 30, 48, 31, 138 };
            var componentRates = new List<ComponentRate>();
            var factory = new ComponentRateFactory().Make(count);

            // Track existing combinations to avoid duplicates
            var existingCombinations =
                new HashSet<(long CategoryId, long BreakupId, DateOnly EffectiveFromDate)>();

            // Populate existing combinations from the database
            foreach (var existingRate in context.ComponentRates)
            {
                existingCombinations.Add(
                    (
                        existingRate.CategoryId,
                        existingRate.BreakupId,
                        existingRate.EffectiveFromDate
                    )
                );
            }

            // Ensure at least one ComponentRate with CategoryId 30
            if (!context.ComponentRates.Any(cr => cr.CategoryId == 30))
            {
                var firstRateEntry = factory.FirstOrDefault();
                if (firstRateEntry != null)
                {
                    var componentRate30 = new ComponentRate
                    {
                        CategoryId = 30,
                        BreakupId = firstRateEntry.BreakupId,
                        EffectiveFromDate = firstRateEntry.EffectiveFromDate,
                        RateAmount = firstRateEntry.RateAmount,
                        RateType = firstRateEntry.RateType,
                        CreatedBy = 1,
                        ActiveFlag = true,
                    };

                    // Add the entry for CategoryId 30 if it doesn't already exist
                    if (
                        !existingCombinations.Contains(
                            (30, firstRateEntry.BreakupId, firstRateEntry.EffectiveFromDate)
                        )
                    )
                    {
                        componentRates.Add(componentRate30);
                        existingCombinations.Add(
                            (30, firstRateEntry.BreakupId, firstRateEntry.EffectiveFromDate)
                        );
                    }
                }
            }

            foreach (var rateEntry in factory)
            {
                var categoryId = desiredCategoryIds.ElementAt(
                    new Random().Next(desiredCategoryIds.Count)
                );

                // Check if the combination already exists in the database
                if (
                    existingCombinations.Contains(
                        (categoryId, rateEntry.BreakupId, rateEntry.EffectiveFromDate)
                    )
                )
                {
                    continue; // Skip this entry if it already exists
                }

                // Add the new component rate
                componentRates.Add(
                    new ComponentRate
                    {
                        CategoryId = categoryId,
                        BreakupId = rateEntry.BreakupId,
                        EffectiveFromDate = rateEntry.EffectiveFromDate,
                        RateAmount = rateEntry.RateAmount,
                        RateType = rateEntry.RateType,
                        CreatedBy = 1,
                        ActiveFlag = true,
                    }
                );

                existingCombinations.Add(
                    (categoryId, rateEntry.BreakupId, rateEntry.EffectiveFromDate)
                );

                if (componentRates.Count >= count)
                {
                    break;
                }
            }

            context.ComponentRates.AddRange(componentRates);
            context.SaveChanges();
        }
    }
}
