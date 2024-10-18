using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoSanctionDetailsRepository : IRepository<PpoSanctionDetail>
    {
        public Task<PpoSanctionDetail?> GetSanctionDetailsByIdAsync(
            long ppoSanctionDetailsId,
            string treasuryCode
        );
        public Task<T> AddNewSanctionDetails<T>(
            PpoSanctionDetail ppoSanctionDetail,
            string treasuryCode
        );
        public Task<PpoSanctionDetail?> GetSanctionDetailsByPpoIdAsync(
            int ppoId,
            string treasuryCode
        );
        public Task<T> UpdateSanctionDetails<T>(
            PpoSanctionDetail ppoSanctionDetail,
            string treasuryCode
        );
    }
}