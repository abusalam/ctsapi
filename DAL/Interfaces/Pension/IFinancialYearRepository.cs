namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IFinancialYearRepository
    {
        public Task<short> GetCurrentFinancialYearAsync();
    }
}
