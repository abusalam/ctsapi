using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class CategorySeeder(PensionDbContext context, IMapper mapper) : ISeeder
    {
        public void Seed(int count = 12)
        {
            if (context.Categories.Any())
            {
                return;
            }

            new PrimaryCategorySeeder(context, mapper).Seed(count + 10);
            new SubCategorySeeder(context, mapper).Seed(count + 10);

            var categories = new[]
            {
                new Category
                {
                    Id = 25,
                    PrimaryCategoryId = 1,
                    SubCategoryId = 2,
                    CategoryName = "State Pension-ROPA 2009",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 26,
                    PrimaryCategoryId = 3,
                    SubCategoryId = 3,
                    CategoryName = "State Pension-ROPA 1998",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 29,
                    PrimaryCategoryId = 2,
                    SubCategoryId = 2,
                    CategoryName = "Education Pension-ROPA 2009",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 30,
                    PrimaryCategoryId = 1,
                    SubCategoryId = 6,
                    CategoryName = "Education Pension-ROPA 1998",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 31,
                    PrimaryCategoryId = 2,
                    SubCategoryId = 7,
                    CategoryName = "Education Pension-Pension Rules 1966(Pre 81)",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 58,
                    PrimaryCategoryId = 7,
                    SubCategoryId = 2,
                    CategoryName = "State Pension Family-ROPA 2009",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 59,
                    PrimaryCategoryId = 1,
                    SubCategoryId = 3,
                    CategoryName = "State Pension Family-ROPA 1998",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 136,
                    PrimaryCategoryId = 2,
                    SubCategoryId = 4,
                    CategoryName = "College( Government) Pension-ROPA 2019",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 138,
                    PrimaryCategoryId = 1,
                    SubCategoryId = 5,
                    CategoryName = "Education Pension-ROPA 2019",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 139,
                    PrimaryCategoryId = 7,
                    SubCategoryId = 10,
                    CategoryName = "Education Pension-ROPA 2019",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 140,
                    PrimaryCategoryId = 2,
                    SubCategoryId = 5,
                    CategoryName = "State Pension-ROPA 2019",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Category
                {
                    Id = 48,
                    PrimaryCategoryId = 1,
                    SubCategoryId = 7,
                    CategoryName = "College( Government) Pension-ROPA 2009",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
            };

            var newCategories = new List<Category>(categories);

            if (count > categories.Length)
            {
                var additionalCategories = new List<Category>();
                var existingCombinations = new HashSet<(long, long)>();

                // Populate existing combinations
                foreach (var category in context.Categories)
                {
                    existingCombinations.Add((category.PrimaryCategoryId, category.SubCategoryId));
                }

                // Add predefined categories to the existing combinations
                foreach (var category in categories)
                {
                    existingCombinations.Add((category.PrimaryCategoryId, category.SubCategoryId));
                }

                Random random = new Random();
                for (int i = 0; i < count - categories.Length; i++)
                {
                    long primaryCategoryId;
                    long subCategoryId;

                    do
                    {
                        primaryCategoryId = random.Next(1, 10);
                        subCategoryId = random.Next(1, 10);
                    } while (existingCombinations.Contains((primaryCategoryId, subCategoryId)));

                    existingCombinations.Add((primaryCategoryId, subCategoryId));

                    var categoryId = categories.Max(x => x.Id) + i + 1;

                    additionalCategories.Add(
                        new Category
                        {
                            Id = categoryId,
                            PrimaryCategoryId = primaryCategoryId,
                            SubCategoryId = subCategoryId,
                            CategoryName = "Category " + categoryId,
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                    );
                }
                newCategories.AddRange(additionalCategories);
            }

            context.Categories.AddRange(newCategories);
            context.SaveChanges();
            context.Database.ExecuteSqlRaw(
                "SELECT setval('cts_pension.categories_id_seq', {0}, true)",
                newCategories.Max(c => c.Id) + 1
            );
        }
    }
}
