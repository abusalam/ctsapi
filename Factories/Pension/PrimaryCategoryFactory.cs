using Bogus;
using CTS_BE.DAL;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public partial class PrimaryCategoryFactory : BaseFactory<PensionPrimaryCategoryEntryDTO>
    {
        public PrimaryCategoryFactory()
        {

            _faker = new Faker<PensionPrimaryCategoryEntryDTO>()
                .RuleFor(
                    x => x.AccountHeadId,
                    f => f.PickRandom(197256, 69552)
                )
                .RuleFor(
                    x => x.PrimaryCategoryName,
                    f => f.Random.Word() + " " + f.Random.Replace("######")
                );
        }
    }
}