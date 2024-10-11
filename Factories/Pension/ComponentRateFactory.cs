using Bogus;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class ComponentRateFactory : BaseFactory<ComponentRateEntryDTO>
    {
        public ComponentRateFactory()
        {
            _faker = new Faker<ComponentRateEntryDTO>()
                .RuleFor(x => x.CategoryId, f => f.PickRandom(25, 29, 31))
                .RuleFor(x => x.BreakupId, f => f.Random.Int(1, 8))
                .RuleFor(
                    x => x.EffectiveFromDate,
                    f => DateOnly.FromDateTime(DateTime.Now)
                        .AddMonths(f.Random.Int(-90, -10))
                        .AddDays(f.Random.Int(1, 30))
                )
                .RuleFor(x => x.RateType, f => f.PickRandom('P','A'))
                .RuleFor(x => x.RateAmount, f => f.Random.Int(10, 90));
        }
    }
}