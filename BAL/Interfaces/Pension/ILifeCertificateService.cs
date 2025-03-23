using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface ILifeCertificateService
    {
        public Task<T> GetLifeCertificateByPpoId<T>(long ppoId, string treasuryCode);
        public Task<LifeCertificateListResponseDTO> GetLifeCertificatesByBranchId(
            long branchId,
            short financialYear,
            string treasuryCode
        );
        public Task<T> CreateLifeCertificate<T>(
            LifeCertificateEntryDTO lifeCertificateEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<T> UpdateLifeCertificateByPpoId<T>(
            int ppoId,
            LifeCertificateEntryDTO lifeCertificateEntryDTO,
            short financialYear,
            string treasuryCode
        );
    }
}
