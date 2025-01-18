using System.Security.Claims;

namespace CTS_BE.Model.Claims
{
    public class AuthClaimModel
    {
        public List<Claim> Claims { get; set; } = [];
        public string RefreshedAccessToken { get; set; } = "";
    }
}
