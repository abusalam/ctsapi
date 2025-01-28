using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class EppoReceiptSeeder(PensionDbContext context, IMapper mapper) : BaseSeeder, ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            if (await context.EppoReceipts.AnyAsync())
            {
                return; // Exit if there are already eppo receipts in the database
            }

            var eppoReceipts = new List<EppoReceipt>();
            for (int i = 0; i < count; i++)
            {
                Random random = new Random();
                char[] religions = ['P', 'F', 'D'];
                eppoReceipts.Add(
                    new EppoReceipt
                    {
                        FinancialYear = _financialYear,
                        TreasuryCode = _treasuryCode,
                        PpoId = i + 1,
                        PensionApplnNo = $"12012{random.Next(100000, 999999):D6}",
                        FreshRevisionFlag = 'F',
                        PpoTypeCode = 'F',
                        PpoNo = $"PPO-{random.Next(100, 999):D3}-{random.Next(100, 999):D3}",
                        IssuingLetterNo = $"PRI/BUR/23/F/010{random.Next(1, 10)}",
                        IssuingLetterDate = DateOnly.FromDateTime(
                            DateTime.Now.AddDays(random.Next(-365, 0))
                        ),
                        PenCatId = random.Next(100, 999),
                        SanctionAuthority = "Sanction Authority: " + random.Next(1, 10).ToString(),
                        SanctionNo = "Sanction No: " + random.Next(1, 10).ToString(),
                        SanctionDate = DateOnly.FromDateTime(
                            DateTime.Now.AddDays(random.Next(-365, 0))
                        ),
                        ProvisionalPensionStatus = 'N',
                        Religion = religions[random.Next(religions.Length)],
                        PensionerName = "Pensioner Name: " + random.Next(1, 10).ToString(),
                        PensionerAddress = "Pensioner Address: " + random.Next(1, 10).ToString(),
                        MobileNumber = (9000000000 + new Random().Next(100000000)).ToString(),
                        AadhaarNo = random.Next(100000000, 999999999).ToString("D9"),
                        DateOfBirth = DateOnly.FromDateTime(
                            DateTime.Now.AddDays(random.Next(1, 301)).AddYears(-61)
                        ),
                        DateOfRetirement = DateOnly.FromDateTime(
                            DateTime.Now.AddDays(random.Next(-365, 0))
                        ),
                        DateOfDeath =
                            random.Next(2) == 0
                                ? (DateOnly?)null
                                : DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(-365, 0))),
                        QualifyingServiceGrossYears = random.Next(10, 15),
                        QualifyingServiceGrossMonths = random.Next(1, 12),
                        QualifyingServiceGrossDays = random.Next(1, 28),
                        EmployeeLastPay = random.Next(10000, 99999),
                        EmployeeLastPayNotional = random.Next(10000, 99999),
                        CommutedPensionAmount = random.Next(0, 1000),
                        Withdrawn = false,
                        WithdrawDate = null,
                        WithdrawReason = null,
                        CreatedBy = 1,
                        ActiveFlag = true,
                    }
                );
            }
            context.EppoReceipts.AddRange(eppoReceipts);
            await context.SaveChangesAsync();
            // Seed EppoNominees
            await new EppoNomineeSeeder(context, mapper).SeedAsync(count);

            // Seed EppoAmounts
            await new EppoAmountSeeder(context, mapper).SeedAsync(count);
        }

        public void Seed(int count = 1)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }
    }
}
