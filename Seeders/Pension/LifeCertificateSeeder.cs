using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class LifeCertificateSeeder(
        PensionDbContext context,
        IMapper mapper,
        IPpoIdSequenceRepository _ppoIdSequenceRepository,
        IManualPpoReceiptRepository _manualPpoReceiptRepository
    ) : BaseSeeder, ISeeder
    {
        public async Task SeedAsync(int count = 1)
        {
            if (await context.LifeCertificates.AnyAsync())
            {
                return; // Exit if there are already life certificates in the database
            }

            var pensionerSeeder = new PensionerSeeder(
                context,
                mapper,
                _manualPpoReceiptRepository,
                _ppoIdSequenceRepository
            );
            await pensionerSeeder.SeedAsync(count);

            var createdPensioners = pensionerSeeder.GetCreatedPensioners();

            var lifeCertificates = new List<LifeCertificate>();
            for (int i = 0; i < count; i++)
            {
                if (i >= createdPensioners.Count)
                {
                    break; // Exit if there are no more pensioners to assign
                }
                var pensioner = createdPensioners[i];
                var pensionerId = pensioner.Id;
                lifeCertificates.Add(
                    new LifeCertificate
                    {
                        Id = i + 1,
                        FinancialYear = _financialYear,
                        TreasuryCode = _treasuryCode,
                        PensionerId = pensionerId,
                        PpoId = createdPensioners[i].PpoId,
                        CreatedBy = 1,
                        ActiveFlag = true,
                    }
                );
            }

            await context.LifeCertificates.AddRangeAsync(lifeCertificates);
            await context.SaveChangesAsync();
        }

        public void Seed(int count = 1)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }
    }
}
