using CTS_BE.Model.Claims;
using Microsoft.IdentityModel.Tokens;

namespace CTS_BE.Helper.Authentication
{
    public interface ITokenHelper
    {
        public SecurityToken ValidateToken(string token, out int LifetimeExpirtedFlag);
        AuthClaimModel ValidateAndGetTokenClaims(
            string token,
            out bool RefreshedAccessTokenRecieved
        );
    }
}
