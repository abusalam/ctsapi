using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class FinancialYearSeeder(PensionDbContext context) : BaseSeeder, ISeeder
    {
        public void Seed(int count = 10)
        {
            if (context.FinancialYears.Any())
            {
                return;
            }

            context.FinancialYears.AddRange(
                Enumerable
                    .Range(_financialYear - count + 2, count)
                    .Select(year => new FinancialYear
                    {
                        CurrentYear = year,
                        CreatedBy = 1,
                        ActiveFlag = year == _financialYear,
                    })
                    .ToList()
            );
            context.SaveChanges();
        }
    }
}
