using System.Linq.Expressions;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPpoByTransferService : IBaseService
    {
        Task<T> GetAllPpoByTransferById<T>(int ppoId, short financialYear, string treasuryCode)
            where T : BaseDTO, new();

        public Task<T> SavePpoByTransferHead<T>(
            PpoByTransferEntryDTO ppoByTransferEntryDTO,
            short financialYear,
            string treasuryCode
        )
            where T : BaseDTO, new();

        public Task<T> UpdatePPOByTransfer<T>(PpoByTransferUpdateDTO updateDTO);
        public Task<T> DeletePPOByTransfer<T>(long id);
    }
}
