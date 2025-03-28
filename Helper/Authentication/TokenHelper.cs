using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CTS_BE.Model.Claims;
using Microsoft.IdentityModel.Tokens;

namespace CTS_BE.Helper.Authentication
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration _Configuration;
        private readonly ILogger<TokenHelper> _logger;
        private readonly string authSecretKey = "";
        private readonly string ActiveLifeTimeWindowinMint = "";

        public TokenHelper(ILogger<TokenHelper> logger, IConfiguration Configuration)
        {
            _logger = logger;
            _Configuration = Configuration;
            authSecretKey = _Configuration.GetValue<string>("Auth:SecretKey") ?? "";
            ActiveLifeTimeWindowinMint =
                _Configuration.GetValue<string>("Auth:ActiveLifeTimeWindowinMint") ?? "30";
        }

        /// <summary>
        /// out int LifetimeExpirtedFlag = 0 when the token is valid
        /// out int LifetimeExpirtedFlag = 1 when the token is invalid due any other reason
        /// out int LifetimeExpirtedFlag = 2 when the token is invalid due to its life time expiried only
        /// </summary>
        /// <param name="token"></param>
        /// <param name="LifetimeExpirtedFlag"></param>
        /// <returns></returns>
        public SecurityToken ValidateToken(string token, out int LifetimeExpirtedFlag)
        {
            SecurityToken validatedToken = null!;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                LifetimeExpirtedFlag = 0;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IDX10223: Lifetime validation failed."))
                    LifetimeExpirtedFlag = 2;
                else
                    LifetimeExpirtedFlag = 1;
                //_logger.LogError(ex);
                //IDX10223: Lifetime validation failed.
                validatedToken = null!;
            }

            return validatedToken;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token // need to add later
                ValidateIssuer = false, // Because there is no issuer in the generated token
                //ValidIssuer = "Sample", //// need to add later
                //ValidAudience = "Sample", // // need to add later
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSecretKey)), // The same key as the one that generate the token
            };
        }

        public AuthClaimModel ValidateAndGetTokenClaims(
            string token,
            out bool RefreshedAccessTokenRecieved
        )
        {
            SecurityToken? validToken = ValidateToken(token, out int LifetimeExpiredFlag);
            RefreshedAccessTokenRecieved = false;
            AuthClaimModel? authClaimModel = new()
            {
                RefreshedAccessToken = string.Empty,
                claims = [],
            };

            if (validToken != null)
            {
                // var cachedItem = _tokencache.GetItem(token); /* just to increase time to leave time in cache*/
                var tokenHandler = new JwtSecurityTokenHandler();

                if (tokenHandler.ReadToken(token) is JwtSecurityToken authToken)
                {
                    authClaimModel.claims = [.. authToken.Claims];
                }
            }
            return authClaimModel;
        }
    }
}
