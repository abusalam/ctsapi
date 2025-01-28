namespace CTS_BE.Seeders.Pension
{
    public abstract class BaseSeeder
    {
        protected short _financialYear = (short)(
            DateTime.Now.Month >= 4 ? DateTime.Now.Year : DateTime.Now.Year - 1
        );
        protected string _treasuryCode = "DAA";

        protected T SetCreatedBy<T>(T entity)
            where T : class
        {
            entity?.GetType()?.GetProperty("FinancialYear")?.SetValue(entity, _financialYear);
            entity?.GetType()?.GetProperty("TreasuryCode")?.SetValue(entity, _treasuryCode);
            entity?.GetType()?.GetProperty("CreatedBy")?.SetValue(entity, 1);
            entity?.GetType()?.GetProperty("CreatedAt")?.SetValue(entity, DateTime.Now);
            entity?.GetType()?.GetProperty("ActiveFlag")?.SetValue(entity, true);
            return entity!;
        }
    }
}
