using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class FinancialYearSeeder : ISeeder
    {
        public void Seed(PensionDbContext context)
        {
            if (context.FinancialYears.Any())
            {
                return;
            }

            context.FinancialYears.AddRange(
                new FinancialYear
                {
                    CurrentYear = 2023,
                    CreatedBy = 1,
                    ActiveFlag = false,
                },
                new FinancialYear
                {
                    CurrentYear = 2024,
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new FinancialYear
                {
                    CurrentYear = 2025,
                    CreatedBy = 1,
                    ActiveFlag = false,
                },
                new FinancialYear
                {
                    CurrentYear = 2026,
                    CreatedBy = 1,
                    ActiveFlag = false,
                }
            );
        }
    }
}
