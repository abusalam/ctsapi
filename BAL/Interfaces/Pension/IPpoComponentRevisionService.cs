using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoComponentRevisionService
    {
        public Task<T> GetPposForComponentRevisions<T>(short financialYear, string treasuryCode);
        public Task<T> CreateSinglePpoComponentRevision<T>(
            int ppoId,
            PpoComponentRevisionEntryDTO ppoComponentRevisionDTO,
            short financialYear,
            string treasuryCode
        );

        public Task<List<T>> GetPpoComponentRevisionsByPpoId<T>(
            int ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<T> UpdatePpoComponentRevisionById<T>(
            long revisionId,
            PpoComponentRevisionUpdateDTO ppoComponentRevisionUpdateDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<T> DeletePpoComponentRevisionById<T>(
            long revisionId,
            short financialYear,
            string treasuryCode
        );
    }
}
