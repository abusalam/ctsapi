using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class AccountHeadSeeder(PensionDbContext context) : ISeeder
    {
        public void Seed(int count = 0)
        {
            if (context.AccountHeads.Any())
            {
                return; // Exit if there are already account heads in the database
            }

            var accountHeads = new[]
            {
                new AccountHead
                {
                    Id = 7415,
                    FinancialYear = 2023,
                    DemandNo = "18",
                    MajorHead = "2071",
                    SubmajorHead = "01",
                    MinorHead = "102",
                    PlanStatus = "NP",
                    SchemeHead = "001",
                    DetailHead = "04",
                    SubdetailHead = "00",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = false,
                },
                new AccountHead
                {
                    Id = 7418,
                    FinancialYear = 2023,
                    DemandNo = "18",
                    MajorHead = "2071",
                    SubmajorHead = "01",
                    MinorHead = "104",
                    PlanStatus = "NP",
                    SchemeHead = "001",
                    DetailHead = "04",
                    SubdetailHead = "00",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = false,
                },
                new AccountHead
                {
                    Id = 12148,
                    FinancialYear = 2023,
                    DemandNo = "27",
                    MajorHead = "2052",
                    SubmajorHead = "00",
                    MinorHead = "090",
                    PlanStatus = "NP",
                    SchemeHead = "001",
                    DetailHead = "01",
                    SubdetailHead = "04",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = false,
                },
                new AccountHead
                {
                    Id = 69545,
                    FinancialYear = 2023,
                    DemandNo = "18",
                    MajorHead = "2071",
                    SubmajorHead = "01",
                    MinorHead = "105",
                    PlanStatus = "00",
                    SchemeHead = "001",
                    DetailHead = "04",
                    SubdetailHead = "00",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new AccountHead
                {
                    Id = 69552,
                    FinancialYear = 2023,
                    DemandNo = "18",
                    MajorHead = "2071",
                    SubmajorHead = "01",
                    MinorHead = "109",
                    PlanStatus = "00",
                    SchemeHead = "001",
                    DetailHead = "04",
                    SubdetailHead = "00",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new AccountHead
                {
                    Id = 183560,
                    FinancialYear = 2023,
                    DemandNo = "18",
                    MajorHead = "2071",
                    SubmajorHead = "01",
                    MinorHead = "102",
                    PlanStatus = "00",
                    SchemeHead = "001",
                    DetailHead = "04",
                    SubdetailHead = "00",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new AccountHead
                {
                    Id = 197256,
                    FinancialYear = 2023,
                    DemandNo = "18",
                    MajorHead = "2071",
                    SubmajorHead = "01",
                    MinorHead = "101",
                    PlanStatus = "00",
                    SchemeHead = "005",
                    DetailHead = "04",
                    SubdetailHead = "00",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new AccountHead
                {
                    Id = 312757,
                    FinancialYear = 2024,
                    DemandNo = "18",
                    MajorHead = "2049",
                    SubmajorHead = "60",
                    MinorHead = "701",
                    PlanStatus = "00",
                    SchemeHead = "001",
                    DetailHead = "45",
                    SubdetailHead = "00",
                    VotedCharged = 'C',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new AccountHead
                {
                    Id = 312584,
                    FinancialYear = 2024,
                    DemandNo = "18",
                    MajorHead = "2071",
                    SubmajorHead = "01",
                    MinorHead = "104",
                    PlanStatus = "00",
                    SchemeHead = "004",
                    DetailHead = "04",
                    SubdetailHead = "00",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new AccountHead
                {
                    Id = 312596,
                    FinancialYear = 2024,
                    DemandNo = "18",
                    MajorHead = "2071",
                    SubmajorHead = "01",
                    MinorHead = "104",
                    PlanStatus = "00",
                    SchemeHead = "003",
                    DetailHead = "04",
                    SubdetailHead = "00",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
                new AccountHead
                {
                    Id = 332019,
                    FinancialYear = 2024,
                    DemandNo = "68",
                    MajorHead = "2052",
                    SubmajorHead = "00",
                    MinorHead = "090",
                    PlanStatus = "00",
                    SchemeHead = "001",
                    DetailHead = "01",
                    SubdetailHead = "04",
                    VotedCharged = 'V',
                    CreatedBy = 1,
                    ActiveFlag = true,
                },
            };
            if (count <= 11)
            {
                context.AccountHeads.AddRange(accountHeads);
            }
            else
            {
                context.AccountHeads.AddRange(accountHeads);
                var additionalAccountHeads = new List<AccountHead>();
                for (int i = 0; i < count - 11; i++)
                {
                    additionalAccountHeads.Add(
                        new AccountHead
                        {
                            Id = accountHeads.Max(x => x.Id) + i + 1,
                            FinancialYear = 2024,
                            DemandNo = "18",
                            MajorHead = "2071",
                            SubmajorHead = "01",
                            MinorHead = "102",
                            PlanStatus = "00",
                            SchemeHead = "004",
                            DetailHead = "04",
                            SubdetailHead = "00",
                            VotedCharged = 'V',
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                    );
                }
                context.AccountHeads.AddRange(additionalAccountHeads);
            }
            context.SaveChanges();
        }
    }
}
