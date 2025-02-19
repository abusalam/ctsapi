using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoByTransferRepository : IRepository<PpoBytransfer>
    {
        public Task<T> SavePpoByTransferHead<T>(PpoBytransfer ppobyTransferHeadEntity);
    }
}
