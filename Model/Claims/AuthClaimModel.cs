using System.Security.Claims;

namespace CTS_BE.Model.Claims
{
    public class AuthClaimModel
    {
        public List<Claim> claims { get; set; } = null!;

        //  public List<Claim> Claims { get; set; } = new();
        public string RefreshedAccessToken { get; set; } = null!;
    }
}
