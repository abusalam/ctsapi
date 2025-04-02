using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoArrearBillRepository : IPpoBillRepository
    {
        public Task<Pensioner?> GetPpoArrearBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<List<Pensioner>> GetPensionersForArrearBillGeneration(
            short financialYear,
            string treasuryCode
        );

        public T GenerateArrearPensionBill<T>(
            Pensioner pensioner,
            PpoArrearBillEntryDTO ppoArrearBillEntryDTO,
            short financialYear,
            string treasuryCode
        );

        public Task<List<Pensioner>> GetPensionersForArrearBillPrint(
            short financialYear,
            string treasuryCode
        );
    }
}
