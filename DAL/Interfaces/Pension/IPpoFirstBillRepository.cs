using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoFirstBillRepository : IPpoBillRepository
    {
        public Task<T> GetPpoFirstBillByPpoId<T>(
            int ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<List<Pensioner>> GetPensionersForFirstBillGeneration(
            short financialYear,
            string treasuryCode
        );

        public Task<List<Pensioner>> GetPensionersForFirstBillPrint(
            short financialYear,
            string treasuryCode
        );
        public T GenerateFirstPensionBill<T>(
            Pensioner pensioner,
            InitiateFirstPensionBillEntryDTO ppoFirstBillEntryDTO,
            short financialYear,
            string treasuryCode
        );
    }
}
