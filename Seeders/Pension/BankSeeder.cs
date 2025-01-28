using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class BankSeeder(PensionDbContext context) : ISeeder
    {
        public void Seed(int count = 0)
        {
            if (context.Banks.Any())
            {
                return;
            }

            var banks = new[]
            {
                new Bank
                {
                    Id = 1,
                    BankName = "ABHYUDAYA COOPERATIVE BANK LIMITED",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Bank
                {
                    Id = 2,
                    BankName = "ABU DHABI COMMERCIAL BANK",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Bank
                {
                    Id = 3,
                    BankName = "ADITYA BIRLA IDEA PAYMENTS BANK",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Bank
                {
                    Id = 4,
                    BankName = "AHMEDABAD MERCANTILE COOPERATIVE BANK",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Bank
                {
                    Id = 5,
                    BankName = "AHMEDNAGAR MERCHANTS CO-OP BANK LTD",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Bank
                {
                    Id = 6,
                    BankName = "AIRTEL PAYMENTS BANK LIMITED",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Bank
                {
                    Id = 7,
                    BankName = "AKOLA JANATA COMMERCIAL COOPERATIVE BANK",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Bank
                {
                    Id = 8,
                    BankName = "ALLAHABAD BANK",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Bank
                {
                    Id = 9,
                    BankName = "ALMORA URBAN COOPERATIVE BANK LIMITED",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new Bank
                {
                    Id = 10,
                    BankName = "AMBARNATH JAIHIND COOP BANK LTD AMBARNATH",
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
            };

            if (count <= 10)
            {
                context.Banks.AddRange(banks);
            }
            else
            {
                context.Banks.AddRange(banks);
                var additionalBanks = new List<Bank>();
                for (int i = 0; i < count - 10; i++)
                {
                    additionalBanks.Add(
                        new Bank
                        {
                            Id = banks.Max(x => x.Id) + i + 1,
                            BankName = $"Bank {banks.Max(x => x.Id) + i + 1}",
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                    );
                }
                context.Banks.AddRange(additionalBanks);
            }
            context.SaveChanges();
        }
    }
}
