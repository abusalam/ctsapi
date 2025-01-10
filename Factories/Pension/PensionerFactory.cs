using Bogus;
using CTS_BE.BAL.Services.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class PensionerFactory : BaseFactory<PensionerEntryDTO>
    {
        private static readonly long[] BankIds = { 1, 2, 3, 4, 5 };
        private static readonly IDictionary<long, long[]> BranchIds = new Dictionary<long, long[]>
        {
            { 1, Enumerable.Range(1, 112).Select(i => (long)i).ToArray() }, // Bank ID 1 has branches 1 to 112
            { 2, new long[] { 113, 114 } }, // Bank ID 2 has branches 113 to 114
            { 3, new long[] { 115, 116 } }, // Bank ID 3 has branches 115 to 116
            { 4, Enumerable.Range(117, 35).Select(i => (long)i).ToArray() }, // Bank ID 4 has branches 117 to 151
            { 5, Enumerable.Range(152, 19).Select(i => (long)i).ToArray() }, // Bank ID 5 has branches 152 to 170
        };

        public PensionerFactory()
        {
            _faker = new Faker<PensionerEntryDTO>()
                .RuleFor(d => d.PpoNo, f => f.Random.Replace("PPO-###?###?"))
                .RuleFor(d => d.Gender, f => f.PickRandom('F', 'M'))
                .RuleFor(
                    d => d.PensionerName,
                    (f, d) =>
                        f.Name.FullName(
                            d.Gender == 'M'
                                ? Bogus.DataSets.Name.Gender.Male
                                : Bogus.DataSets.Name.Gender.Female
                        )
                )
                .RuleFor(d => d.PpoType, f => f.PickRandom('P', 'F', 'C'))
                .RuleFor(
                    d => d.PpoSubType,
                    f => f.PickRandom('E', 'L', 'U', 'V', 'N', 'R', 'P', 'G', 'J', 'K', 'H', 'W')
                )
                .RuleFor(d => d.CategoryId, f => f.PickRandom(30, 48))
                .RuleFor(d => d.PayMode, f => f.PickRandom('Q', 'B'))
                .RuleFor(d => d.BankAcNo, f => f.Random.Replace("################"))
                .RuleFor(d => d.BankId, f => f.PickRandom(BankIds))
                .RuleFor(d => d.BranchId, (f, d) => f.PickRandom(BranchIds[d.BankId]))
                .RuleFor(d => d.MobileNumber, f => f.Random.Replace("9#########"))
                .RuleFor(d => d.EmailId, f => f.Person.Email)
                .RuleFor(d => d.IdentificationMark, f => f.Random.Words(1))
                .RuleFor(d => d.PanNo, f => f.Random.Replace("?????####?"))
                .RuleFor(d => d.AadhaarNo, f => f.Random.Replace("############"))
                .RuleFor(d => d.AdhocPension, f => f.PickRandom(true, false))
                .RuleFor(d => d.DoublePension, f => f.PickRandom(true, false))
                .RuleFor(d => d.EfpAmount, f => f.Random.Number(1, 100))
                .RuleFor(d => d.EfpUptoDate, f => f.Date.PastDateOnly(60))
                .RuleFor(d => d.EfpWefDate, f => f.Date.PastDateOnly(60))
                .RuleFor(d => d.EmployedPensioner, f => f.Random.Bool())
                .RuleFor(d => d.ReEmployedPensioner, f => f.Random.Bool())
                .RuleFor(d => d.GpfTpfNo, f => f.Random.Replace("TPF-#####"))
                .RuleFor(d => d.HealthScheme, f => f.Random.Bool())
                .RuleFor(d => d.InterimAllowance, f => f.Random.Bool())
                .RuleFor(d => d.ProvisionalPension, f => f.Random.Bool())
                .RuleFor(d => d.SharedPension, f => f.Random.Bool())
                .RuleFor(d => d.NfpAmount, f => f.Random.Number(1, 100))
                .RuleFor(d => d.NfpWefDate, f => f.Date.PastDateOnly(60))
                .RuleFor(d => d.NotionalPensionAmount, f => f.Random.Number(1, 1000))
                .RuleFor(d => d.NotionalWefDate, f => f.Date.PastDateOnly(60))
                .RuleFor(d => d.BasicPensionAmount, f => f.Random.Number(100, 300) * 100)
                .RuleFor(d => d.CommutedPensionAmount, f => f.Random.Number(10, 50) * 100)
                .RuleFor(d => d.PensionerAddress, f => f.Address.FullAddress())
                .RuleFor(d => d.Religion, f => f.PickRandom('H', 'M', 'O'))
                .RuleFor(d => d.Remarks, f => f.Random.Words(5))
                .RuleFor(d => d.AccountHolderName, (f, d) => d.PensionerName)
                .RuleFor(d => d.EnhancePensionAmount, (f, d) => d.BasicPensionAmount)
                .RuleFor(
                    d => d.DateOfBirth,
                    f =>
                        DateOnly.FromDateTime(
                            DateTime.Now.AddDays((f.Random.Number(1, 300))).AddYears(-61)
                        )
                )
                .RuleFor(
                    d => d.DateOfRetirement,
                    (f, d) => PensionCalculator.CalculatePeriodEndDate(d.DateOfBirth.AddYears(60))
                )
                .RuleFor(d => d.DateOfCommencement, (f, d) => d.DateOfRetirement.AddDays(1))
                .RuleFor(d => d.CommutedFromDate, (f, d) => d.DateOfCommencement)
                .RuleFor(
                    d => d.ReducedPensionAmount,
                    (f, d) => (d.BasicPensionAmount - d.CommutedPensionAmount ?? 0)
                )
                .RuleFor(
                    d => d.CommutedUptoDate,
                    (f, d) => d.DateOfCommencement.AddYears(f.Random.Number(1, 10))
                );
        }
    }
}
