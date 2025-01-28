using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class EppoRevisionSeeder(PensionDbContext context) : BaseSeeder, ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            if (await context.EppoRevisions.AnyAsync())
            {
                return; // Exit if there are already eppo revisions in the database
            }

            var eppoRevisions = new List<EppoRevision>();
            for (int i = 0; i < count; i++)
            {
                Random random = new Random();
                char[] religions = ['P', 'F', 'D'];
                eppoRevisions.Add(
                    new EppoRevision
                    {
                        FinancialYear = _financialYear,
                        TreasuryCode = _treasuryCode,
                        PpoId = i + 1,
                        PensionApplnNo = $"12012{random.Next(100000, 999999):D6}",
                        PpoNo = $"PPO-{random.Next(100, 999):D3}-{random.Next(100, 999):D3}",
                        IssuingLetterNo = $"PRI/BUR/23/F/010{random.Next(1, 10)}",
                        IssuingLetterDate = DateOnly.FromDateTime(
                            DateTime.Now.AddDays(random.Next(-365, 0))
                        ),
                        FreshRevisionFlag = 'P',
                        PpoTypeCode = 'P',
                        PpoSubType = 'N',
                        PenCatId = random.Next(100, 999),
                        EmployeeLastPay = random.Next(10000, 99999),
                        EmployeeLastPayNotional = random.Next(10000, 99999),
                        CommutedPensionAmount = random.Next(0, 1000),
                        CreatedBy = 1,
                        ActiveFlag = true,
                    }
                );
            }
            context.EppoRevisions.AddRange(eppoRevisions);
            await context.SaveChangesAsync();
        }

        public void Seed(int count = 1)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }
    }
}
