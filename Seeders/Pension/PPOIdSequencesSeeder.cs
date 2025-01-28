using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class PPOIdSequencesSeeder(PensionDbContext context) : ISeeder
    {
        public void Seed(int count = 0)
        {
            if (context.PpoIdSequences.Any())
            {
                return;
            }

            context.PpoIdSequences.AddRange(
                new PpoIdSequence
                {
                    Id = 1,
                    FinancialYear = 0,
                    TreasuryCode = "BAB",
                    NextSequenceValue = 1,
                    CreatedBy = 1,
                    ActiveFlag = true,
                }
            );
            context.SaveChanges();
        }
    }
}
