using Bogus;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class ComponentFactory : BaseFactory<PensionBreakupEntryDTO>
    {
        public ComponentFactory()
        {
            _faker = new Faker<PensionBreakupEntryDTO>()
                .RuleFor(
                    x => x.ComponentName,
                    f => CapitalizeFirstLetter().Replace(
                        f.Random.Word(),
                        m => m.Value.ToUpper()
                    ) + f.Random.Replace("######")
                )
                .RuleFor(
                    x => x.ComponentType,
                    f => f.PickRandom('P','D')
                )
                .RuleFor(
                    x => x.ReliefFlag,
                    f => f.Random.Bool()
                );
        }
    }
}