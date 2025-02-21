using Bogus;
using CTS_BE.BAL.Services.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class PpoByTransferFactory : BaseFactory<PpoByTransferEntryDTO>
    {
        private static readonly string[] DescriptionPrefixes =
        {
            "INCOME TAX",
            "SALES TAX",
            "WITHHOLDING TAX",
            "PAYROLL TAX",
        };
        private static readonly string[] DescriptionSuffixes =
        {
            "T.D.S",
            "ADVANCE TAX",
            "REFUND",
            "DEDUCTION",
        };

        public PpoByTransferFactory()
        {
            _faker = new Faker<PpoByTransferEntryDTO>()
                .RuleFor(
                    d => d.FromDate,
                    (f, d) =>
                        PensionCalculator.CalculatePeriodStartDate(
                            f.Date.PastDateOnly(
                                1,
                                DateOnly.FromDateTime(DateTime.Now).AddMonths(-2)
                            )
                        )
                )
                .RuleFor(d => d.ToDate, (f, d) => d.FromDate.AddDays(1))
                .RuleFor(d => d.BytransferAmount, f => f.Random.Int(1000, 10000))
                .RuleFor(
                    d => d.Remarks,
                    f =>
                        $"{f.PickRandom(DescriptionPrefixes)} - {f.PickRandom(DescriptionSuffixes)}"
                );
        }
    }
}
