using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoIdSequenceRepository : Repository<PpoIdSequence, PensionDbContext>, IPpoIdSequenceRepository
    {
        public PpoIdSequenceRepository(PensionDbContext context) : base(context)
        {
        }

        public async Task<int> GetNextPpoId(
                short financialYear,
                string treasuryCode
            )
        {
            PpoIdSequence ppoIdSequenceEntity = new();
            int seqValue = 0;
            
            try {
                ppoIdSequenceEntity = await GetSingleAysnc(
                        entity 
                        => entity.TreasuryCode == treasuryCode
                    );
                if(ppoIdSequenceEntity?.NextSequenceValue > 0) {
                    //TODO: Not to increase when saving ppo details fails
                    ppoIdSequenceEntity.NextSequenceValue++;
                    if(Update(ppoIdSequenceEntity)) {
                        seqValue = ppoIdSequenceEntity.NextSequenceValue;
                    }

                } else {
                    ppoIdSequenceEntity = new () {
                            TreasuryCode = treasuryCode,
                            NextSequenceValue = 1
                        };
                    Add(ppoIdSequenceEntity);
                }
                if(await SaveChangesManagedAsync()>0) {
                    seqValue = ppoIdSequenceEntity.NextSequenceValue;
                }
            }
            finally {
                if(seqValue == 0 ) {
                    ppoIdSequenceEntity.NextSequenceValue = 0;
                }
            }
            return ppoIdSequenceEntity.NextSequenceValue;

        }
    }
}