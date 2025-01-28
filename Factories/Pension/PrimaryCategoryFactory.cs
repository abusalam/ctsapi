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
                    f =>
                        f.PickRandom(
                            7415,
                            7418,
                            12148,
                            69545,
                            69552,
                            183560,
                            197256,
                            312757,
                            312584,
                            312596,
                            332019
                        )
                )
                .RuleFor(
                    x => x.PrimaryCategoryName,
                    f => f.Random.Word() + " " + f.Random.Replace("######")
                );
        }
    }
}
