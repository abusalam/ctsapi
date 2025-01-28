using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.Factories.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class BreakupSeeder(PensionDbContext context) : ISeeder
    {
        public void Seed(int count = 10)
        {
            if (context.Breakups.Any())
            {
                return;
            }

            var breakups = new ComponentFactory()
                .Make(count + 8)
                .Select(
                    (dto, id) =>
                        new Breakup
                        {
                            Id = id + 1,
                            ComponentName = dto.ComponentName,
                            ComponentType = dto.ComponentType,
                            ReliefFlag = dto.ReliefFlag,
                            CreatedBy = 1,
                            ActiveFlag = true,
                        }
                )
                .ToList();

            context.Breakups.AddRange(breakups);
            context.SaveChanges();
        }
    }
}
