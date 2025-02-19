using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoByTransferService : IBaseService
    {
        public Task<T> SavePpoByTransferHead<T>(
            PpoByTransferEntryDTO ppoByTransferEntryDTO,
            short financialYear,
            string treasuryCode
        );
    }
}
