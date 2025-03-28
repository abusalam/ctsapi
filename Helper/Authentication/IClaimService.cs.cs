namespace CTS_BE.Helper.Authentication
{
    public interface IClaimService
    {
        public string[] GetRoles();
        public string GetRole();
        public string GetScope();
        public int GetUserId();
        public string GetUserName();
        public string[] GetPermissions();

        public short GetFinancialYear();
    }
}
