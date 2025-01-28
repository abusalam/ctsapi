using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class PpoBillSeeder(
        PensionDbContext context,
        IPpoBillRepository _ppoBillRepository,
        IMapper mapper,
        IManualPpoReceiptRepository _manualPpoReceiptRepository,
        IPpoIdSequenceRepository _ppoIdSequenceRepository
    ) : BaseSeeder, ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            if (await context.PpoBills.AnyAsync())
            {
                return; // Exit if there are already PpoBills in the database
            }

            new AccountHeadSeeder(context).Seed(count);
            new BranchSeeder(context).Seed(count);
            var billSeeder = new BillSeeder(context, _ppoBillRepository);
            await billSeeder.SeedAsync(count);

            // Get the bill IDs
            var billIds = await context.Bills.Select(b => b.Id).ToListAsync();

            var pensionerSeeder = new PensionerSeeder(
                context,
                mapper,
                _manualPpoReceiptRepository,
                _ppoIdSequenceRepository
            );
            await pensionerSeeder.SeedAsync(count); // Ensure this is asynchronous

            var createdPensioners = context.Pensioners.ToList();

            var ppoBills = new List<PpoBill>();
            for (int i = 0; i < count; i++)
            {
                Random random = new Random();
                if (i >= createdPensioners.Count)
                {
                    break; // Exit if there are no more pensioners to assign
                }
                var pensioner = createdPensioners[i];
                var branch = context.Branches.FirstOrDefault(b => b.Id == pensioner.BranchId);
                var pensionerId = pensioner.Id;
                if (branch != null)
                {
                    ppoBills.Add(
                        new PpoBill
                        {
                            FinancialYear = _financialYear,
                            TreasuryCode = _treasuryCode,
                            BillId = billIds[i],
                            PensionerId = pensionerId,
                            PpoId = createdPensioners[i].PpoId,
                            BillType = BillType.FirstBill,
                            GrossAmount = random.Next(0, 1000),
                            BytransferAmount = random.Next(0, 1000),
                            NetAmount = random.Next(0, 1000),
                            AccountHolderName = createdPensioners[i].AccountHolderName,
                            BankAcNo = createdPensioners[i].BankAcNo,
                            IfscCode = branch.IfscCode,
                            PaymentStatus = 'I',
                            CorrectionStatus = 'P',
                            FailedReason = "Processing",
                            Remarks = "Bill Generated",
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                    );
                }
                else
                {
                    return;
                }
            }
            context.PpoBills.AddRange(ppoBills);
            await context.SaveChangesAsync();
        }

        public void Seed(int count = 1)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }
    }
}
