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

            var predefinedAccountHeads = new[]
            {
                new AccountHead
                {
                    Id = 7415,
                    FinancialYear = 2024,
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
                    FinancialYear = 2024,
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
                    FinancialYear = 2024,
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
                    FinancialYear = 2024,
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
                    FinancialYear = 2024,
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
                    FinancialYear = 2024,
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
                    FinancialYear = 2024,
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
                context.AccountHeads.AddRange(predefinedAccountHeads.Take(11));
            }
            else
            {
                context.AccountHeads.AddRange(predefinedAccountHeads);
                int additionalNeeded = count - predefinedAccountHeads.Length;
                var additionalAccountHeads = new List<AccountHead>();
                string[] demandNos = { "18", "27", "68" };
                string[] majorHeads = { "2071", "2052", "2049" };
                string[] submajorHeads = { "01", "00", "60" };
                string[] minorHeads = { "102", "104", "105", "109", "090", "701" };
                string[] planStatuses = { "00", "NP" };
                string[] schemeHeads = { "001", "003", "004", "005" };
                string[] detailHeads = { "04", "45", "01" };
                string[] subdetailHeads = { "00", "04" };
                char[] votedCharged = { 'V', 'C' };

                Random random = new();

                for (int i = 0; i < additionalNeeded; i++)
                {
                    additionalAccountHeads.Add(
                        new AccountHead
                        {
                            Id = predefinedAccountHeads.Max(x => x.Id) + i + 1,
                            FinancialYear = 2024,
                            DemandNo = demandNos[random.Next(demandNos.Length)],
                            MajorHead = majorHeads[random.Next(majorHeads.Length)],
                            SubmajorHead = submajorHeads[random.Next(submajorHeads.Length)],
                            MinorHead = minorHeads[random.Next(minorHeads.Length)],
                            PlanStatus = planStatuses[random.Next(planStatuses.Length)],
                            SchemeHead = schemeHeads[random.Next(schemeHeads.Length)],
                            DetailHead = detailHeads[random.Next(detailHeads.Length)],
                            SubdetailHead = subdetailHeads[random.Next(subdetailHeads.Length)],
                            VotedCharged = votedCharged[random.Next(votedCharged.Length)],
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
