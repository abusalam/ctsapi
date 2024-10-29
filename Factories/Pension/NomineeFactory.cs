using Bogus;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class NomineeFactory : BaseFactory<NomineeEntryDTO>
    {
        private static readonly long[] BankIds = {1,2};
        private static readonly IDictionary<long, long[]> BranchIds = new Dictionary<long, long[]>
        {
            {1, new long[] {1,2}},
            {2, new long[] {3,4}}
        };
        public NomineeFactory()
        {
            _faker = new Faker<NomineeEntryDTO>()
                .RuleFor(d => d.SerialNo, f => f.Random.Int(1, 10))
                .RuleFor(d => d.NomineeType, f => f.PickRandom('5', '6', '0'))
                .RuleFor(d => d.NomineePriority, f => f.Random.Int(1, 5))
                .RuleFor(d => d.NomineeShare, f => f.Random.Int(1, 100))
                .RuleFor(d => d.NomineeName, f => f.Person.FullName)
                .RuleFor(d => d.Refused, f => f.PickRandom(true, false))
                .RuleFor(d => d.FamilyPension, f => f.PickRandom(true, false))
                .RuleFor(d => d.NomineeActive, f => f.PickRandom(true, false))
                .RuleFor(
                    d => d.DateOfBirth,
                    f => DateOnly.FromDateTime(
                        DateTime.Now.AddDays((f.Random.Number(1, 300))).AddYears(-5)
                    )
                )
                .RuleFor(d => d.BankAcNo, f => f.Random.Replace("################"))
                .RuleFor(d => d.IdentificationMark, f => f.Random.Words(1))
                .RuleFor(d => d.BankId, f => f.PickRandom(BankIds))
                .RuleFor(d => d.BranchId, (f, d) => f.PickRandom(BranchIds[d.BankId]))
                .RuleFor(
                    d => d.Relation,
                    f => f.PickRandom('F', 'M', 'H', 'W', 'S', 'D', 'B', 'T', 'E', 'I', 'A', 'C', 'O')
                );
        }
    }
}