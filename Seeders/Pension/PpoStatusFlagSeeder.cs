using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class PpoStatusFlagSeeder(
        PensionDbContext context,
        IMapper mapper,
        IManualPpoReceiptRepository _manualPpoReceiptRepository,
        IPpoIdSequenceRepository _ppoIdSequenceRepository
    ) : BaseSeeder, ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            if (await context.PpoStatusFlags.AnyAsync())
            {
                return; // Exit if there are already PpoStatusFlags
            }

            if (!await context.Pensioners.AnyAsync())
            {
                var pensionerSeeder = new PensionerSeeder(
                    context,
                    mapper,
                    _manualPpoReceiptRepository,
                    _ppoIdSequenceRepository
                );
                await pensionerSeeder.SeedAsync(count);
            }

            var createdPensioners = await context.Pensioners.ToListAsync();
            var random = new Random();
            var ppoStatusFlags = new List<PpoStatusFlag>();

            foreach (var pensioner in createdPensioners)
            {
                var statusWef = DateOnly.FromDateTime(DateTime.Today.AddDays(-random.Next(0, 365)));

                ppoStatusFlags.Add(
                    new PpoStatusFlag
                    {
                        FinancialYear = _financialYear,
                        TreasuryCode = _treasuryCode,
                        PensionerId = pensioner.Id,
                        PpoId = pensioner.PpoId,
                        StatusWef = statusWef,
                        StatusFlag = PensionStatusFlag.PpoApproved,
                        CreatedBy = 1,
                        ActiveFlag = true,
                    }
                );
            }

            context.Set<PpoStatusFlag>().AddRange(ppoStatusFlags);
            await context.SaveChangesAsync();

            Console.WriteLine($"Seeded {ppoStatusFlags.Count} PpoStatusFlag records");
        }

        public void Seed(int count = 1)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }
    }
}
