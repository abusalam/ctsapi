using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.Factories.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class ComponentRateSeeder(PensionDbContext context, IMapper mapper) : ISeeder
    {
        private readonly PensionDbContext context = context;
        private readonly IMapper mapper = mapper;
        private readonly long[] desiredCategoryIds =
        [
            25,
            26,
            30,
            48,
            31,
            138,
            58,
            59,
            136,
            139,
            140,
        ];

        public void Seed(int count = 10)
        {
            if (context.ComponentRates.Any())
            {
                return; // Exit if there are already component rates in the database
            }

            new BreakupSeeder(context).Seed();
            new CategorySeeder(context, mapper).Seed(count);

            var componentRates = new List<ComponentRate>();
            var factory = new ComponentRateFactory().Make(count);
            var rateEntries = factory.ToList();

            DateOnly effectiveDate = factory.First().EffectiveFromDate;

            for (int i = 0; i < desiredCategoryIds.Length; i++)
            {
                componentRates.Add(
                    new ComponentRate
                    {
                        CategoryId = desiredCategoryIds[i],
                        BreakupId = 1,
                        EffectiveFromDate = effectiveDate,
                        RateAmount = rateEntries[i % rateEntries.Count].RateAmount,
                        RateType = rateEntries[i % rateEntries.Count].RateType,
                        CreatedBy = 1,
                        ActiveFlag = true,
                    }
                );
            }

            if (count > desiredCategoryIds.Length)
            {
                var random = new Random();
                var existingCombinations = new HashSet<(long CategoryId, long BreakupId)>(
                    componentRates.Select(r => (r.CategoryId, r.BreakupId))
                );

                while (
                    componentRates.Count < count
                    && existingCombinations.Count < desiredCategoryIds.Length * 7
                )
                {
                    long categoryId = desiredCategoryIds[random.Next(desiredCategoryIds.Length)];

                    long breakupId = random.Next(2, 9);

                    if (!existingCombinations.Contains((categoryId, breakupId)))
                    {
                        componentRates.Add(
                            new ComponentRate
                            {
                                CategoryId = categoryId,
                                BreakupId = breakupId,
                                EffectiveFromDate = effectiveDate,
                                RateAmount = rateEntries[
                                    componentRates.Count % rateEntries.Count
                                ].RateAmount,
                                RateType = rateEntries[
                                    componentRates.Count % rateEntries.Count
                                ].RateType,
                                CreatedBy = 1,
                                ActiveFlag = true,
                            }
                        );

                        existingCombinations.Add((categoryId, breakupId));
                    }
                }
                if (componentRates.Count < count)
                {
                    var dayOffset = 1;
                    while (componentRates.Count < count)
                    {
                        foreach (var categoryId in desiredCategoryIds)
                        {
                            if (componentRates.Count >= count)
                                break;

                            componentRates.Add(
                                new ComponentRate
                                {
                                    CategoryId = categoryId,
                                    BreakupId = 1,
                                    EffectiveFromDate = effectiveDate.AddDays(dayOffset),
                                    RateAmount = rateEntries[
                                        componentRates.Count % rateEntries.Count
                                    ].RateAmount,
                                    RateType = rateEntries[
                                        componentRates.Count % rateEntries.Count
                                    ].RateType,
                                    CreatedBy = 1,
                                    ActiveFlag = true,
                                }
                            );
                        }
                        dayOffset++;
                    }
                }
            }

            context.ComponentRates.AddRange(componentRates);
            context.SaveChanges();
        }
    }
}
