using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class PPOReceiptSequencesSeeder(PensionDbContext context) : BaseSeeder, ISeeder
    {
        public void Seed(int count = 0)
        {
            if (context.PpoReceiptSequences.Any())
            {
                return;
            }

            context.PpoReceiptSequences.AddRange(
                new PpoReceiptSequence
                {
                    Id = 1,
                    FinancialYear = _financialYear,
                    TreasuryCode = _treasuryCode,
                    NextSequenceValue = 2,
                    CreatedBy = 1,
                    ActiveFlag = true,
                }
            );
            context.SaveChanges();
        }
    }
}
