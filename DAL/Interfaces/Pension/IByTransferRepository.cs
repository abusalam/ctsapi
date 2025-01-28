using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IByTransferRepository : IRepository<BytransferHead>
    {
        Task<T> SaveByTransferHead<T>(BytransferHead byTransferHeadEntity);

        Task<BytransferHead?> GetByTransferHeadByIdAsync(long id);

        Task<List<BytransferHead>> GetAllByTransferHeadsAsync();
    }
}
