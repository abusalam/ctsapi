namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IPpoIdSequenceRepository
    {
        public Task<int> GetNextPpoId(short financialYear, string treasuryCode);
    }
}
