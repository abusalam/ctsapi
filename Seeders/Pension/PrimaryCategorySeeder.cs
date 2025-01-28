using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.Factories.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class PrimaryCategorySeeder(PensionDbContext context, IMapper mapper)
        : BaseSeeder,
            ISeeder
    {
        public void Seed(int count = 10)
        {
            if (context.PrimaryCategories.Any())
            {
                return; // Exit if there are already primary categories in the database
            }

            new AccountHeadSeeder(context).Seed(count);

            context.PrimaryCategories.AddRange(
                new PrimaryCategoryFactory()
                    .Make(count)
                    .Select(dto => SetCreatedBy(mapper.Map<PrimaryCategory>(dto)))
                    .ToList()
            );
            context.SaveChanges();
        }
    }
}
