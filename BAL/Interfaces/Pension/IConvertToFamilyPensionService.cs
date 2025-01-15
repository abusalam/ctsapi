namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IConvertToFamilyPensionService : IBaseService
    {
        public Task<List<T>> GetPensioners<T>(string treasuryCode);
    }
}
