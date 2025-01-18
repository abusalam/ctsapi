using System;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoIdSequenceRepository
        : Repository<PpoIdSequence, PensionDbContext>,
            IPpoIdSequenceRepository
    {
        private readonly PensionDbContext _context;

        public PpoIdSequenceRepository(PensionDbContext context)
            : base(context) => _context = context;

        public async Task<int> GetNextPpoId(short financialYear, string treasuryCode)
        {
            PpoIdSequence? ppoIdSequenceEntity = new();
            int seqValue = 0;

            try
            {
                // Use FirstOrDefaultAsync to get the entity
                ppoIdSequenceEntity = await _context.PpoIdSequences.FirstOrDefaultAsync(entity =>
                    entity.TreasuryCode == treasuryCode
                )!;

                if (ppoIdSequenceEntity?.NextSequenceValue > 0)
                {
                    ppoIdSequenceEntity.NextSequenceValue++;
                    _context.PpoIdSequences.Update(ppoIdSequenceEntity); // Update the entity
                    seqValue = ppoIdSequenceEntity.NextSequenceValue;
                }
                else
                {
                    ppoIdSequenceEntity = new()
                    {
                        TreasuryCode = treasuryCode,
                        NextSequenceValue = 1,
                    };
                    await _context.PpoIdSequences.AddAsync(ppoIdSequenceEntity);
                }

                // Save changes asynchronously
                if (await _context.SaveChangesAsync() > 0)
                {
                    seqValue = ppoIdSequenceEntity.NextSequenceValue;
                }
            }
            finally
            {
                if (seqValue == 0 && ppoIdSequenceEntity != null)
                {
                    ppoIdSequenceEntity.NextSequenceValue = 0;
                }
            }
            return ppoIdSequenceEntity.NextSequenceValue;
        }
    }
}
