using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class EppoAmountSeeder(PensionDbContext context, IMapper mapper) : BaseSeeder, ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            if (await context.EppoAmounts.AnyAsync())
            {
                return; // Exit if there are already eppo amounts in the database
            }

            new ClassificationSeeder(context).Seed(count);
            new CategorySeeder(context, mapper).Seed(count + 12);

            // Seed EppoReceipts first
            await new EppoReceiptSeeder(context).SeedAsync(count);

            // Retrieve the IDs of the created EppoReceipts
            var eppoReceiptIds = await context
                .EppoReceipts.Where(er => er.ActiveFlag)
                .Select(er => er.Id)
                .ToListAsync();

            var eppoAmounts = new List<EppoAmount>();
            for (int i = 0; i < count; i++)
            {
                if (i >= eppoReceiptIds.Count)
                {
                    throw new InvalidOperationException(
                        $"Not enough Eppo receipts created. Expected at least {count}."
                    );
                }

                var eppoReceiptId = eppoReceiptIds[i];

                string[] amountTypes = { "CLS", "EFP", "BSC", "NFP", "BYT" };
                int[] categoryIds = new[] { 25, 26, 29, 30, 31, 58, 59, 136, 138, 139, 140, 48 };
                Random random = new Random();

                eppoAmounts.Add(
                    new EppoAmount
                    {
                        AmountType = amountTypes[random.Next(amountTypes.Length)],
                        ClassificationId = random.Next(1, 16),
                        FromDate = DateOnly.FromDateTime(
                            DateTime.Today.AddDays(random.Next(1, 365))
                        ),
                        ToDate = DateOnly.FromDateTime(DateTime.Today.AddDays(random.Next(1, 365))),
                        Amount = random.Next(1, 5000),
                        Consolidated = new Random().NextDouble() < 0.5,
                        CategoryId = categoryIds[random.Next(categoryIds.Length)],
                        EppoReceiptId = eppoReceiptId,
                        CreatedBy = 1,
                        ActiveFlag = true,
                    }
                );
            }

            await context.EppoAmounts.AddRangeAsync(eppoAmounts);
            await context.SaveChangesAsync();
        }

        public void Seed(int count = 1)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }
    }
}
