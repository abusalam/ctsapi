using CTS_BE.DAL.Entities.Pension;

namespace CTS_BE.BAL.Services.Calculator
{
    public class PayCheckByDate
    {
        public static bool CheckRevisionDate(
            PpoComponentRevision ppoComponentRevision,
            DateOnly date
        )
        {
            return ppoComponentRevision.FromDate >= date && ppoComponentRevision.ToDate <= date;
        }
    }
}
