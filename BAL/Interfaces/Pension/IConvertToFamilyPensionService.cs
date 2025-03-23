namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IConvertToFamilyPensionService
    {
        public Task<List<T>> GetPensioners<T>(string treasuryCode);
    }
}
