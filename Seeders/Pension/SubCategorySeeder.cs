using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.Factories.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class SubCategorySeeder(PensionDbContext context, IMapper mapper) : BaseSeeder, ISeeder
    {
        public void Seed(int count = 10)
        {
            if (context.SubCategories.Any())
            {
                return; // Exit if there are already subcategories in the database
            }

            context.SubCategories.AddRange(
                new SubCategoryFactory()
                    .Make(count)
                    .Select(dto => SetCreatedBy(mapper.Map<SubCategory>(dto)))
                    .ToList()
            );
            context.SaveChanges();
        }
    }
}
