using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    f => CapitalizeFirstLetter().Replace(
                        f.Random.Word(),
                        m => m.Value.ToUpper()
                    )
                );
        }
    }
}