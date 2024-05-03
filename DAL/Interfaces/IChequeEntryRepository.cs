using CTS_BE.DAL.Entities;
namespace CTS_BE.DAL.Interfaces
{
    public interface IChequeEntryRepository : IRepository<ChequeEntry>
    {
        public Task<bool> NewChequeEntries(string chequeEntryData);
    }
}