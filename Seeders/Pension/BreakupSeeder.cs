using AutoMapper;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.Factories.Pension;

namespace CTS_BE.Seeders.Pension
{
    public class BreakupSeeder(PensionDbContext context) : ISeeder
    {
        public void Seed(int count = 8)
        {
            if (context.Breakups.Any())
            {
                return;
            }

            // Create the "Basic Pension" component first with id=1
            var basicPensionBreakup = new Breakup
            {
                ComponentName = "BASIC PENSION",
                ComponentType = 'P',
                ReliefFlag = false,
                CreatedBy = 1,
                ActiveFlag = true,
            };

            context.Breakups.Add(basicPensionBreakup);

            var amountCommutedBreakup = new Breakup
            {
                ComponentName = "AMOUNT COMMUTED",
                ComponentType = 'D',
                ReliefFlag = false,
                CreatedBy = 39,
                ActiveFlag = true,
            };
            context.Breakups.Add(amountCommutedBreakup);
            context.SaveChanges();

            int additionalCount = Math.Max(6, count - 2);

            var breakups = new ComponentFactory()
                .Make(additionalCount)
                .Select(
                    (dto, index) =>
                        new Breakup
                        {
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
