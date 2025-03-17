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

            new PrimaryCategorySeeder(context, mapper).Seed();
            new SubCategorySeeder(context, mapper).Seed();

            var predefinedCategories = new[]
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

            var allCategories = new List<Category>(predefinedCategories);

            if (count > predefinedCategories.Length)
            {
                var maxId = predefinedCategories.Max(x => x.Id);
                var random = new Random();

                var existingCombinations = new HashSet<(long, long)>();
                foreach (var category in predefinedCategories)
                {
                    existingCombinations.Add((category.PrimaryCategoryId, category.SubCategoryId));
                }

                var availableCombinations = new List<(long primaryId, long subId)>();
                for (long primaryId = 1; primaryId <= 9; primaryId++)
                {
                    for (long subId = 1; subId <= 9; subId++)
                    {
                        if (!existingCombinations.Contains((primaryId, subId)))
                        {
                            availableCombinations.Add((primaryId, subId));
                        }
                    }
                }

                for (int i = availableCombinations.Count - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    (availableCombinations[i], availableCombinations[j]) = (
                        availableCombinations[j],
                        availableCombinations[i]
                    );
                }

                int additionalNeeded = count - predefinedCategories.Length;
                int combinationIndex = 0;

                while (additionalNeeded > 0 && combinationIndex < availableCombinations.Count)
                {
                    var (primaryId, subId) = availableCombinations[combinationIndex];

                    allCategories.Add(
                        new Category
                        {
                            Id = maxId + combinationIndex + 1,
                            PrimaryCategoryId = primaryId,
                            SubCategoryId = subId,
                            CategoryName = "Category " + (maxId + combinationIndex + 1),
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                    );

                    additionalNeeded--;
                    combinationIndex++;
                }
            }

            context.Categories.AddRange(allCategories);
            context.SaveChanges();
            context.Database.ExecuteSqlRaw(
                "SELECT setval('cts_pension.categories_id_seq', {0}, true)",
                allCategories.Max(c => c.Id) + 1
            );
        }
    }
}
