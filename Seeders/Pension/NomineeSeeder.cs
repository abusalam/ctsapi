using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Factories.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class NomineeSeeder(
        PensionDbContext context,
        IMapper mapper,
        IManualPpoReceiptRepository _manualPpoReceiptRepository,
        IPpoIdSequenceRepository _ppoIdSequenceRepository
    ) : BaseSeeder, ISeeder
    {
        public void Seed(int count = 1)
        {
            if (context.Nominees.Any())
            {
                return; // Exit if there are already nominees in the database
            }
            new CategorySeeder(context, mapper).Seed(count);
            new BranchSeeder(context).Seed(count);
            var pensionerSeeder = new PensionerSeeder(
                context,
                mapper,
                _manualPpoReceiptRepository,
                _ppoIdSequenceRepository
            );
            pensionerSeeder.Seed(count);
            var pensioners = context.Pensioners.ToList();

            new NomineeFactory()
                .Make(count)
                .Select(
                    (dto, i) =>
                    {
                        var nomineeEntity = SetCreatedBy(mapper.Map<Nominee>(dto));
                        if (i < pensioners.Count)
                        {
                            nomineeEntity.Pensioner = pensioners[i];
                            nomineeEntity.PpoId = pensioners[i].PpoId;
                        }

                        return nomineeEntity;
                    }
                )
                .ToList()
                .ForEach(entity => context.Nominees.Add(entity));

            context.SaveChanges();
        }
    }
}
