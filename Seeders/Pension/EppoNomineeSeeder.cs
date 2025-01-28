using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class EppoNomineeSeeder(PensionDbContext context, IMapper mapper) : BaseSeeder, ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            // Retrieve the IDs of the created EppoReceipts
            var eppoReceiptIds = await context
                .EppoReceipts.Where(er => er.ActiveFlag) // Assuming you want only active receipts
                .Select(er => er.Id)
                .ToListAsync();

            var eppoNominees = new List<EppoNominee>();
            for (int i = 0; i < count; i++)
            {
                // Ensure that we have enough EppoReceipt IDs
                if (i >= eppoReceiptIds.Count)
                {
                    throw new InvalidOperationException(
                        $"Not enough Eppo receipts created. Expected at least {count}."
                    );
                }

                var eppoReceiptId = eppoReceiptIds[i]; // Use the actual ID from the created receipts

                Random random = new();
                char[] nomineeTypes = { 'P', 'F', 'D' };
                char[] relations =
                {
                    'W',
                    'E',
                    'H',
                    'S',
                    'D',
                    'O',
                    'M',
                    'R',
                    'N',
                    'A',
                    'F',
                    'K',
                    'Y',
                    'C',
                    'U',
                    'I',
                    'T',
                    'J',
                    'B',
                    'P',
                    'V',
                    'L',
                };
                char[] nomineeAdultMinor = { 'A', 'M' };

                eppoNominees.Add(
                    new EppoNominee
                    {
                        NomineeType = nomineeTypes[random.Next(nomineeTypes.Length)],
                        SerialNo = i + 1,
                        NomineeName =
                            context
                                .EppoReceipts.FirstOrDefault(er => er.Id == eppoReceiptId)
                                ?.PensionerName ?? string.Empty,
                        DateOfBirth = DateOnly.FromDateTime(
                            DateTime.Now.AddDays(random.Next(1, 301)).AddYears(-61)
                        ),
                        Relation = relations[random.Next(relations.Length)],
                        NomineeShare = random.Next(1, 10),
                        NomineeAdultMinor = nomineeAdultMinor[
                            random.Next(nomineeAdultMinor.Length)
                        ],
                        EppoReceiptId = eppoReceiptId,
                        CreatedBy = 1,
                        ActiveFlag = true,
                    }
                );
            }

            await context.EppoNominees.AddRangeAsync(eppoNominees);
            await context.SaveChangesAsync();
        }

        public void Seed(int count = 1)
        {
            if (context.EppoNominees.Any())
            {
                return; // Exit if there are already eppo nominees in the database
            }

            // Create EppoReceipts first
            new EppoReceiptSeeder(context, mapper).Seed(count);
        }
    }
}
