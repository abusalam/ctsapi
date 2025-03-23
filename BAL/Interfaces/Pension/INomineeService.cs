using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface INomineeService
    {
        public Task<NomineeListResponseDTO> GetNomineeByPpoId(int ppoId, string treasuryCode);
        public Task<T> CreateNomineeDetails<T>(
            NomineeEntryDTO nomineeEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<T> UpdateNomineeDetailsById<T>(
            long nomineeId,
            NomineeEntryDTO nomineeEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<T> DeleteNomineeDetailsById<T>(long nomineeId, string treasuryCode);
        public Task<T> GetNomineeDetailsByNomineeId<T>(long nomineeId, string treasuryCode);
    }
}
