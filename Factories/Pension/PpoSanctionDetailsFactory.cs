using Bogus;
using CTS_BE.DTOs;

namespace CTS_BE.Factories.Pension
{
    public class PpoSanctionDetailsFactory : BaseFactory<PpoSanctionDetailsEntryDTO>
    {
        public PpoSanctionDetailsFactory()
        {
            _faker = new Faker<PpoSanctionDetailsEntryDTO>()
                .RuleFor(d => d.EmployeeGender, f => f.PickRandom('F', 'M'))
                .RuleFor(
                    d => d.EmployeeName,
                    (f,d) => f.Name.FullName(
                        d.EmployeeGender == 'M' ?
                        Bogus.DataSets.Name.Gender.Male :
                        Bogus.DataSets.Name.Gender.Female
                    )
                )
                .RuleFor(d => d.SanctionAuthority, f => f.Random.Words(1))
                .RuleFor(d => d.SanctionNo, f => f.Random.Replace("####/????/####"))
                .RuleFor(d => d.SanctionDate, f => f.Date.PastDateOnly(1))
                .RuleFor(d => d.EmployeeDob, f => f.Date.PastDateOnly(60))
                .RuleFor(d => d.EmployeeDateOfAppointment, f => f.Date.PastDateOnly(20))
                .RuleFor(d => d.EmployeeOffice, f => f.Random.Words(5))
                .RuleFor(d => d.EmployeeDesignation, f => f.Random.Words(1))
                .RuleFor(d => d.EmployeeLastPay, f => f.Random.Number(10, 80) * 1000)
                .RuleFor(d => d.AverageEmolument, f => f.Random.Number(10, 50) * 100)
                .RuleFor(d => d.EmployeeHrmsId, f => f.Random.Replace("###-###"))
                .RuleFor(d => d.IssuingAuthority, f => f.Random.Words(5))
                .RuleFor(d => d.IssuingLetterNo, f => f.Random.Replace("####/????/####"))
                .RuleFor(d => d.IssuingLetterDate, f => f.Date.PastDateOnly(1))
                .RuleFor(d => d.QualifyingServiceGrossYears, f => f.Random.Number(10, 30))
                .RuleFor(d => d.QualifyingServiceGrossMonths, f => f.Random.Number(0, 11))
                .RuleFor(d => d.QualifyingServiceGrossDays, f => f.Random.Number(0, 27))
                .RuleFor(
                    d => d.QualifyingServiceNetYears,
                    (f, d) => f.Random.Number(
                        5,
                        d.QualifyingServiceGrossYears ?? 10
                    )
                )
                .RuleFor(
                    d => d.QualifyingServiceNetMonths,
                    (f, d) => f.Random.Number(
                        0,
                        d.QualifyingServiceGrossMonths ?? 10
                    )
                )
                .RuleFor(
                    d => d.QualifyingServiceNetDays,
                    (f, d) => f.Random.Number(
                        0,
                        d.QualifyingServiceGrossDays ?? 20
                    )
                );
        }
    }
}