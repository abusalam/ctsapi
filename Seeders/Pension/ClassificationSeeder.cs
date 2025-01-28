using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class ClassificationSeeder(PensionDbContext context) : ISeeder
    {
        public void Seed(int count = 0)
        {
            if (context.Classifications.Any())
            {
                return;
            }

            new AccountHeadSeeder(context).Seed(count);

            var classifications = new[]
            {
                new Classification
                {
                    Id = 1,
                    ClassificationName = "Retirement/Death Gratuities",
                    AccountHeadId = 7418,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'O',
                    ActiveFlag = false,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 2,
                    ClassificationName = "Commuted value of Pension",
                    AccountHeadId = 7415,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'O',
                    ActiveFlag = false,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 3,
                    ClassificationName = "Retiring Gratuity- Final",
                    AccountHeadId = 312596,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = true,
                },
                new Classification
                {
                    Id = 4,
                    ClassificationName = "Death Gratuity- Final",
                    AccountHeadId = 312584,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = true,
                },
                new Classification
                {
                    Id = 5,
                    ClassificationName = "PROV PENSION",
                    AccountHeadId = 332019,
                    DueDrawFlag = 'D',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 6,
                    ClassificationName = "Commuted Value of Pension- First Time",
                    AccountHeadId = 183560,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'D',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = true,
                },
                new Classification
                {
                    Id = 7,
                    ClassificationName = "Retiring Gratuity- Additional/Revision",
                    AccountHeadId = 312596,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 8,
                    ClassificationName = "Retiring Gratuity- Provisional",
                    AccountHeadId = 312596,
                    DueDrawFlag = 'D',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 9,
                    ClassificationName = "Death Gratuity- Provisional",
                    AccountHeadId = 312584,
                    DueDrawFlag = 'D',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 10,
                    ClassificationName = "Death Gratuity- Additional/Revision",
                    AccountHeadId = 312584,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 11,
                    ClassificationName = "Additional Commuted Value of Pension",
                    AccountHeadId = 183560,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'D',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 12,
                    ClassificationName = "Interest For Court Case Payment",
                    AccountHeadId = 312757,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 13,
                    ClassificationName = "Arrear (Adj.)",
                    AccountHeadId = 69545,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 14,
                    ClassificationName = "Recovery (Adj.)",
                    AccountHeadId = 12148,
                    DueDrawFlag = 'D',
                    ClassificationFlag = 'P',
                    ActiveFlag = false,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 15,
                    ClassificationName = "C.A. to Minister",
                    AccountHeadId = 12148,
                    DueDrawFlag = 'M',
                    ClassificationFlag = 'P',
                    ActiveFlag = false,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
                new Classification
                {
                    Id = 16,
                    ClassificationName = "LTA / Arrear",
                    AccountHeadId = 197256,
                    DueDrawFlag = 'P',
                    ClassificationFlag = 'P',
                    ActiveFlag = true,
                    CreatedBy = 1,
                    CommutedValuePension = false,
                },
            };
            var newClassifications = new List<Classification>();
            if (count <= classifications.Length)
            {
                newClassifications.AddRange(classifications);
            }
            else
            {
                newClassifications.AddRange(classifications);
                var additionalClassifications = new List<Classification>();
                Random random = new Random();
                int[] accountHeadIds = new[]
                {
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
                    332019,
                };

                for (int i = 0; i < count - classifications.Length; i++)
                {
                    additionalClassifications.Add(
                        new Classification
                        {
                            Id = classifications.Max(x => x.Id) + i + 1,
                            ClassificationName = "Additional Classification " + (i + 1),
                            AccountHeadId = accountHeadIds[random.Next(accountHeadIds.Length)],
                            DueDrawFlag = 'P',
                            ClassificationFlag = 'P',
                            ActiveFlag = true,
                            CreatedBy = 1,
                            CommutedValuePension = false,
                        }
                    );
                }
                newClassifications.AddRange(additionalClassifications);
            }

            context.Classifications.AddRange(newClassifications);
            context.SaveChanges();
        }
    }
}
