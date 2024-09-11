using Bogus;
using CTS_BE.BAL.Services.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class PpoReceiptFactory : BaseFactory<ManualPpoReceiptEntryDTO>
    {
        public PpoReceiptFactory()
        {
            _faker = new Faker<ManualPpoReceiptEntryDTO>()
                // .StrictMode(true)
                .RuleFor(d => d.PpoNo, f => f.Random.Replace("PPO-###?###?"))
                .RuleFor(d => d.PensionerName, f => f.Person.FullName)
                .RuleFor(
                    d => d.DateOfCommencement,
                    (f,d) => PensionCalculator.CalculatePeriodStartDate(
                        f.Date.PastDateOnly(
                            1,
                            DateOnly.FromDateTime(DateTime.Now).AddMonths(-2)
                        )
                    )
                )
                .RuleFor(
                    d => d.ReceiptDate,
                    (f,d) => d.DateOfCommencement.AddDays(1)
                )
                .RuleFor(d => d.MobileNumber, f => f.Random.Replace("9#########"))
                .RuleFor(d => d.PsaCode, f => f.PickRandom('A','D','O'))
                .RuleFor(d => d.PpoType, f => f.PickRandom('N','R','P','O'))
                ;
        }
    }
}