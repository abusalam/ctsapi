namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface ITreasuryRepository
    {
        public Task<string> GetTreasuryNameAsync(string treasuryCode);
    }
}
