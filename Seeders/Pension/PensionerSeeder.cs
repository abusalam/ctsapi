using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Factories.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Seeders.Pension
{
    public class PensionerSeeder(
        PensionDbContext context,
        IMapper mapper,
        IManualPpoReceiptRepository _manualPpoReceiptRepository,
        IPpoIdSequenceRepository _ppoIdSequenceRepository
    ) : BaseSeeder, ISeeder
    {
        private readonly List<Pensioner> _createdPensioners = [];

        public async Task SeedAsync(int count = 10)
        {
            if (await context.Pensioners.AnyAsync())
            {
                return; // Exit if there are already pensioners in the database
            }
            if (!await context.Categories.AnyAsync())
                new CategorySeeder(context, mapper).Seed(count);

            if (!await context.Branches.AnyAsync())
                new BranchSeeder(context).Seed(count);

            var pensioners = new List<Pensioner>();

            var pensionerDtos = new PensionerFactory().Make(count);
            for (int i = 0; i < pensionerDtos.Count; i++)
            {
                var dto = pensionerDtos[i];
                var pensionerEntity = SetCreatedBy(mapper.Map<Pensioner>(dto));

                pensionerEntity.PpoId = await _ppoIdSequenceRepository.GetNextPpoId(
                    _financialYear,
                    _treasuryCode
                );

                pensionerEntity.Receipt = SetCreatedBy(
                    mapper.Map<PpoReceipt>(new PpoReceiptFactory().Create())
                );
                pensionerEntity.Receipt.TreasuryReceiptNo =
                    _manualPpoReceiptRepository.GenerateTreasuryReceiptNo(
                        _financialYear,
                        _treasuryCode
                    );
                pensionerEntity.Receipt.PpoStatus = "PPO Received";
                pensionerEntity.Receipt.CreatedBy = 1;
                pensionerEntity.Receipt.ActiveFlag = true;

                _createdPensioners.Add(pensionerEntity);
                pensioners.Add(pensionerEntity);
                new PPOReceiptSequencesSeeder(context).Seed(i);
            }
            new PPOReceiptSequencesSeeder(context).Seed(count + 1);

            context.Pensioners.AddRange(pensioners);
            await context.SaveChangesAsync();
        }

        public void Seed(int count = 10)
        {
            SeedAsync(count).GetAwaiter().GetResult();
        }

        public List<Pensioner> GetCreatedPensioners() => _createdPensioners;
    }
}
