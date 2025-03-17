using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoByTransferAmountService : IBaseService
    {
        //public Task<List<T>> GetByTransfersByPpoId<T>(
        //    int ppoId,
        //    short financialYear,
        //    string treasuryCode
        //);
        public Task<T> GetByTransfersByPpoId<T>(
            int ppoId,
            short financialYear,
            string treasuryCode
        );

        public Task<T> DeletePpoByTransferById<T>(long id);

        public Task<T> CreatePpoByTransfer<T>(
            int ppoId,
            PpoByTransferAmountEntryDTO ppoByTransferEntryDTO,
            short financialYear,
            string treasuryCode
        );

        public Task<T> UpdatePpoByTransfer<T>(long id, PpoByTransferAmountUpdateDTO updateDTO);
    }
}
