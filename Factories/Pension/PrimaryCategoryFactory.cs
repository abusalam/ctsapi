using System.Text.RegularExpressions;
using Bogus;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public partial class PrimaryCategoryFactory : BaseFactory<PensionPrimaryCategoryEntryDTO>
    {
        public PrimaryCategoryFactory()
        {
            _faker = new Faker<PensionPrimaryCategoryEntryDTO>()
                .RuleFor(
                    x => x.HoaId,
                    f => f.Random.Replace("#### - ## - ### - ## - ### - ? - ## - ##")
                )
                .RuleFor(
                    x => x.PrimaryCategoryName,
                    f => CapitalizeFirstLetter().Replace(
                        f.Random.Word(),
                        m => m.Value.ToUpper()
                    )
                );
        }
    }
}