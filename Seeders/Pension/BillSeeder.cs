using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class BillSeeder(PensionDbContext context, IPpoBillRepository _ppoBillRepository)
        : BaseSeeder,
            ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            if (await context.Bills.AnyAsync())
            {
                return; // Exit if there are already bills in the database
            }

            new AccountHeadSeeder(context).Seed(count);
            new BranchSeeder(context).Seed(count);

            var random = new Random();
            var accountHeadIds = new[]
            {
                7415,
                7418,
                12148,
                69545,
                69552,
                183560,
                197256,
                312757,
                312584,
                312596,
                332019,
            };
            var branchIds = new[]
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
                11,
                12,
                13,
                14,
                15,
                16,
                17,
                19,
                20,
                113,
                114,
                115,
                116,
                117,
                118,
                152,
                153,
            };
            var bills = new List<Bill>();
            for (int i = 0; i < count; i++)
            {
                var billNo = await _ppoBillRepository.GetNextBillNo(_financialYear, _treasuryCode);
                var billDate = DateOnly.FromDateTime(DateTime.Today.AddDays(random.Next(1, 365))); // any future date
                var fromDate = billDate.AddMonths(1).AddDays(1 - billDate.Day); // 01. next month
                var toDate = fromDate.AddMonths(1).AddDays(-1); // last day of next month
                bills.Add(
                    new Bill
                    {
                        FinancialYear = _financialYear,
                        TreasuryCode = _treasuryCode,
                        AccountHeadId = accountHeadIds[random.Next(accountHeadIds.Length)],
                        BranchId = branchIds[random.Next(branchIds.Length)],
                        BillNo = billNo,
                        BillDate = billDate,
                        TreasuryVoucherNo = $"{_treasuryCode}-{billNo}",
                        TreasuryVoucherDate = billDate,
                        FromDate = fromDate,
                        ToDate = toDate,
                        GrossAmount = 10,
                        NetAmount = 10,
                        CreatedBy = 1,
                        ActiveFlag = true,
                    }
                );
            }
            await context.Bills.AddRangeAsync(bills);
            await context.SaveChangesAsync();
        }

        public void Seed(int count = 1)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }

        public async Task<int> GetBillIdAsync()
        {
            var bill = await context.Bills.OrderByDescending(b => b.Id).FirstOrDefaultAsync();
            if (bill != null)
            {
                return (int)bill.Id;
            }
            else
            {
                throw new InvalidOperationException("No bill found");
            }
        }
    }
}
