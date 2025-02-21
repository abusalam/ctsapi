using Bogus;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class ByTransferHeadFactory : BaseFactory<ByTransferHeadEntryDTO>
    {
        private static readonly char[] ByTransferTypes = { 'P', 'R' };
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

        public ByTransferHeadFactory()
        {
            _faker = new Faker<ByTransferHeadEntryDTO>()
                .RuleFor(d => d.ByTransferType, f => f.PickRandom(ByTransferTypes))
                .RuleFor(
                    x => x.AccountHeadId,
                    f =>
                        f.PickRandom(
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
                            332019
                        )
                )
                .RuleFor(
                    d => d.ByTransferDescription,
                    f =>
                        $"{f.PickRandom(DescriptionPrefixes)} - {f.PickRandom(DescriptionSuffixes)}"
                )
                .RuleFor(x => x.AgBytransfer, f => f.Random.Bool());
        }
    }
}
