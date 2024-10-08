using Bogus;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class SubCategoryFactory : BaseFactory<PensionSubCategoryEntryDTO>
    {
        public SubCategoryFactory()
        {
            _faker = new Faker<PensionSubCategoryEntryDTO>()
                .RuleFor(
                    x => x.SubCategoryName,
                    f => f.Random.Word() + " " + f.Random.Replace("######")
                );
        }
    }
}